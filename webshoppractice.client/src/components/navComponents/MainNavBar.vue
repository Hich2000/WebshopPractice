<script lang="ts">
import { defineComponent, ref, type Ref } from 'vue';

interface NavItems {
  label: string,
  route: (() => void) | string,
}

interface UserInfo {
  name: string,
  username: string,
}

interface NavBarData {
  navItems: NavItems[],
  userMenuItems: NavItems[],
  userMenuRef: Ref
  currentUser: UserInfo | null
}

const navItems = [
  {
    label: "Home",
    route: '/',
  },
  {
    label: "Login",
    route: '/Login',
  }
];
const userMenuItems = [
  {
    label: "Profile",
    route: '/Login',
  },
  {
    label: "Logout",
    route: '/',
  }
]

const userMenu = ref()

export default defineComponent({
  data(): NavBarData {
    return {
      navItems: navItems,
      userMenuItems: userMenuItems,
      userMenuRef: userMenu,
      currentUser: null
    }
  },
  async created() {
    await this.me()
  },
  watch: {
    '$route': 'me'
  },
  methods: {
    async me() {
      const response = await fetch("/login/me")
      if (response.status === 401 || !response.ok) return
      const result = await response.json()
      this.currentUser = {
        name: result.name,
        username: result.username
      }
    },
    toggle(event: Event) {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const menu = this.$refs.userMenu as any
      menu.toggle(event)

      this.$nextTick(() => {
        const el = menu.container as HTMLElement
        if (el) {
          const rect = (event.currentTarget as HTMLElement).getBoundingClientRect()
          el.style.left = `${rect.right - el.offsetWidth}px`
          el.style.top = `${rect.bottom}px`
        }
      })
    }
  }
})
</script>

<template>
  <PMenubar class="mainNavBar" :model="navItems">
    <template #item="{ item }">
      <RouterLink :to="item.route">
        {{ item.label }}
      </RouterLink>
    </template>
    <template #end>
      <PButton type="button" icon="pi pi-user" @click="toggle($event)" aria-haspopup="true" aria-controls="user_menu" />
      <PMenu ref="userMenu" id="user_menu" :model="userMenuItems" :popup="true" style="z-index: 11;" appendTo="body">
        <template #item="{ item }">
          <RouterLink :to="item.route">
            {{ item.label }}
          </RouterLink>
        </template>
      </PMenu>
    </template>
  </PMenubar>
</template>

<style lang="css" scoped>
.mainNavBar {
  position: sticky;
  top: 1rem;
  z-index: 10;
  width: 100%;
}

:deep(.p-menu) {
  transform: translateX(-100%);
}
</style>
