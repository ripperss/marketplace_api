<script setup>
import '@/assets/css/main.css'


const product = {
  price: '10000',
  name: 'Кроссовки',
}

</script>


<template>

  <div class="card-items">
    <img src="@/assets/img/card.jpeg" :alt="product.name" class="card-img">
    <p class="card-coast"> {{ product.price.toLocaleString() }} ₽</p>
    <p class="card--descripion">
      {{ product.name }}
    </p>
    <Button class="order--buton" @click="addToCard">Заказать</Button>
  </div>
</template>
