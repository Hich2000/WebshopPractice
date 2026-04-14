<script setup lang="ts">
import { ref, watch, onMounted, nextTick } from 'vue'
import { useRouter } from 'vue-router'
import { useUser } from '@/composables/user'
import { useNavItems } from '@/composables/mainMenuItems'
import { Button, Menu, Menubar } from 'primevue'

const router = useRouter()

const { currentUser, fetchCurrentUser } = useUser()
const { navItemList, buildNavList } = useNavItems()

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const userMenu = ref<any>(null)

onMounted(async () => {
  await fetchCurrentUser()
  await buildNavList()
})

watch(currentUser, async () => {
  await buildNavList()
})

function toggle(event: Event) {
  const menu = userMenu.value
  menu.toggle(event)

  nextTick(() => {
    const el = menu.container as HTMLElement
    if (el) {
      const rect = (event.currentTarget as HTMLElement).getBoundingClientRect()
      el.style.left = `${rect.right - el.offsetWidth}px`
      el.style.top = `${rect.bottom}px`
    }
  })
}
</script>

<template>
  <Menubar class="mainNavBar" :model="navItemList.mainMenuItems">
    <template #end>
      <div v-if="currentUser">
        <Button type="button" icon="pi pi-user" @click="toggle($event)" aria-haspopup="true"
          aria-controls="user_menu" />
        <Menu ref="userMenu" id="user_menu" :model="navItemList.userMenuItems" :popup="true">
          <template #start>
            <div style="text-align: center;">
              Welcome {{ currentUser.name }}
            </div>
          </template>
        </Menu>
      </div>

      <div v-else>
        <Button @click="router.push('/Login')">Login</Button>
      </div>
    </template>
  </Menubar>
</template>

<style scoped>
.mainNavBar {
  position: sticky;
  top: 1rem;
  z-index: 10;
  width: 100%;
}
</style>
