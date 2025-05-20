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


  <div class="flex min-h-screen justify-center p-4">
    <div class="w-full max-w-md p-6 rounded-lg mt-0">
      <h2 class="text-xl font-semibold text-center mb-4 header">Регистрация</h2>

      <form @submit.prevent="register" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 bober-font">Имя</label>
          <input v-model="form.name" type="text" placeholder="Введите имя" class="w-full p-2 border round8px bober-font" />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 bober-font">Email</label>
          <input v-model="form.email" type="email" placeholder="Введите email" class="w-full p-2 border round8px bober-font" />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 bober-font">Пароль</label>
          <input v-model="form.password" type="password" placeholder="Введите пароль"
            class="w-full p-2 border round8px bober-font" />
        </div>
<!--
        <Select v-model="form.role"  class="user-role button-style round8px border" >
          <SelectTrigger>
            <SelectValue class="bober-font" placeholder="Зарегистрироваться как :" />
          </SelectTrigger>
          <SelectContent>
            <SelectGroup>
              <SelectLabel>Зарегистрироваться как :</SelectLabel>
              <SelectItem  class="button-style bober-font" value="1">
                Пользователь
              </SelectItem>

              <SelectItem class="button-style bober-font" value="2">
                Продавец
              </SelectItem>
            </SelectGroup>
          </SelectContent>
        </Select>
-->
        <div class="flex flex-row w-full">
          <button type="button" class="button-style bober-font p-2 round8px flexl" value="1">Пользователь</button>
            <div style="width: 60px;"></div>
          <button type="button" class="button-style bober-font p-2 round8px flexr" value="2">Продавец</button>
        </div>


        <button type="submit" class="w-full button-style bober-font p-2 round8px">
          Зарегистрироваться
        </button>
      </form>
      <NuxtLink to="/auth/login">
        <p class="rout-to-reg text-center bober-font">Уже есть аккаунт?</p>
      </NuxtLink>
    </div>
  </div>
</template>
