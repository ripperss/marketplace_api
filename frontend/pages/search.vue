<script setup>
import { useRoute } from 'vue-router';
import { ref, computed } from 'vue';

const route = useRoute();
const searchQuery = ref(route.query.query || '');

// Здесь можно будет получить товары (или загрузить их через API)
const products = ref([
    { id: 1, name: 'Кроссовки', price: 1000, description: 'Модные кроссовки для бега' },
    { id: 2, name: 'Шапка', price: 500, description: 'Теплая зимняя шапка' },
    { id: 3, name: 'Футболка', price: 700, description: 'Хлопковая футболка с принтом' },
]);

const filteredProducts = computed(() => {
    return products.value.filter(product =>
        product.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        product.description.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
});
</script>

<template>
    <div class="search-results">
        <h1>Результаты поиска для "{{ searchQuery }}"</h1>
        <div class="search-items">
            <div v-for="product in filteredProducts" :key="product.id" class="product-item">
                <p>{{ product.name }}</p>
                <p>{{ product.price }} ₽</p>
            </div>
        </div>
        <div v-if="filteredProducts.length === 0">
            <p>Товары не найдены.</p>
        </div>
    </div>
</template>

<style scoped>
/* Стили для страницы с результатами поиска */
.search-items {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    gap: 10px;
}

.product-item {
    padding: 10px;
    background: #f9f9f9;
    border-radius: 5px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}
</style>
