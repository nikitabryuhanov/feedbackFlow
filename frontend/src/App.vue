<template>
  <Header :showLogout="showLogout" :user="user" />
  <main class="min-h-screen bg-gray-50 pt-8">
    <router-view />
  </main>
  <Footer />
</template>
<script setup>
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from './services/api'
const route = useRoute()
const router = useRouter()
const showLogout = computed(() => !['/login', '/register'].includes(route.path))
const user = ref(null)
async function loadUser() {
  try {
    const response = await api.get('/users/me')
    user.value = response.data
  } catch {
    // не редиректим, чтобы не мешать страницам логина/регистрации
  }
}
onMounted(() => {
  loadUser()
})
</script>