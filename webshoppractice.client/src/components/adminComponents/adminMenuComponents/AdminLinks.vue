<script lang="ts">
import { defineComponent } from 'vue';
import type { MenuItem } from 'primevue/menuitem';
import { router } from '@/main';

export interface ProfileMenu {
  menuItems: MenuItem[]
}

export default defineComponent({
  data(): ProfileMenu {
    return {
      menuItems: [
        {
          label: 'Users',
          icon: 'pi pi-user',
          command: () => router.push('/Admin/Users')
        }
      ]
    }
  }
})
</script>

<template>
  <PMenu :model="menuItems" />
</template>
