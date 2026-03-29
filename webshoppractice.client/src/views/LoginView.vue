<script lang="ts">
import { defineComponent } from 'vue';
import LoginForm from '@/components/loginComponents/LoginForm.vue';
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
  grid-template-columns: repeat(2, 1fr);
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 10px;
}
</style>
