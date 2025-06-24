<template>
  <header class="w-full bg-blue-700 text-white py-4 px-4 flex flex-col items-center shadow gap-2">
    <span class="font-thin text-xl">FeedbackFlow</span>
    <span v-if="user" class="mb-2">Добро пожаловать, {{ user.username }}!</span>
    <div class="flex flex-col sm:flex-row items-center gap-2 sm:gap-4">
      <button v-if="user && user.role !== 'Admin'" @click="goProfile" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Личный кабинет</button>
      <button v-if="user && user.role !== 'Admin'" @click="goDashboard" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Мои заявки</button>
      <button v-if="showLogout && user && user.role === 'Admin'" @click="logout" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Выйти</button>
    </div>
  </header>
</template>
<script setup>
import { useRouter, useRoute } from 'vue-router'
import { computed } from 'vue'
const router = useRouter()
const route = useRoute()
const props = defineProps({ showLogout: Boolean, user: Object })
function logout() {
  localStorage.removeItem('jwt_token')
  localStorage.removeItem('role')
  router.push('/login')
}
function goProfile() {
  router.push('/profile')
}
function goDashboard() {
  router.push('/dashboard')
}
</script> 