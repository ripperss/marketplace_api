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

// Массив товаров (в/ реальном проекте данные берутся из базы)
const product = ref(null); // Храним загруженный товар
const thumbsSwiper = ref(null);


const fetchProduct = async () => {
  try {
    const response = await fetch(`http://localhost:8080/product/product/${productId}`);
    const data = await response.json();
    product.value = {
      ...data,
      images: [data.imagePath] // Если API возвращает только 1 картинку
    };
  } catch (error) {
    console.error('Ошибка загрузки товара:', error);
  } finally {
    isLoading.value = false; // Скрываем загрузку
  }
};
// Загружаем данные при монтировании
onMounted(fetchProduct);


// Получаем параметры маршрута
const router = useRouter(); // Получаем доступ к маршрутизатору

// Функция "Назад"
const goBack = () => {
  router.go(-1); // Вызов router.back() для возврата на предыдущую страницу
};

const isLoading = ref(true);


const addToCart = async () => {
  try {
    const response = await fetch(`http://localhost:8080/cart/add_pdouct_cart/${productId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include', // Если сервер работает с куками
    });

    if (response.ok) {
      console.log(`Товар ${productId} успешно добавлен в корзину`);
    } else {
      throw new Error('Ошибка при добавлении товара в корзину');
    }
  } catch (error) {
    console.error(error);
  }
};


</script>

<template>
  <NavMenu />
  <div v-if="isLoading" class="loading">
    <div class="flex flex-col space-y-3">
      <Skeleton class="h-[350px] w-full rounded-lg" /> <!-- Скелетон для изображения -->
      <div class="space-y-4">
        <Skeleton class="h-8 w-[60%]" /> <!-- Скелетон для названия товара -->
        <Skeleton class="h-6 w-[30%]" /> <!-- Скелетон для цены -->
        <Skeleton class="h-4 w-full" /> <!-- Скелетон для описания -->
        <Skeleton class="h-4 w-[80%]" /> <!-- Скелетон для характеристик -->
      </div>
      <div class="buy-buttons">
        <Skeleton class="h-12 w-[48%] rounded-lg" /> <!-- Скелетон для кнопки "Добавить в корзину" -->
        <Skeleton class="h-12 w-[48%] rounded-lg" /> <!-- Скелетон для кнопки "Перейти в корзину" -->
      </div>
    </div>
  </div>
  <div v-else-if="product" class="product-page">
    <!-- Карусель с увеличением -->
    <div class="carousel-container">
      <div class="button--container">
        <nuxt-link :to="goBack">
          <button class="back--button" @click="goBack">← Назад</button>
        </nuxt-link>
      </div>
      <Swiper :modules="[Navigation, Thumbs, Zoom]" :navigation="false" :thumbs="{ swiper: thumbsSwiper }" :zoom="true"
        class="main-slider">
        <SwiperSlide v-for="(image, index) in product.images" :key="index">
          <div class="swiper-zoom-container">
            <img :src="product.imagePath" :alt="'Фото ' + (index + 1)" />
          </div>
        </SwiperSlide>
      </Swiper>



      <!-- Миниатюры -->
      <Swiper :modules="[Thumbs]" :space-between="10" :slidesPerView="4" @swiper="(swiper) => (thumbsSwiper = swiper)"
        class="thumb-slider">
        <SwiperSlide v-for="(image, index) in product.images" :key="index">
          <img :src="product.imagePath" :alt="'Миниатюра ' + (index + 1)" />
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
        <button class="add-to-cart" @click="addToCart">Добавить в корзину</button>
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

<style scoped></style>
