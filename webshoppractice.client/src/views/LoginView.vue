<script setup lang="ts">
import LoginForm from '@/components/loginComponents/LoginForm.vue';
import RegisterForm from '@/components/loginComponents/RegisterForm.vue';
import { useUser } from '@/composables/user';

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
