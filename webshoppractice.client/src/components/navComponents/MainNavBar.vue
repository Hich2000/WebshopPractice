<script lang="ts">
import { defineComponent, ref, type Ref } from 'vue';
import type { MenuItem } from 'primevue/menuitem';
import { router } from '@/main';

interface UserInfo {
  name: string,
  username: string,
}

interface NavBarData {
  navItems: MenuItem[],
  userMenuItems: MenuItem[],
  userMenuRef: Ref
  currentUser: UserInfo | null
}

export default defineComponent({
  data(): NavBarData {
    return {
      navItems: [
        {
          label: "Home",
          command: () => router.push('/')
        },
        {
          label: "Login",
          command: () => router.push('/Login')
        }
      ],
      userMenuItems: [
        {
          label: "Profile",
          command: () => router.push('/Login')
        },
        {
          label: "Logout",
          command: () => router.push('/')
        }
      ],
      userMenuRef: ref(),
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
      if (response.status === 401 || !response.ok) {
        this.currentUser = null
        return
      }
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
    <template #end>
      <div v-if="currentUser">
        <PButton type="button" icon="pi pi-user" @click="toggle($event)" aria-haspopup="true"
          aria-controls="user_menu" />
        <PMenu ref="userMenu" id="user_menu" :model="userMenuItems" :popup="true"></PMenu>
      </div>
      <div v-else>
        <PButton @click="$router.push('/Login')">Login</PButton>
      </div>
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
</style>
