<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  data() {
    return {
      items: [
        {
          label: "Home",
          route: "/"
        },
        {
          label: "Login",
          route: "/Login"
        }
      ]
    }
  }
})
</script>

<template>
  <PMenubar class="mainNavBar" :model="items">
    <template #item="{ item }">
      <RouterLink :to="item.route">
        {{ item.label }}
      </RouterLink>
    </template>
    <template #end>
test
    </template>
  </PMenubar>
</template>

<style lang="css" scoped>
.mainNavBar {
  position: sticky;
  top: 1rem;
  z-index: 9999;
  width: 100%;
}
</style>
