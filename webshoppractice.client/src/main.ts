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

//imports
import { createRouter, createWebHistory } from 'vue-router'
import { UserLevel, useUser } from '@/shared/composables/user';
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
import SellerTable from './features/admin/components/SellerTable.vue'
import SellerView from './features/seller/views/SellerView.vue'
import SellerLinks from './features/seller/components/SellerLinks.vue'
import SellerInformation from './features/seller/components/SellerInformation.vue'
import MyProducts from './features/seller/components/MyProducts.vue'


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
    meta: { requiresAuth: true, requiredLevel: UserLevel.Admin },
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
      },
      {
        path: 'Sellers',
        component: SellerTable
      }
    ]
  },
  {
    path: '/Seller',
    component: SellerView,
    meta: { requiresAuth: true, requiredLevel: UserLevel.Seller },
    children: [
      {
        path: '',
        component: SellerLinks,
      },
      {
        path: 'Information',
        component: SellerInformation,
      },
      {
        path: 'Users',
        component: PlaceHolderView
      },
      {
        path: 'Products',
        component: MyProducts
      },
    ]
  }
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

//setup guard logic
const { fetchCurrentUser } = useUser()
router.beforeEach(async (to, _from, next) => {
  if (to.matched.length < 1) {
    next('/NoAccess');
  }

  const currentUser = await fetchCurrentUser();
  if (to.meta.requiresAuth && currentUser == null) {
    next({
      path: '/Login',
      query: { redirect: to.fullPath }
    });
  }

  if (to.meta.requiredLevel != null) {
    if (!currentUser || currentUser.level != to.meta.requiredLevel) {
      next('/NoAccess');
    }
  }

  next();
});

app.use(router)
app.mount('#app');
