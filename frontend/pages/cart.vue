<script setup>
import { ref, computed } from 'vue';
import '@/assets/css/cart.css'

// Пример списка товаров в корзине (потом можно заменить на данные с бэка)
const cartItems = ref([]);

// Увеличение количества
const increaseQuantity = (item) => {
    item.quantity++;
};

// Уменьшение количества
const decreaseQuantity = (item) => {
    if (item.quantity > 1) {
        item.quantity--;
    }
};




// Удаление товара
const removeItem = (id) => {
    cartItems.value = cartItems.value.filter(item => item.id !== id);
};

// Подсчет общей суммы заказа
const totalPrice = computed(() => {
    return cartItems.value.reduce((sum, item) => sum + item.price * item.quantity, 0);
});
</script>

<template>
    <NavMenu />
    <div class="cart-container">
        <h1>Корзина</h1>

        <div class="cart-grid">
            <!-- Колонка с товарами -->
            <div class="cart-items">
                <div v-for="item in cartItems" :key="item.id" class="cart-item">
                    <img src="@/assets/img/card.jpeg" :alt="item.name" class="cart-item-img" />
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



