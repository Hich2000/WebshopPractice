<script setup lang="ts">
import type { MenuItem } from 'primevue/menuitem'
import { router } from '@/main'
import { Menu } from 'primevue';

const menuItems: MenuItem[] = [
  {
    label: 'Seller Information',
    icon: 'pi pi-user',
    command: () => router.push('/Seller/Information')
  },
  {
    label: 'Users',
    icon: 'pi pi-user',
    command: () => router.push('/Seller/Users')
  },
  {
    label: 'My Products',
    icon: 'pi pi-user',
    command: () => router.push('/Seller/Products')
  },
  {
    label: 'New Product',
    icon: 'pi pi-user',
    command: () => router.push('/Seller/NewProduct')
  }
]
</script>

<template>
  <Menu :model="menuItems" />
</template>
