<script setup>
import { useRoute } from 'vue-router';
import "@/assets/css/navbar.css";

const route = useRoute();
const router = useRouter();


const searchQuery = ref('');  // Объявляем переменную для поиска;

// Функция обработки поиска
const handleSearch = () => {
    if (searchQuery.value.trim()) {
        // Переход к той же странице с обновленным поисковым запросом
        router.push({ path: '/search', query: { q: searchQuery.value } });
    }
}


const handleKeyup = (event) => {
    if (event.key === 'Enter' && searchQuery.value.trim()) {
        handleSearch();
    }
};

</script>

<template>
    <header>
        <nav>
            <div class="nav--container">
                <div class="nav--logo">
                    <NuxtLink to="/" class="logo">WildBobr</NuxtLink>
                </div>
                <div class="nav--search">
                    <Input v-model="searchQuery" type="search" placeholder="Найти на WildBobr"
                        class="nav--search__place" @keyup.enter="handleKeyup" />
                </div>
                <div class="nav--pc_items">
                    <NuxtLink to="/delivery" class="nav--pc_link" exact-active-class="active">
                        <span class="nav--pc_icon nav--pc_icon__delivery"></span>
                        Доставки
                    </NuxtLink>
                    <NuxtLink to="/profile" class="nav--pc_link" exact-active-class="active">
                        <span class="nav--pc_icon nav--pc_icon__profile"></span>
                        Профиль
                    </NuxtLink>
                    <NuxtLink to="/cart" class="nav--pc_link" exact-active-class="active">
                        <span class="nav--pc_icon nav--pc_icon__cart"></span>
                        Корзина
                    </NuxtLink>
                </div>
            </div>

            <div class="nav--container_mob">
                <NuxtLink to="/" class="nav--pc_link" exact-active-class="active">
                    <span class="nav--pc_icon nav--mob_icon__home"></span>
                </NuxtLink>
                <NuxtLink to="/categories" class="nav--pc_link" exact-active-class="active">
                    <span class="nav--pc_icon nav--mob_icon__category"></span>
                </NuxtLink>
                <NuxtLink to="/cart" class="nav--pc_link" exact-active-class="active">
                    <span class="nav--pc_icon nav--pc_icon__cart"></span>
                </NuxtLink>
                <NuxtLink to="/profile" class="nav--pc_link" exact-active-class="active">
                    <span class="nav--pc_icon nav--pc_icon__profile"></span>
                </NuxtLink>
            </div>
        </nav>
    </header>
</template>
