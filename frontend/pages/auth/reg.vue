<script setup>
import { ref } from 'vue'
import '@/assets/css/registr.css'

const form = ref({
  name: '',
  email: '',
  password: ''
})

const register = async () => {
  try {
    const response = await fetch('http://localhost:8080/authuser/reg', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    })
    const data = await response.json()
    console.log('Регистрация успешна:', data)
  } catch (error) {
    console.error('Ошибка регистрации:', error)
  }
}
</script>

<template>

 <NavMenu />


  <div class="flex min-h-screen items-center justify-center p-4">
    <div class="w-full max-w-md bg-white p-6 rounded-lg ">
      <h2 class="text-xl font-semibold text-center mb-4">Регистрация</h2>

      <form @submit.prevent="register" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Имя</label>
          <input
            v-model="form.name"
            type="text"
            placeholder="Введите имя"
            class="w-full p-2 border rounded-md"
          />
        </div>

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
          Зарегистрироваться
        </button>
      </form>
    </div>
  </div>
</template>

