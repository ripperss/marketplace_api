<script setup>
defineProps({
  product: Object, // Принимаем объект product
});

import '@/assets/css/main.css';
</script>

<template>
  <div class="card-items">
    <img :src="product.imagePath" :alt="product.name" class="card-img" />
    <p class="card-coast">{{ product.price.toLocaleString() }} ₽</p>
    <p class="card--descripion">{{ product.name }}</p>
    <p class="card--ps">Осталось: {{ product.countProduct }} штук</p>
    <Button class="order--buton" @click="addToCard">Заказать</Button>
  </div>
</template>