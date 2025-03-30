<script setup>
import { ref } from 'vue'
import '@/assets/css/registr.css'

import { useRouter } from 'vue-router'

import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'





const form = ref({
  name: '',
  email: '',
  password: '',
  role: ''
})


const router = useRouter(); // Получаем объект роутера



const register = async () => {
  try {
    const payload = {
      name: form.value.name,
      email: form.value.email,
      hashPassword: form.value.password,
      role: Number(form.value.role)
    };

    const response = await fetch('http://localhost:8080/authuser/reg', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    });

    const text = await response.text();
    try {
      const data = JSON.parse(text);
      console.log('Регистрация успешна:', data);
    } catch (error) {
      console.log('Ответ сервера:', text);
    }

    // ✅ Если регистрация успешна, делаем редирект
    router.push('/auth/login');

  } catch (error) {
    console.error('Ошибка регистрации:', error);
  }
};

</script>

<template>

  <NavMenu />


  <div class="flex min-h-screen items-center justify-center p-4">
    <div class="w-full max-w-md bg-white p-6 rounded-lg ">
      <h2 class="text-xl font-semibold text-center mb-4">Регистрация</h2>

      <form @submit.prevent="register" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Имя</label>
          <input v-model="form.name" type="text" placeholder="Введите имя" class="w-full p-2 border rounded-md" />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Email</label>
          <input v-model="form.email" type="email" placeholder="Введите email" class="w-full p-2 border rounded-md" />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Пароль</label>
          <input v-model="form.password" type="password" placeholder="Введите пароль"
            class="w-full p-2 border rounded-md" />
        </div>

        <Select v-model="form.role"  class="user-role" >
          <SelectTrigger>
            <SelectValue placeholder="Зарегистрироваться как :" />
          </SelectTrigger>
          <SelectContent>
            <SelectGroup>
              <SelectLabel>Зарегистрироваться как :</SelectLabel>
              <SelectItem  value="1">
                Пользователь
              </SelectItem>

              <SelectItem value="2">
                Продавец
              </SelectItem>
            </SelectGroup>
          </SelectContent>
        </Select>


        <button type="submit" class="w-full bg-var-color text-white p-2 rounded-md">
          Зарегистрироваться
        </button>
      </form>
      <NuxtLink to="/auth/login">
        <p class="rout-to-reg">Уже есть аккаунт?</p>
      </NuxtLink>
    </div>
  </div>
</template>
