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

import { ConfirmationService } from 'primevue'
app.use(ConfirmationService);

//navigation
import { createRouter, createWebHistory } from 'vue-router'
import LoginView from './features/user/views/LoginView.vue'
import HomeView from './shared/views/HomeView.vue'
import ProfileView from './features/user/views/ProfileView.vue'
import MyInformation from './features/user/components/MyInformation.vue'
import ChangeMyPassword from './features/user/components/ChangeMyPassword.vue'
import AdminView from './features/admin/views/AdminView.vue'
import AdminLinks from './features/admin/components/AdminLinks.vue'
import NoAccessView from './shared/views/NoAccessView.vue'
import PlaceHolderView from './shared/views/PlaceHolderView.vue'
import UsersTable from './features/admin/components/UsersTable.vue'
import AdminUserForm from './features/admin/components/AdminUserForm.vue'
import DeleteMyAccount from './features/user/components/DeleteMyAccount.vue'
import AccountDeleted from './features/user/components/AccountDeleted.vue'

const routes = [
  { path: '/', component: HomeView },
  { path: '/NoAccess', component: NoAccessView },
  { path: '/Placeholder', component: PlaceHolderView },
  { path: '/Login', component: LoginView },
  { path: '/AccountDeleted', component: AccountDeleted },
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
      },
      {
        path: 'DeleteAccount',
        meta: { requiresAuth: true },
        component: DeleteMyAccount
      },
    ]
  },
  {
    path: '/Admin',
    component: AdminView,
    meta: { requiresAuth: true, },
    children: [
      {
        path: '',
        component: AdminLinks,
      },
      {
        path: 'Users',
        component: UsersTable,
      },
      {
        path: 'RegisterAdmin',
        component: AdminUserForm
      }
    ]
  }
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

//setup guard logic
import { useUser } from '@/shared/composables/user';
const { fetchCurrentUser } = useUser()
router.beforeEach(async (to, _from, next) => {
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
app.mount('#app');
