<script setup lang="ts">
import '@/assets/css/main.css';
import NavMenu from '@/components/NavMenu/NavMenu.vue';
import CardItem from '@/components/CardItem/CardItem.vue';
import { useRouter } from 'vue-router';
import { ref, onMounted, computed } from 'vue';

// Типы данных
interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  // Добавьте другие поля, которые есть в вашем продукте
}

const products = ref<Product[]>([]); // Массив всех товаров
const displayedProducts = ref<Product[]>([]); // Товары, которые отображаются на текущей странице
const isLoading = ref<boolean>(true); // Состояние загрузки
const currentPage = ref<number>(1); // Текущая страница
const itemsPerPage = ref<number>(120); // Количество товаров на странице

// Функция для загрузки товаров
const fetchProducts = async () => {
  try {
    isLoading.value = true;
    const response = await fetch('http://localhost:8080/product/top_product');
    const data = await response.json();

    // Присваиваем все товары в массив
    products.value = data;

    // Распределяем товары по страницам
    updateDisplayedProducts();
  } catch (error) {
    console.error('Ошибка загрузки товаров:', error);
  } finally {
    isLoading.value = false; // После загрузки скрываем индикатор
  }
};

// Функция для обновления отображаемых товаров в зависимости от текущей страницы
const updateDisplayedProducts = () => {
  const startIndex = (currentPage.value - 1) * itemsPerPage.value;
  const endIndex = currentPage.value * itemsPerPage.value;
  displayedProducts.value = products.value.slice(startIndex, endIndex);
};

// Загружаем товары при монтировании компонента
onMounted(fetchProducts);

const router = useRouter();

// Функция для перехода на страницу товара
const goToProduct = (id: number) => {
  router.push(`/product/${id}`);
};

// Количество страниц
const totalPages = computed(() => Math.ceil(products.value.length / itemsPerPage.value));

// Функция для смены страницы
const changePage = (page: number) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    updateDisplayedProducts(); // Обновляем товары для текущей страницы
  }
};
</script>

<template>
  <NavMenu />

  <!-- Индикатор загрузки -->
  <div v-if="isLoading" class="loading">
    <div class="flex flex-wrap gap-4">
      <Skeleton 
        v-for="n in 100" 
        :key="n"
        class="h-[125px] w-[250px] rounded-xl" 
      />
    </div>
  </div>

  <!-- Показываем товары после загрузки -->
  <div v-else class="card">
    <CardItem 
      v-for="product in displayedProducts" 
      :key="product.id" 
      :product="product" 
      @click="goToProduct(product.id)" 
    />
  </div>

  <!-- Пагинация -->
  <div class="pagination">
    <button
      :disabled="currentPage === 1"
      @click="changePage(currentPage - 1)"
      class="pagination__button"
    >
      Назад
    </button>

    <span class="pagination__info">Страница {{ currentPage }} из {{ totalPages }}</span>

    <button
      :disabled="currentPage === totalPages"
      @click="changePage(currentPage + 1)"
      class="pagination__button"
    >
      Вперёд
    </button>
  </div>
</template>

<style scoped>

</style>
