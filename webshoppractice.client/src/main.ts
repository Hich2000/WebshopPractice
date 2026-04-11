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
import ProfileView from './views/ProfileView.vue'
import MyInformation from './components/profileComponents/MyInformation.vue'
import ChangeMyPassword from './components/profileComponents/ChangeMyPassword.vue'
import NoAccessView from './views/NoAccessView.vue'
import PlaceHolderView from './views/PlaceHolderView.vue'

const routes = [
  { path: '/', component: ProductPage },
  { path: '/NoAccess', component: NoAccessView },
  { path: '/Placeholder', component: PlaceHolderView },
  { path: '/Login', component: LoginView },
  {
    path: '/Profile',
    component: ProfileView,
    meta: { requiresAuth: true },
    children: [
      {
        path: 'Me',
        meta: { requiresAuth: true },
        component: MyInformation
      },
      {
        path: 'Password',
        meta: { requiresAuth: true },
        component: ChangeMyPassword
      }
    ]
  },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

//setup guard logic
import { useUser } from '@/composables/user';
const { fetchCurrentUser } = useUser()
router.beforeEach(async (to, _from, next)  => {
  if (to.matched.length < 1) {
    router.push('/NoAccess');
  }

  const currentUser = await fetchCurrentUser();
  if (to.meta.requiresAuth && currentUser == null) {
    next({
      path: '/login',
      query: { redirect: to.fullPath }
    });
  } else {
    next();
  }
});

app.use(router)

// components
import { Button, Menubar, Menu } from 'primevue'
app.component('PButton', Button)
app.component('PMenubar', Menubar)
app.component('PMenu', Menu)

app.mount('#app');
