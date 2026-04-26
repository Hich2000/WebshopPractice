<script setup lang="ts">
import { RouterView } from 'vue-router';
import MainNavBar from './features/navigation/components/MainNavBar.vue';
</script>

<template>
  <main>
    <MainNavBar />
    <div class="page-content">
      <RouterView />
    </div>
  </main>
</template>

<style scoped>
main {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.page-content {
  flex: 1;
  padding-top: 2rem;
}
</style>
