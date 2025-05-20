<script setup>
import { useRoute } from 'vue-router';
import "@/assets/css/navbar.css";
import "@/assets/css/styles.css";

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
            <div class="topbar">
                <div class="topbar-l">
                    <NuxtLink to="/">
                        <img src="@/assets/img/topbar/bob3r.svg" alt="Лого" class="logo-img" >
                    </NuxtLink>
                </div>
                <div class="search-container">
                    <Input v-model="searchQuery" type="search" placeholder="Поиск"
                        class="search-input" @keyup.enter="handleKeyup" />
                    <img id=search1 src="@/assets/img/topbar/loopa.svg" alt="Поиск" class="logo-search">
                </div>
                <div class="topbar-r">
                    <div class="logo-img-w-txt">
                        <NuxtLink to="/delivery" exact-active-class="active">
                            <img src="@/assets/img/topbar/topbar_button_img_доставки.svg" alt="Доставка" class="logo-img-2">
                            <figcaption alt="Доставка" class="logo-img-txt">Доставка</figcaption>
                        </NuxtLink>
                    </div>

                    <div class="logo-img-w-txt">
                        <!--TODO: Добавть историю-->
                        <NuxtLink to="/cart"  exact-active-class="active">
                            <img src="@/assets/img/topbar/topbar_button_img_история.svg" alt="История" class="logo-img-2">
                            <figcaption alt="История" class="logo-img-txt">История</figcaption>
                        </NuxtLink>
                    </div>

                    <div class="logo-img-w-txt">
                        <NuxtLink to="/cart"  exact-active-class="active">
                            <img src="@/assets/img/topbar/topbar_button_img_корзина.svg" alt="Корзина" class="logo-img-2">
                            <figcaption alt="Корзина" class="logo-img-txt">Корзина</figcaption>
                        </NuxtLink>
                    </div>
                    
                    <div class="logo-img-w-txt">
                        <NuxtLink to="/profile"  exact-active-class="active">
                            <img src="@/assets/img/topbar/topbar_button_img_профиль.svg" alt="Профиль" class="logo-img-2">
                            <figcaption alt="Профиль" class="logo-img-txt">Профиль</figcaption>
                        </NuxtLink>
                    </div>

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
