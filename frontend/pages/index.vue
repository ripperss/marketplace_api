<script setup>
import '@/assets/css/main.css';
import NavMenu from '@/components/NavMenu/NavMenu.vue';
import CardItem from '@/components/CardItem/CardItem.vue';
import { useRouter } from 'vue-router'; // Импортируем useRouter для перехода по маршруту
import '@/assets/css/main.css';

// Временные товары (позже заменим на API)
const products = [
  { id: 1, name: 'Кроссовки', price: 10000},
  { id: 2, name: 'Футболка', price: 2500},
  { id: 3, name: 'Куртка', price: 15000},
  { id: 4, name: 'Штаны', price: 5000},
  { id: 5, name: 'Шапка', price: 1200},
  { id: 6, name: 'Кроссовки Nike', price: 12000},
  { id: 7, name: 'Рюкзак', price: 4500},
  { id: 8, name: 'Перчатки', price: 800},
  { id: 9, name: 'Кроссовки', price: 10000},
  { id: 10, name: 'Футболка', price: 2500},
  { id: 11, name: 'Куртка', price: 15000},
  { id: 12, name: 'Штаны', price: 5000},
  { id: 13, name: 'Шапка', price: 1200},
  { id: 14, name: 'Кроссовки Nike', price: 12000},
  { id: 15, name: 'Рюкзак', price: 4500},
  { id: 16, name: 'Перчатки', price: 800},
];

const router = useRouter();

// Функция для перехода на страницу товара
const goToProduct = (id) => {
  router.push(`/product/${id}`);
}
</script>

<template>
  <NavMenu />

  <div class="card">
    <CardItem 
      v-for="product in products" 
      :key="product.id" 
      :product="product" 
      @click="goToProduct(product.id)" 
    />
  </div>
</template>