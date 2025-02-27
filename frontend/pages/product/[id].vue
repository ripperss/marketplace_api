<script setup>
import { useRoute } from 'vue-router'; // Импортируем useRoute для получения параметров маршрута

// Получаем параметр id из URL
const route = useRoute();
const productId = route.params.id;

// Временные данные товара (позже заменим на данные из базы)
const products = [
  { id: 1, name: 'Кроссовки', price: 10000, description: 'Модные кроссовки для активных людей.', characteristics: 'Размер 42, цвет черный, материал: кожа' },
  { id: 2, name: 'Футболка', price: 2500, description: 'Качественная футболка из 100% хлопка.', characteristics: 'Размер M, цвет белый' },
  { id: 3, name: 'Куртка', price: 15000, description: 'Теплая зимняя куртка с капюшоном.', characteristics: 'Размер L, цвет темно-синий' },
  // ...другие товары
];

// Находим товар по id
const product = products.find(p => p.id === Number(productId));
</script>

<template>

  <NavMenu />


  <div v-if="product" class="product-page">
    <!-- Изображение товара -->
    <img src="@/assets/img/card.jpeg" :alt="product.name" class="product-image" />

    <div class="product-info">
      <!-- Название товара -->
      <h1>{{ product.name }}</h1>
      <!-- Цена товара -->
      <p class="price">{{ product.price }} ₽</p>
      <!-- Описание товара -->
      <p class="description">{{ product.description }}</p>
      <!-- Характеристики товара -->
      <p class="characteristics">{{ product.characteristics }}</p>

      <!-- Кнопки -->
      <div class="buy-buttons">
        <button class="add-to-cart">Добавить в корзину</button>
        <nuxt-link to="/cart">
          <button class="go-to-cart">Перейти в корзину</button>
        </nuxt-link>
      </div>
    </div>
  </div>

  <div v-else class="not-found">
    <p class="not-found__text">Товар не найден</p>
    <nuxt-link to="/">
      <Button >Вернуться на главную</Button>
    </nuxt-link>
  </div>
</template>

<style scoped>
@import '@/assets/css/productPage.css';
/* Импортируем наш файл стилей */
</style>
