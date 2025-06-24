<template>
  <div class="max-w-lg mx-auto mt-20 p-6 border rounded shadow bg-white">
    <h2 class="text-2xl font-bold mb-4">Детали заявки</h2>
    <div v-if="loading">Загрузка...</div>
    <div v-else-if="error" class="text-red-600">
      {{ error === 'Request failed with status code 404' || error === 'Not Found' ? 'Заявка не найдена или уже удалена' : error }}
    </div>
    <div v-else>
      <div class="mb-2"><b>Тип:</b> {{ (request.type && request.type.trim()) ? request.type : '—' }}</div>
      <div class="mb-2"><b>Тема:</b> {{ request.title }}</div>
      <div class="mb-2"><b>Описание:</b> {{ request.description }}</div>
      <div class="mb-2"><b>Статус:</b> {{ statusMap[request.status] || request.status }}</div>
      <div class="mb-2"><b>Создана:</b> {{ request.createdAt?.slice(0, 10) }}</div>
      <button
        @click="router.push('/dashboard')"
        class="mt-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Вернуться к моим заявкам
      </button>
      <button @click="remove" class="mt-4 bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700">Удалить</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api'
import { useRoute, useRouter } from 'vue-router'

const statusMap = {
  New: 'Новая',
  'In Progress': 'В работе',
  Done: 'Выполнено'
}

const route = useRoute()
const router = useRouter()
const request = ref({})
const loading = ref(true)
const error = ref(null)

async function load() {
  loading.value = true
  error.value = null
  try {
    const response = await api.get(`/requests/${route.params.id}`)
    request.value = response.data
  } catch (e) {
    error.value = e.response?.data?.error || e.message || 'Ошибка'
  } finally {
    loading.value = false
  }
}

async function remove() {
  await api.delete(`/requests/${route.params.id}`)
  router.push('/dashboard')
}

onMounted(load)
</script> 