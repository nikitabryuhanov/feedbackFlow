<template>
  <div class="max-w-md mx-auto mt-20 p-6 bg-white rounded shadow text-lg">
    <h2 class="text-2xl font-bold mb-4 text-center">Личный кабинет</h2>
    <form @submit.prevent="saveProfile" class="space-y-4">
      <div>
        <label class="block mb-1 font-semibold text-left">Email</label>
        <input v-model="email" type="email" class="w-full p-2 border rounded" required />
      </div>
      <div>
        <label class="block mb-1 font-semibold text-left">Ник</label>
        <input v-model="username" type="text" class="w-full p-2 border rounded" required />
      </div>
      <button type="submit" class="w-full bg-blue-600 text-white p-2 rounded hover:bg-blue-700">Сохранить</button>
      <div v-if="success" class="text-green-600">{{ success }}</div>
      <div v-if="error" class="text-red-600">{{ error }}</div>
    </form>
    <button @click="logout" class="mt-6 w-full bg-red-600 text-white p-2 rounded hover:bg-red-700">Выйти</button>
    <button @click="router.push('/dashboard')" class="mt-2 w-full bg-gray-200 text-blue-700 p-2 rounded hover:bg-gray-300">Вернуться к моим заявкам</button>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api'
import { useRouter } from 'vue-router'
const router = useRouter()
const email = ref('')
const username = ref('')
const success = ref('')
const error = ref('')
async function loadProfile() {
  try {
    const response = await api.get('/users/me')
    email.value = response.data.email || ''
    username.value = response.data.username || ''
  } catch {
    router.push('/login')
  }
}
onMounted(loadProfile)
async function saveProfile() {
  try {
    await api.patch('/users/me', { email: email.value, username: username.value })
    success.value = 'Профиль обновлён!'
    error.value = ''
  } catch (e) {
    error.value = e.response?.data?.error || 'Ошибка сохранения'
    success.value = ''
  }
}
function logout() {
  localStorage.removeItem('jwt_token')
  localStorage.removeItem('role')
  router.push('/login')
}
</script> 