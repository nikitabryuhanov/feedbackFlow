<template>
  <header class="w-full bg-blue-700 text-white py-4 px-4 flex flex-col sm:flex-row items-center sm:justify-between shadow gap-2">
    <span class="font-thin text-xl w-full text-center sm:w-auto">FeedbackFlow</span>
    <div class="flex flex-col sm:flex-row items-center gap-2 sm:gap-4 sm:ml-auto">
      <span v-if="user" class="mb-2 sm:mb-0 sm:mr-4">Добро пожаловать, {{ user.username }}!</span>
      <template v-if="user && user.role !== 'Admin'">
        <button @click="goProfile" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Личный кабинет</button>
        <button @click="goDashboard" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Мои заявки</button>
        <button @click="logout" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Выйти</button>
      </template>
      <template v-else-if="showLogout && user && user.role === 'Admin'">
        <button @click="logout" class="bg-white text-blue-700 px-4 py-2 rounded hover:bg-gray-100">Выйти</button>
      </template>
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