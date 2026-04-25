<script setup lang="ts">
import { router } from '@/main';
import { Button } from 'primevue';

</script>

<template>
  <div class="form-div">
    <div v-if="router.currentRoute.value.path != '/Seller'" style="width: 100%; text-align: start; padding: 1rem;"
      v-on:click="router.back()">
      <Button>back</Button>
    </div>
    <RouterView />
  </div>
</template>
