import './assets/main.css'
import './assets/custom.css'
import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'
import 'primeicons/primeicons.css'

import { createApp } from 'vue'
import App from './App.vue'

const app = createApp(App)
  .use(PrimeVue, {
    theme: {
      preset: Aura,
    }
  });

//navigation
import { createRouter, createWebHistory } from 'vue-router'
import LoginView from './views/LoginView.vue'
import ProductPage from './components/productComponents/ProductPage.vue'

const routes = [
  { path: '/', component: ProductPage },
  { path: '/Login', component: LoginView },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

app.use(router)

// components
import { Button } from 'primevue'
app.component('PButton', Button)
import { Menubar } from 'primevue'
app.component('PMenubar', Menubar)
import { Menu } from 'primevue'
app.component('PMenu', Menu)

app.mount('#app');
