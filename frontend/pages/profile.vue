<script setup>
import { NuxtLink } from '#components'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import NavMenu from '~/components/NavMenu/NavMenu.vue'

const router = useRouter()
const user = ref(null) // Данные пользователя
const isLoading = ref(true)

const goMain = () => {
  router.push('/auth/login') // Вызов router.back() для возврата на предыдущую страницу
};

// Функция загрузки профиля
const fetchProfile = async () => {
    try {
        const response = await fetch('http://localhost:8080/user/user', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        })

        if (!response.ok) throw new Error()

        user.value = await response.json()
    } catch (error) {
        console.error(error)
    } finally {
        isLoading.value = false
    }
}

onMounted(fetchProfile)

// Выход из аккаунта
const logout = () => {
    // Очищаем токены (если есть)
    localStorage.removeItem('token')
    router.push('/login') // Перенаправляем на страницу входа
}
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

                <div v-if="user.orders.length === 0">У вас пока нет заказов</div>

                <div v-else class="order-list">
                    <div v-for="order in user.orders" :key="order.id" class="order-card">
                        <p><strong>Заказ #{{ order.id }}</strong></p>
                        <p>Сумма: {{ order.total }} ₽</p>
                        <p>Статус: {{ order.status }}</p>
                    </div>
                </div>
            </div>
        </div>

        <div v-else class="error" ><NuxtLink > Вы не вошли<Button @click="goMain"> Войти</Button></NuxtLink></div>
        
    </div>
</template>