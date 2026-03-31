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
    }
  }
})
</script>

<template>
  <PMenu class="formDiv" :model="menuItems" />
</template>
