<template>
  <div class="max-w-md mx-auto mt-20 p-6 border rounded shadow">
    <h2 class="text-2xl font-bold mb-4">Вход</h2>
    <form @submit.prevent="submitLogin" class="space-y-4">
      <input v-model="username" type="text" placeholder="Имя пользователя" required
        class="w-full p-2 border rounded" />
      <input v-model="password" type="password" placeholder="Пароль" required
        class="w-full p-2 border rounded" />
      <button type="submit" class="w-full bg-blue-600 text-white p-2 rounded hover:bg-blue-700">
        Войти
      </button>
    </form>
    <p v-if="error" class="text-red-600 mt-2">{{ error }}</p>
    <router-link to="/register" class="block mt-4 text-blue-600 hover:underline">Нет аккаунта? Зарегистрироваться</router-link>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import api from '../services/api'
import { useRouter, RouterLink } from 'vue-router'

const username = ref('')
const password = ref('')
const error = ref(null)
const router = useRouter()

async function submitLogin() {
  error.value = null
  try {
    const response = await api.post('/auth/login', {
      username: username.value,
      password: password.value,
    })
    localStorage.setItem('jwt_token', response.data.token)
    router.push('/dashboard')
  } catch {
    error.value = 'Неверное имя пользователя или пароль'
  }
}
</script> 