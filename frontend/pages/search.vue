<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';  // Импортируем useRouter
import CardItem from '@/components/CardItem/CardItem.vue';  // Импортируем компонент карточки товара
import '@/assets/css/search.css'

const route = useRoute();  // Получаем текущий маршрут
const router = useRouter();  // Инициализируем router

const searchQuery = ref(route.query.q || '');  // Извлекаем запрос из параметров URL
const filteredProducts = ref([]);  // Массив отфильтрованных товаров

// Пример товаров (замени на реальный источник данных)
const products = [
    { id: 1, name: 'Кроссовки', price: 10000, description: 'Модные кроссовки' },
    { id: 2, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
    { id: 3, name: 'Куртка', price: 15000, description: 'Теплая зимняя куртка' },
    { id: 4, name: 'Сумка', price: 5000, description: 'Стильная сумка' },
    { id: 5, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
    { id: 6, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
    { id: 7, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
    { id: 8, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
    { id: 9, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
    { id: 10, name: 'Футболка', price: 2500, description: 'Качественная футболка' },
];

// Функция для перехода к странице товара по его id
const goToProduct = (id) => {
  router.push(`/product/${id}`);  // Перенаправление на страницу товара
}

// Функция фильтрации товаров
const filterProducts = () => {
    if (!searchQuery.value) {
        filteredProducts.value = products;
        return;
    }

    filteredProducts.value = products.filter(product =>
        product.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        product.description.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
};

// Слежение за изменением параметра в URL для обновления фильтра
watch(() => route.query.q, (newQuery) => {
    searchQuery.value = newQuery || '';  // Обновляем строку поиска
    filterProducts();  // Фильтруем товары заново
});

onMounted(() => {
    filterProducts();  // Вызываем фильтрацию при загрузке страницы
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
