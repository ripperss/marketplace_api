<script setup>
import { ref, computed, onMounted } from 'vue';
import '@/assets/css/cart.css';

// Список товаров в корзине
const cartItems = ref([]);

// Функция для загрузки корзины
const fetchCartItems = async () => {
  try {
    const response = await fetch('http://localhost:8080/cart/products', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include', // Если нужно отправлять куки
    });

    if (response.ok) {
      const data = await response.json();

      // Преобразуем структуру данных
      cartItems.value = data.map(item => ({
        id: item.product.id,
        name: item.product.name,
        price: item.product.price,
        description: item.product.description,
        imagePath: item.product.imagePath,
        quantity: item.quantity ?? 1, // Если quantity не передан, ставим 1
      }));

      console.log('Загруженные товары:', cartItems.value);
    } else {
      throw new Error('Ошибка при загрузке товаров из корзины');
    }
  } catch (error) {
    console.error(error);
  }
};

// Функция для удаления товара из корзины
const removeItem = async (productId) => {
  try {
    const response = await fetch(`http://localhost:8080/cart/product_del/${productId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
    });

    if (response.ok) {
      // Удаляем товар из локального списка после успешного ответа сервера
      cartItems.value = cartItems.value.filter(item => item.id !== productId);
      console.log(`Товар ${productId} удален из корзины`);
    } else {
      throw new Error('Ошибка при удалении товара из корзины');
    }
  } catch (error) {
    console.error(error);
  }
};

// Увеличение количества товара
const increaseQuantity = (item) => {
  item.quantity++;
};

// Уменьшение количества товара
const decreaseQuantity = (item) => {
  if (item.quantity > 1) {
    item.quantity--;
  }
};

// Подсчет общей суммы заказа
const totalPrice = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + item.price * item.quantity, 0);
});

// Загружаем корзину при монтировании компонента
onMounted(fetchCartItems);
</script>

<template>
  <NavMenu />
  <div class="cart-container">
    <h1>Корзина</h1>

    <div class="cart-grid">
      <!-- Колонка с товарами -->
      <div class="cart-items">
        <div v-for="item in cartItems" :key="item.id" class="cart-item">
          <img :src="item.imagePath" :alt="item.name" class="cart-item-img" />
          <div class="cart-item-info">
            <p class="cart-item-name">{{ item.name }}</p>
            <p class="cart-item-price">{{ item.price.toLocaleString() }} ₽</p>
            <div class="cart-item-controls">
              <button @click="decreaseQuantity(item)">-</button>
              <span>{{ item.quantity }}</span>
              <button @click="increaseQuantity(item)">+</button>
            </div>
          </div>
          <button class="cart-remove" @click="removeItem(item.id)">✖</button>
        </div>
      </div>

      <!-- Колонка с итоговой суммой -->
      <div class="cart-summary">
        <h2>Итого</h2>
        <p class="cart-total">Сумма: <strong>{{ totalPrice.toLocaleString() }} ₽</strong></p>
        <button class="cart-checkout">Оформить заказ</button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
