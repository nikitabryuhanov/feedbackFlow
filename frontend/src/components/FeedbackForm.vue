<template>
  <form @submit.prevent="submit" class="mb-6 p-6 bg-white rounded shadow space-y-4">
    <h2 class="text-xl font-bold mb-4 text-center">Создать заявку</h2>
    <div>
      <label class="block mb-1 font-semibold">Тип заявки</label>
      <select v-model="type" required class="w-full p-2 border rounded">
        <option disabled value="">Выберите тип</option>
        <option>Жалоба</option>
        <option>Вопрос</option>
        <option>Предложение</option>
      </select>
    </div>
    <div>
      <label class="block mb-1 font-semibold">Тема</label>
      <input v-model="title" type="text" placeholder="Тема" required class="w-full p-2 border rounded" />
    </div>
    <div>
      <label class="block mb-1 font-semibold">Описание</label>
      <textarea v-model="description" placeholder="Описание" required class="w-full p-2 border rounded"></textarea>
    </div>
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
const type = ref('Вопрос')
const error = ref(null)
const success = ref(null)

async function submit() {
  error.value = null
  success.value = null
  if (!type.value) type.value = 'Вопрос'
  try {
    await api.post('/requests', { title: title.value, description: description.value, type: type.value })
    success.value = 'Заявка отправлена!'
    title.value = ''
    description.value = ''
    type.value = 'Вопрос'
    emit('submitted')
  } catch (e) {
    error.value = e.response?.data?.error || 'Ошибка отправки'
  }
}
</script> 