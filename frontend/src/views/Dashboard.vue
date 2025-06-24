<template>
  <div class="max-w-3xl mx-auto mt-20 p-6">
    <!-- <h1 class="text-3xl font-bold mb-4">Панель управления</h1> -->
    <!-- <p>Добро пожаловать, {{ user?.username }}</p> -->
    <FeedbackForm @submitted="loadRequests" />
    <h2 class="text-xl font-bold mt-8 mb-2">Мои заявки</h2>
    <ul>
      <li v-for="req in requests" :key="req.id" class="border-b py-2 flex justify-between items-center">
        <router-link :to="`/requests/${req.id}`" class="flex-1 flex items-center space-x-2 hover:underline">
          <span class="font-semibold">[{{ req.type || '—' }}]</span>
          <span>{{ req.title }} — <span class="italic">{{ statusMap[req.status] || req.status }}</span></span>
          <span class="text-xs text-gray-500 ml-2">{{ req.createdAt?.slice(0, 10) }}</span>
        </router-link>
      </li>
    </ul>
    <button @click="logout" class="mt-4 bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700">Выйти</button>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api'
import { useRouter, RouterLink } from 'vue-router'
import FeedbackForm from '../components/FeedbackForm.vue'

const statusMap = {
  New: 'Новая',
  'In Progress': 'В работе',
  Done: 'Выполнено'
}

const user = ref(null)
const requests = ref([])
const router = useRouter()

// Проверка роли: если админ, редирект на /admin
if (localStorage.getItem('role') === 'Admin') {
  router.replace('/admin')
}

async function loadUser() {
  try {
    const response = await api.get('/users/me')
    user.value = response.data
  } catch {
    router.push('/login')
  }
}

async function loadRequests() {
  try {
    const response = await api.get('/requests')
    requests.value = response.data
  } catch {}
}

function logout() {
  localStorage.removeItem('jwt_token')
  localStorage.removeItem('role')
  router.push('/login')
}

onMounted(() => {
  loadUser()
  loadRequests()
})
</script> 