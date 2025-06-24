import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './style.css'

createApp(App).use(router).mount('#app')

// Динамическая смена title
router.afterEach((to) => {
  document.title = to.meta.title || 'FeedbackFlow';
})