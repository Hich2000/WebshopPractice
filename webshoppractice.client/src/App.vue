<script lang="ts">
import { defineComponent } from 'vue';
import { RouterView } from 'vue-router';
import MainNavBar from './components/navComponents/MainNavBar.vue';

export default defineComponent({
  components: {
    RouterView,
    MainNavBar
  },
})

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
  padding-top: 3rem;
}
</style>
