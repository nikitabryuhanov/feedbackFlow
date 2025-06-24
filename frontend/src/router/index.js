import { createRouter, createWebHistory } from 'vue-router'

const Login = () => import('../views/Login.vue')
const Register = () => import('../views/Register.vue')
const Dashboard = () => import('../views/Dashboard.vue')
const AdminPanel = () => import('../views/AdminPanel.vue')
const RequestDetails = () => import('../views/RequestDetails.vue')
const Profile = () => import('../views/Profile.vue')

const routes = [
  { path: '/login', component: Login, meta: { title: 'Вход' } },
  { path: '/register', component: Register, meta: { title: 'Регистрация' } },
  { path: '/dashboard', component: Dashboard, meta: { requiresAuth: true, title: 'Мои заявки' } },
  { path: '/admin', component: AdminPanel, meta: { requiresAuth: true, admin: true, title: 'Админ-панель' } },
  { path: '/requests/:id', component: RequestDetails, meta: { requiresAuth: true, title: 'Детали заявки' } },
  { path: '/profile', component: Profile, meta: { requiresAuth: true, title: 'Личный кабинет' } },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('jwt_token')
  const role = localStorage.getItem('role')
  if (to.path === '/') {
    if (token && role === 'Admin') return next('/admin')
    if (token && role === 'User') return next('/dashboard')
    return next('/login')
  }
  if (to.meta.requiresAuth && !token) {
    next('/login')
  } else if ((to.path === '/login' || to.path === '/register') && token) {
    if (role === 'Admin') next('/admin')
    else next('/dashboard')
  } else if (to.meta.admin && role !== 'Admin') {
    next('/dashboard')
  } else {
    next()
  }
})

export default router
