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
    <RouterView style="padding-top: 3rem;" />
  </main>
</template>

<style scoped>
  header {
    line-height: 1.5;
  }

  .logo {
    display: block;
    margin: 0 auto 2rem;
  }

  @media (min-width: 1024px) {
    header {
      display: flex;
      place-items: center;
      padding-right: calc(var(--section-gap) / 2);
      
    }

    .logo {
      margin: 0 2rem 0 0;
    }

    header .wrapper {
      display: flex;
      place-items: flex-start;
      flex-wrap: wrap;
    }
  }
</style>
