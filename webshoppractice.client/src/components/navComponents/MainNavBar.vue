<script lang="ts">
import { defineComponent, type Ref } from 'vue';
import { useUser, type User } from '@/composables/user'
import { useNavItems, type NavItemList } from '@/composables/mainMenuItems';

const { currentUser, fetchCurrentUser } = useUser()
const { navItemList, buildNavList } = useNavItems()

interface NavBarData {
  navItemList: Ref<NavItemList>
  currentUser: Ref<User | null>
}

export default defineComponent({
  data(): NavBarData {
    return {
      navItemList: navItemList,
      currentUser: currentUser
    }
  },
  async created() {
    await fetchCurrentUser()
    await buildNavList()
  },
  watch: {
    currentUser: {
      async handler() {
        await buildNavList();
      },
      deep: false, // optional here
      immediate: false // don't run on initial setup
    }
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

  <PMenubar class="mainNavBar" :model="navItemList.mainMenuItems">
    <template #end>
      <div v-if="currentUser">
        <PButton type="button" icon="pi pi-user" @click="toggle($event)" aria-haspopup="true"
          aria-controls="user_menu" />
        <PMenu ref="userMenu" id="user_menu" :model="navItemList.userMenuItems" :popup="true">
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
