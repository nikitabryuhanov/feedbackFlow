<template>
  <div class="max-w-md mx-auto mt-20 p-6 border rounded shadow">
    <h2 class="text-2xl font-bold mb-4">Регистрация</h2>
    <form @submit.prevent="submitRegister" class="space-y-4">
      <input v-model="username" type="text" placeholder="Имя пользователя" required class="w-full p-2 border rounded" />
      <input v-model="password" type="password" placeholder="Пароль" required class="w-full p-2 border rounded" />
      <button type="submit" class="w-full bg-green-600 text-white p-2 rounded hover:bg-green-700">Зарегистрироваться</button>
    </form>
    <p v-if="error" class="text-red-600 mt-2">{{ error }}</p>
    <p v-if="success" class="text-green-600 mt-2">{{ success }}</p>
    <router-link to="/login" class="block mt-4 text-blue-600 hover:underline">Уже есть аккаунт? Войти</router-link>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import api from '../services/api'
import { useRouter, RouterLink } from 'vue-router'

const username = ref('')
const password = ref('')
const error = ref(null)
const success = ref(null)
const router = useRouter()

async function submitRegister() {
  error.value = null
  success.value = null
  try {
    await api.post('/auth/register', {
      username: username.value,
      password: password.value,
    })
    success.value = 'Регистрация успешна! Теперь вы можете войти.'
    setTimeout(() => router.push('/login'), 1500)
  } catch (e) {
    error.value = e.response?.data?.error || 'Ошибка регистрации'
  }
}
</script> 