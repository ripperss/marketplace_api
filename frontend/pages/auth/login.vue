<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import '@/assets/css/registr.css'
import NavMenu from '@/components/NavMenu/NavMenu.vue'

const router = useRouter()


const form = ref({
  email: '',
  password: ''
})

const login = async () => {
  try {
    const userResponse = await fetch('http://localhost:8080/authuser/log', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    })

    if (userResponse.ok) {
      const userData = await userResponse.json()
      console.log('Вход как пользователь:', userData)
      router.push('/profile') // Переход на страницу профиля
      return
    }

    const sellerResponse = await fetch('http://localhost:8080/AuthSeller/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    })

    if (sellerResponse.ok) {
      const sellerData = await sellerResponse.json()
      console.log('Вход как продавец:', sellerData)
      router.push('/seller-dashboard') // Переход в панель управления магазином
      return
    }

    console.error('Ошибка: неверные данные')
  } catch (error) {
    console.error('Ошибка входа:', error)
  }
}
</script>

<template>
    <NavMenu />
  <div class="flex min-h-screen items-center justify-center p-4">
    <div class="w-full max-w-md bg-white p-6 rounded-lg shadow-lg">
      <h2 class="text-xl font-semibold text-center mb-4">Вход</h2>

      <form @submit.prevent="login" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Email</label>
          <input
            v-model="form.email"
            type="email"
            placeholder="Введите email"
            class="w-full p-2 border rounded-md"
          />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Пароль</label>
          <input
            v-model="form.password"
            type="password"
            placeholder="Введите пароль"
            class="w-full p-2 border rounded-md"
          />
        </div>

        <button type="submit" class="w-full bg-var-color text-white p-2 rounded-md">
          Войти
        </button>
      </form>
    </div>
  </div>
</template>

<style scoped>
/* Дополнительные стили для мобильных экранов */

</style>
