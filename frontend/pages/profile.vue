<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import '@/assets/css/profile.css'

const router = useRouter()
const user = ref(null)
const isLoading = ref(true)

const fetchProfile = async () => {
    try {
        const response = await fetch('http://localhost:8080/user/user', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
        })

        if (!response.ok) throw new Error('Не авторизован')

        user.value = await response.json()

        const ordersResponse = await fetch('http://localhost:8080/order/orders', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
        })

        if (ordersResponse.ok) {
            user.value.orders = await ordersResponse.json()
        } else {
            throw new Error('Ошибка при загрузке заказов')
        }

    } catch (error) {
        router.push('/auth/login')
    } finally {
        isLoading.value = false
    }
}

onMounted(() => {
    fetchProfile()
})

const logout = async () => {
    try {
        await fetch('http://localhost:8080/authuser/logout', {
            method: 'POST',
            credentials: 'include',
        })
    } catch (error) {
        console.error('Ошибка выхода:', error)
    }
    router.push('/auth/login')
}

const viewAllOrders = () => {
    router.push('/orders') // Перенаправляем на страницу с полным списком заказов
}
</script>

<template>
    <NavMenu />
    <div class="max-w-4xl mx-auto p-6 space-y-6 container">
        
        <h1 class="text-3xl font-bold text-center">Личный кабинет</h1>

        <Card v-if="isLoading">
            <CardContent class="p-6 space-y-4">
                <Skeleton class="h-6 w-3/4" />
                <Skeleton class="h-4 w-1/2" />
            </CardContent>
        </Card>

        <Card v-else-if="user">
            <CardHeader>
                <CardTitle>Добро пожаловать, {{ user.name }}</CardTitle>
            </CardHeader>
            <CardContent>
                <p><strong>Email:</strong> {{ user.email }}</p>
                <div class="flex gap-4 mt-4">
                    <Button @click="logout">Выйти</Button>
                    <Button v-if="user.role === 2" @click="router.push('/seller-dashboard')" variant="outline">Управление магазином</Button>
                    <Button v-if="user.role === 0" @click="router.push('/admin-panel')" variant="destructive">Панель администратора</Button>
                </div>
            </CardContent>
        </Card>

        <Card v-else>
            <CardContent class="p-6 text-center">
                <p class="text-red-500">Вы не вошли в систему</p>
                <Button @click="router.push('/auth/login')" class="mt-4">Войти</Button>
            </CardContent>
        </Card>

        <!-- Заказы -->
        <div class="orders mt-6">
            <h2 class="text-xl font-semibold">Мои последние заказы</h2>

            <div v-if="!user?.orders || user.orders.length === 0" class="text-gray-500">У вас пока нет заказов</div>

            <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Card v-for="order in user.orders.slice(0, 2)" :key="order.id" class="p-4">
                    <p><strong>Заказ #{{ order.id }}</strong></p>
                    <p>Сумма: {{ order.totalPrice }} ₽</p>
                    <p>Статус: {{ order.status === 0 ? 'Ожидает' : 'Завершен' }}</p>
                    
                    <!-- Список товаров с ссылками на страницы товара -->
                    <div class="mt-4">
                        <p><strong>Товары в заказе:</strong></p>
                        <ul>
                            <li v-for="product in order.products" :key="product.productId">
                                <router-link :to="`/product/${product.productId}`">
                                    <img :src="product.product.imagePath" alt="product.name" class="w-30 h-20 inline-block mr-2" />
                                    {{ product.product.name }}
                                </router-link>
                                - {{ product.quantity }} шт.
                            </li>
                        </ul>
                    </div>
                </Card>
            </div>

            <!-- Кнопка для просмотра всех заказов -->
            <div v-if="user?.orders.length > 2" class="text-center mt-4">
                <Button @click="viewAllOrders" variant="outline">Посмотреть все заказы</Button>
            </div>
        </div>
    </div>
</template>
