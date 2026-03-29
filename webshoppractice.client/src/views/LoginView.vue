<script lang="ts">
import { defineComponent } from 'vue';
import LoginForm from '@/components/loginComponents/loginForm.vue';
import RegisterForm from '@/components/loginComponents/RegisterForm.vue';

export default defineComponent({
  components: {
    LoginForm,
    RegisterForm
  }
})
</script>

<template>
  <div class="login-grid">
    <div>
      <LoginForm />
    </div>
    <div>
      <RegisterForm />
    </div>
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
