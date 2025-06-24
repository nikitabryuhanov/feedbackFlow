<template>
  <form @submit.prevent="submit" class="mb-6 space-y-4">
    <input v-model="title" type="text" placeholder="Тема" required class="w-full p-2 border rounded" />
    <textarea v-model="description" placeholder="Описание" required class="w-full p-2 border rounded"></textarea>
    <button type="submit" class="w-full bg-green-600 text-white p-2 rounded hover:bg-green-700">Отправить заявку</button>
    <p v-if="error" class="text-red-600 mt-2">{{ error }}</p>
    <p v-if="success" class="text-green-600 mt-2">{{ success }}</p>
  </form>
</template>

<script setup>
import { ref } from 'vue'
import api from '../services/api'

const emit = defineEmits(['submitted'])
const title = ref('')
const description = ref('')
const error = ref(null)
const success = ref(null)

async function submit() {
  error.value = null
  success.value = null
  try {
    await api.post('/requests', { title: title.value, description: description.value })
    success.value = 'Заявка отправлена!'
    title.value = ''
    description.value = ''
    emit('submitted')
  } catch (e) {
    error.value = e.response?.data?.error || 'Ошибка отправки'
  }
}
</script> 