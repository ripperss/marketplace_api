<script setup>
import { ref } from 'vue';
import { useRoute } from 'vue-router';
import { Carousel, CarouselContent, CarouselItem, CarouselPrevious, CarouselNext } from '@/components/ui/carousel';
import { Swiper, SwiperSlide } from 'swiper/vue';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/thumbs';
import 'swiper/css/zoom';
import { Navigation, Thumbs, Zoom } from 'swiper/modules';
import '@/assets/css/productPage.css'

// Получаем id товара из URL
const route = useRoute();
const productId = route.params.id;

// Массив товаров (в реальном проекте данные берутся из базы)
const products = [
  {
    id: 1,
    name: 'Кроссовки',
    price: 10000,
    description: 'Модные кроссовки для активных людей.',
    characteristics: 'Размер 42, цвет черный, материал: кожа',
    images: [
      '/img/product1.jpg',
      '/img/product2.jpg',
      '/img/product3.jpg',
    ],
  },
  {
    id: 2,
    name: 'Футболка',
    price: 2500,
    description: 'Качественная футболка из 100% хлопка.',
    characteristics: 'Размер M, цвет белый',
    images: [
      '/img/tshirt1.jpg',
      '/img/tshirt2.jpg',
    ],
  },
];

  // Получаем параметры маршрута
const router = useRouter(); // Получаем доступ к маршрутизатору

// Функция "Назад"
const goBack = () => {
  router.go(-1); // Вызов router.back() для возврата на предыдущую страницу
};


// Находим нужный товар
const product = products.find(p => p.id === Number(productId));

// Для миниатюр (thumbs)
const thumbsSwiper = ref(null);
</script>

<template>
  <NavMenu />

  <div v-if="product" class="product-page">
    <!-- Карусель с увеличением -->
    <div class="carousel-container">
      <div class="button--container">
        <nuxt-link :to="goBack">
      <button class="back--button" @click="goBack">← Назад</button>
    </nuxt-link>
    </div>
      <Swiper
        :modules="[Navigation, Thumbs, Zoom]"
        :navigation="false"
        :thumbs="{ swiper: thumbsSwiper }"
        :zoom="true"
        class="main-slider"
      >
        <SwiperSlide v-for="(image, index) in product.images" :key="index">
          <div class="swiper-zoom-container">
            <img src="@/assets/img/card.jpeg" :alt="'Фото ' + (index + 1)" />
          </div>
        </SwiperSlide>
      </Swiper>

      <!-- Миниатюры -->
      <Swiper
        :modules="[Thumbs]"
        :space-between="10"
        :slidesPerView="4"
        @swiper="(swiper) => (thumbsSwiper = swiper)"
        class="thumb-slider"
      >
        <SwiperSlide v-for="(image, index) in product.images" :key="index">
          <img src="@/assets/img/card.jpeg" :alt="'Миниатюра ' + (index + 1)" />
        </SwiperSlide>
      </Swiper>
    </div>

    <!-- Информация о товаре -->
    <div class="product-info">
      <h1>{{ product.name }}</h1>
      <p class="price">{{ product.price }} ₽</p>
      <p class="description">{{ product.description }}</p>
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
    <nuxt-link>
      <Button @click="goBack">Вернуться назад</Button>
    </nuxt-link>
  </div>
</template>

<style scoped>
</style>
