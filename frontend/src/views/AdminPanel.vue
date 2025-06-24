<template>
  <div class="max-w-4xl mx-auto mt-20 p-6">
    <h1 class="text-3xl font-bold mb-4">Админ-панель</h1>
    <h2 class="text-xl font-bold mb-2">Все заявки</h2>
    <select v-model="filter" class="mb-4 p-2 border rounded">
      <option value="">Все статусы</option>
      <option v-for="s in statuses" :key="s" :value="s">{{ statusMap[s] || s }}</option>
    </select>
    <ul>
      <li v-for="req in filteredRequests" :key="req.id" class="border-b py-2 flex justify-between items-center">
        <span>{{ req.title }} — <span class="italic">{{ statusMap[req.status] || req.status }}</span></span>
        <div class="flex items-center space-x-2">
          <select v-model="editedStatuses[req.id]" class="p-1 border rounded">
            <option v-for="s in statuses" :key="s" :value="s">{{ statusMap[s] || s }}</option>
          </select>
          <button
            @click="confirmStatus(req)"
            :disabled="editedStatuses[req.id] === req.status"
            class="bg-blue-600 text-white px-2 py-1 rounded hover:bg-blue-700 disabled:opacity-50"
          >
            Подтвердить
          </button>
          <button @click="remove(req.id)" class="bg-red-600 text-white px-2 py-1 rounded hover:bg-red-700">Удалить</button>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import api from '../services/api'
import { useRouter } from 'vue-router'

const statusMap = {
  New: 'Новая',
  'In Progress': 'В работе',
  Done: 'Выполнено'
}

const statuses = ['New', 'In Progress', 'Done']

const requests = ref([])
const filter = ref('')
const router = useRouter()
const editedStatuses = ref({})

const filteredRequests = computed(() =>
  filter.value ? requests.value.filter(r => r.status === filter.value) : requests.value
)

async function loadRequests() {
  try {
    const response = await api.get('/admin/requests')
    requests.value = response.data
    // Сохраняем текущие статусы для локального редактирования
    editedStatuses.value = Object.fromEntries(response.data.map(r => [r.id, r.status]))
  } catch {
    router.push('/login')
  }
}

async function confirmStatus(req) {
  await api.patch(`/admin/requests/${req.id}`, { Status: editedStatuses.value[req.id] })
  await loadRequests()
}

async function remove(id) {
  await api.delete(`/admin/requests/${id}`)
  await loadRequests()
}

onMounted(loadRequests)
</script> 