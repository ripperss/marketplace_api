<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import '@/assets/css/profile.css'

const router = useRouter()
const orders = ref([])
const isLoading = ref(true)

const fetchOrders = async () => {
    try {
        const response = await fetch('http://localhost:8080/order/orders', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
        })

        if (!response.ok) throw new Error('Ошибка при загрузке заказов')

        orders.value = await response.json()
    } catch (error) {
        console.error(error)
        router.push('/auth/login') // В случае ошибки, перенаправляем на страницу авторизации
    } finally {
        isLoading.value = false
    }
}

onMounted(fetchOrders)
</script>

<template>
    <NavMenu />
    <div class="max-w l mx-auto p-6 space-y-6 container">
        <h1 class="text-3xl font-bold text-center">Мои заказы</h1>

        <Card v-if="isLoading">
            <CardContent class="p-6 space-y-4">
                <Skeleton class="h-6 w-3/4" />
                <Skeleton class="h-4 w-1/2" />
            </CardContent>
        </Card>

        <Card v-else>
            <div v-if="orders.length === 0" class="text-center text-gray-500">
                У вас нет заказов
            </div>

            <div v-else class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <Card v-for="order in orders" :key="order.id" class="p-4">
                    <p><strong>Заказ #{{ order.id }}</strong></p>
                    <p>Сумма: {{ order.totalPrice }} ₽</p>
                    <p>Статус: {{ order.status === 0 ? 'Ожидает' : 'Завершен' }}</p>
                    <div class="mt-4">
                        <p><strong>Товары в заказе:</strong></p>
                        <ul>
                            <li v-for="product in order.products" :key="product.productId">
                                <img :src="product.product.imagePath" alt="product.name" class="w-30 h-20 inline-block mr-2" />
                                {{ product.product.name }} - {{ product.quantity }} шт.
                            </li>
                        </ul>
                    </div>
                </Card>
            </div>
        </Card>
    </div>
</template>
