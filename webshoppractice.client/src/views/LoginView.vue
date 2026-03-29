<script lang="ts">
import { defineComponent } from 'vue';
import loginForm from '@/components/loginComponents/loginForm.vue';

export default defineComponent({
  components: {
    loginForm
  }
})
</script>

<template>
  <div class="login-grid">
    <div>
      <loginForm />
    </div>
    <div>test</div>
  </div>
</template>

<style scoped lang="css">
.login-grid {
  display: grid;
  grid-template-columns: 50% 50%;
  padding: 2rem;
  border: 1px solid #ddd;
  border-radius: 10px;
  padding: 16px;
}
</style>
