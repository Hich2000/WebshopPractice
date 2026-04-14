<script setup lang="ts">
import type { MenuItem } from 'primevue/menuitem'
import { router } from '@/main'
import { Menu } from 'primevue';

const menuItems: MenuItem[] = [
  {
    label: 'My information',
    icon: 'pi pi-user',
    command: () => router.push('/Profile/Me')
  },
  {
    label: 'Change password',
    icon: 'pi pi-hashtag',
    command: () => router.push('/Profile/Password')
  }
]
</script>

<template>
  <Menu class="form-div" :model="menuItems" />
</template>
