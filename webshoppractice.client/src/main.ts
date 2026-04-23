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
import ProductPage from './components/productComponents/ProductPage.vue'
import ProfileView from './features/user/views/ProfileView.vue'
import MyInformation from './features/user/components/MyInformation.vue'
import ChangeMyPassword from './features/user/components/ChangeMyPassword.vue'
import AdminView from './views/AdminView.vue'
import AdminLinks from './components/adminComponents/adminMenuComponents/AdminLinks.vue'
import NoAccessView from './views/NoAccessView.vue'
import PlaceHolderView from './views/PlaceHolderView.vue'
import UsersTable from './components/adminComponents/userManagementComponents/UsersTable.vue'
import AdminUserForm from './components/adminComponents/userManagementComponents/AdminUserForm.vue'
import DeleteMyAccount from './features/user/components/DeleteMyAccount.vue'
import AccountDeleted from './features/user/components/AccountDeleted.vue'

const routes = [
  { path: '/', component: ProductPage },
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
import { useUser } from '@/composables/user';
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
