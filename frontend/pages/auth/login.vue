<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import '@/assets/css/registr.css'
import NavMenu from '@/components/NavMenu/NavMenu.vue'

const router = useRouter()

const form = ref({
  email: '',
  hashPassword: ''
})

const errorMessage = ref('')

const login = async () => {
  errorMessage.value = '' // Очищаем ошибку перед новой попыткой

  try {
    // Вход как пользователь
    const userResponse = await fetch('http://localhost:8080/authuser/log', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    })

    if (userResponse.ok) {
      console.log('Вход как пользователь')
      router.push('/profile') // Перенаправление на страницу профиля
      return
    }

    // Вход как продавец
    const sellerResponse = await fetch('http://localhost:8080/authseller/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    })

    if (sellerResponse.ok) {
      console.log('Вход как продавец')
      router.push('/seller-dashboard') // Перенаправление в панель продавца
      return
    }

    // Если оба запроса неуспешны
    errorMessage.value = 'Неверный email или пароль'
  } catch (error) {
    console.error('Ошибка входа:', error)
    errorMessage.value = 'Ошибка сервера. Попробуйте позже'
  }
}
</script>



<template>
  <NavMenu />
  <div class="flex min-h-screen items-center justify-center p-4">
    <div class="w-full max-w-md bg-white p-6 rounded-lg">
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
            v-model="form.hashPassword"
            type="password"
            placeholder="Введите пароль"
            class="w-full p-2 border rounded-md"
          />
        </div>

        <p v-if="errorMessage" class="text-red-500 text-sm text-center">{{ errorMessage }}</p>

        <button type="submit" class="w-full bg-var-color text-white p-2 rounded-md">
          Войти
        </button>
      </form>
      <NuxtLink to="/auth/reg">
        <p class="rout-to-reg">Зарегистрироваться</p>
      </NuxtLink>
    </div>
  </div>
</template>
