<script lang="ts">
  import { defineComponent } from 'vue';

  // import HelloWorld from './components/HelloWorld.vue'
  // import TheWelcome from './components/TheWelcome.vue'
  import loginForm from './components/loginComponents/loginForm.vue'
  import ProductPage from './components/productComponents/ProductPage.vue'

  export default defineComponent({
    components: {
      loginForm,
      ProductPage
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
 
  <Menubar :model="items" position="left">
    <template #start style="width: 1rem">
      <img src="@/assets/logo.svg" width="35" />
    </template>
    <template #item="{ item, props, hasSubmenu, root }">
        <a class="flex items-center" v-bind="props.action">
            <span>{{ item.label }}</span>
            <span v-if="item.shortcut" class="ml-auto border border-surface rounded bg-emphasis text-muted-color text-xs p-1">{{ item.shortcut }}</span>
            <i v-if="hasSubmenu" :class="['pi pi-angle-down ml-auto', { 'pi-angle-down': root, 'pi-angle-right': !root }]"></i>
        </a>
    </template>
  </Menubar>

  <!--<header>
    <img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />
    <div class="wrapper">
      <HelloWorld msg="You did it!" />
    </div>
  </header>-->


  <main>
    <!--<TheWelcome />-->
    <loginForm />
    <!-- <ProductPage /> -->
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
