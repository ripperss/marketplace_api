<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import '@/assets/css/registr.css'
import NavMenu from '@/components/NavMenu/NavMenu.vue'

const router = useRouter()

const form = ref({
  /*
  email: 'moishemg@vk.com',
  hashPassword: '2281337dD@'
  */
 /*
    email: 'cool-ryba@omut.com',
    hashPassword: 'Pa$$vv0Rd'
    */
})



const login = async () => {
  try {
    const response = await fetch('http://localhost:8080/authuser/log', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value),
      credentials: 'include' // ВАЖНО! Чтобы куки работали
    });

    console.log('Ответ сервера (пользователь):', response);

    if (!response.ok) {
      throw new Error('Ошибка входа: Неверные данные');
    }

    console.log('✅ Вход выполнен! (как пользователь)');
    router.push('/profile');
    return; // Если успешно, дальше не идем
  } catch (error) {
    console.warn('⚠️ Ошибка входа как пользователь, пробуем как продавец...');
  }

  // Пробуем войти как продавец, если первый запрос не удался
  try {
    const responseSeller = await fetch('http://localhost:8080/authseller/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value),
      credentials: 'include'
    });

    console.log('Ответ сервера (продавец):', responseSeller);

    if (!responseSeller.ok) {
      throw new Error('Ошибка входа: Неверные данные');
    }

    console.log('✅ Вход выполнен! (как продавец)');
    router.push('/profile');
  } catch (error) {
    console.error('❌ Ошибка входа:', error);
    errorMessage.value = 'Ошибка авторизации. Проверьте email и пароль';
  }
};




</script>



<template>
  <NavMenu />
  <div class="flex min-h-screen justify-center p-4">
    <div class="w-full max-w-md p-6 rounded-lg mt-0">
      <h2 class="text-xl font-semibold text-center mb-4 header">Вход</h2>

      <form @submit.prevent="login" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 bober-font">Email</label>
          <input
            v-model="form.email"
            type="email"
            placeholder="Введите email"
            class="w-full p-2 border round8px bober-font"
          />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 bober-font">Пароль</label>
          <input
            v-model="form.hashPassword"
            type="password"
            placeholder="Введите пароль"
            class="w-full p-2 border round8px bober-font"
          />
        </div>

        <p v-if="errorMessage" class="text-red-500 text-sm text-center bober-font">{{ errorMessage }}</p>

        <button type="submit" class="w-full button-style bober-font p-2 round8px">
          Войти
        </button>
      </form>
      <NuxtLink to="/auth/reg">
        <p class="rout-to-reg text-center bober-font">Зарегистрироваться</p>
      </NuxtLink>
    </div>
  </div>
</template>
