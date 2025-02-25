<script>
import "@/assets/css/navbar.css";
</script>

<template>

    <header>
        <nav>
            <div class="nav--container">
                <div class="nav--logo">
                    <p class="logo">WildBobr</p>
                </div>
                <div class="nav--search">
                    <Input type="search" placeholder="Найти на WildBobr" class="nav--search__place" />
                </div>
                <div class="nav--pc_items">
                    <a href="#" class="nav--pc_link">
                        <span class="nav--pc_icon nav--pc_icon__delivery">

                        </span>
                        Доставки
                    </a>
                    <a href="#" class="nav--pc_link">
                        <span class="nav--pc_icon nav--pc_icon__favorite">

                        </span>
                        Избранное
                    </a>
                    <a href="#" class="nav--pc_link">
                        <span class="nav--pc_icon nav--pc_icon__profile">

                        </span>
                        Профиль
                    </a>
                    <a href="#" class="nav--pc_link">
                        <span class="nav--pc_icon nav--pc_icon__cart">

                        </span>
                        Корзина
                    </a>

                </div>
            </div>
        </nav>
    </header>
</template>