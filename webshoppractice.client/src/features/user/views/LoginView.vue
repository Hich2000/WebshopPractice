<script setup lang="ts">
import RegisterForm from '@/features/user/components/RegisterForm.vue';
import { useUser } from '@/shared/composables/user';
import LoginForm from '@/features/user/components/LoginForm.vue';

const { registerCustomer } = useUser();
</script>

<template>
  <div class="login-grid">
    <div>
      <LoginForm />
    </div>
    <div>
      <RegisterForm :register="registerCustomer" intro-text="Don't have an account yet? Register now."/>
    </div>
  </div>
</template>

<style scoped lang="css">
.login-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 10px;
}
</style>
