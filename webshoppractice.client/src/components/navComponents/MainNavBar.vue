<script lang="ts">
import { defineComponent, ref, type Ref } from 'vue';
import type { MenuItem } from 'primevue/menuitem';
import { router } from '@/main';
import { useUser, type User } from '@/composables/user'

const { currentUser, fetchCurrentUser, logout } = useUser()

interface NavBarData {
  navItems: MenuItem[],
  userMenuItems: MenuItem[],
  userMenuRef: Ref
  currentUser: Ref<User | null>
}

export default defineComponent({
  data(): NavBarData {
    return {
      navItems: [
        {
          label: "Home",
          command: () => router.push('/')
        },
      ],
      userMenuItems: [
        {
          label: "Profile",
          command: () => router.push('/Profile')
        },
        {
          label: "Logout",
          command: async () => {
            await logout()
            router.push('/')
          }
        }
      ],
      userMenuRef: ref(),
      currentUser: currentUser
    }
  },
  async created() {
    await fetchCurrentUser()
  },
  methods: {
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
        <PMenu ref="userMenu" id="user_menu" :model="userMenuItems" :popup="true">
          <template #start>
            <div style="text-align: center;">
              Welcome {{ currentUser.name }}
            </div>
          </template>
        </PMenu>
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
