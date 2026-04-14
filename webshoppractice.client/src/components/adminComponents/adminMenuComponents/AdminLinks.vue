<script setup lang="ts">
import type { MenuItem } from 'primevue/menuitem'
import { router } from '@/main'
import { Menu } from 'primevue';

const menuItems: MenuItem[] = [
  {
    label: 'Register Admin',
    icon: 'pi pi-user',
    command: () => router.push('/Admin/RegisterAdmin')
  },
  {
    label: 'Users',
    icon: 'pi pi-user',
    command: () => router.push('/Admin/Users')
  }
]
</script>

<template>
  <Menu :model="menuItems" />
</template>
