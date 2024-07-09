import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: () => import('../views/login.vue')
    },
    {
      path: '/vote',
      component: () => import('../views/vote.vue')
    }
  ]
})

export default router