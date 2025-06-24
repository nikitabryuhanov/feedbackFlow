import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5297/api',
})

api.interceptors.request.use(config => {
  const token = localStorage.getItem('jwt_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  response => {
    // Сохраняем роль пользователя при логине
    if (response.config.url.endsWith('/auth/login') && response.data.token && response.data.role) {
      localStorage.setItem('role', response.data.role)
    }
    return response
  },
  error => Promise.reject(error)
)

export default api 