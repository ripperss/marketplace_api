<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'


const router = useRouter()
const user = ref(null)
const isLoading = ref(true)



const fetchProfile = async () => {
    try {
        const response = await fetch('http://localhost:8080/user/user', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include' // ВАЖНО! Чтобы сервер видел куки
        })

        if (!response.ok) {
            throw new Error('Не авторизован')
        }

        user.value = await response.json()
    } catch (error) {
        console.warn('Пользователь не авторизован, редирект на логин')
        router.push('/auth/login')
    } finally {
        isLoading.value = false
    }
}

onMounted(fetchProfile)

const logout = async () => {
    
    try {
        await fetch('http://localhost:8080/authuser/logout', { 
            method: 'POST', 
            credentials: 'include',
        })
    } catch (error) {
        console.error('Ошибка выхода:', error)
    }

    router.push('/auth/login') // Перенаправляем на логин
}

const goMain = () => {
  router.push('/auth/login') // Вызов router.back() для возврата на предыдущую страницу
};

</script>







<template>
    <div class="container">
        <NavMenu />
        <h1 class="text-2xl font-bold">Личный кабинет</h1>

        <!-- Индикатор загрузки -->
        <div v-if="isLoading" class="loading">Загрузка...</div>

        <div v-else-if="user" class="profile">
            <!-- Информация о пользователе -->
            <div class="profile-info">
                <p><strong>Имя:</strong> {{ user.name }}</p>
                <p><strong>Email:</strong> {{ user.email }}</p>
            </div>

            <!-- Кнопки для разных типов пользователей -->
            <div class="buttons">
                <button @click="logout" class="btn">Выйти</button>

                <button v-if="user.role === 'seller'" @click="router.push('/seller-dashboard')" class="btn">
                    Управление магазином
                </button>

                <button v-if="user.role === 'admin'" @click="router.push('/admin-panel')" class="btn">
                    Панель администратора
                </button>
            </div>

            <!-- История заказов -->
            <div class="orders">
                <h2 class="text-xl font-semibold">Мои заказы</h2>

                <div div v-if="!user.orders || user.orders.length === 0">У вас пока нет заказов</div>

                <div v-else class="order-list">
                    <div v-for="order in user.orders" :key="order.id" class="order-card">
                        <p><strong>Заказ #{{ order.id }}</strong></p>
                        <p>Сумма: {{ order.total }} ₽</p>
                        <p>Статус: {{ order.status }}</p>
                    </div>
                </div>
            </div>
        </div>



        <div v-else class="error" ><NuxtLink > Вы не вошли<Button @click="pus"> Войти</Button></NuxtLink></div>
        

        <div class="history">
            <CardItem 
                v-for="product in filteredProducts" 
                :key="product.id" 
                :product="product" 
                @click="goToProduct(product.id)" 
            />
        </div>
    </div>
</template>