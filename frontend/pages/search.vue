<script setup>
import { ref, watch, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';

// Получаем текущий маршрут
const route = useRoute();  
const router = useRouter();  

// Строка поиска и отфильтрованные товары
const searchQuery = ref(route.query.q || '');  // Извлекаем запрос из параметров URL
const filteredProducts = ref([]);  // Массив отфильтрованных товаров

// Функция фильтрации товаров (для работы с API)
const fetchProducts = async () => {
  try {
    // Отправляем запрос с параметром поиска
    const response = await fetch(`http://localhost:8080/product/name?name=${searchQuery.value}`);
    const data = await response.json();
    
    // Загружаем полученные товары
    filteredProducts.value = data;  // Здесь data — это список товаров, который пришел с сервера
  } catch (error) {
    console.error('Ошибка при загрузке товаров:', error);
  }
};

// Функция для перехода на страницу товара по его id
const goToProduct = (id) => {
  router.push(`/product/${id}`);  // Перенаправление на страницу товара
}

// Слежение за изменением параметра в URL для обновления фильтра
watch(() => route.query.q, (newQuery) => {
  searchQuery.value = newQuery || '';  // Обновляем строку поиска
  fetchProducts();  // Загружаем товары заново при изменении поиска
});

// Вызываем фильтрацию при монтировании страницы
onMounted(() => {
  fetchProducts();  // Загружаем товары при первой загрузке страницы
});
</script>

<template>
  <NavMenu />
  <div class="search-page">
    <h1>Результаты поиска для "{{ searchQuery }}"</h1>
    <div v-if="filteredProducts.length === 0">
        <p>Ничего не найдено.</p>
    </div>
    <div v-else>
        <div class="product-list">
            <!-- Перебор всех отфильтрованных товаров и отображение карточек -->
            <CardItem 
                v-for="product in filteredProducts" 
                :key="product.id" 
                :product="product" 
                @click="goToProduct(product.id)" 
            />
        </div>
    </div>
  </div>
</template>
