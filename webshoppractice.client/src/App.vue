<script lang="ts">
  import { defineComponent } from 'vue';
  import { RouterView } from 'vue-router';

  export default defineComponent({
    components: {
      RouterView
    },
    data() {
      return {
        items: [
          {
            label: 'Home',
            icon: 'pi pi-home',
            command: () => alert('hello1')
          },
          {
            label: 'Account',
            icon: 'pi pi-user',
            command: () => alert('hello2')
          },
        ]
      }
    }
  })

</script>

<template> 
  <main>
    <RouterView />
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
