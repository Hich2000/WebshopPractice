<script setup lang="ts">
import RegisterForm from '@/features/user/components/RegisterForm.vue';
import { useUser } from '@/shared/composables/user';
import LoginForm from '@/features/user/components/LoginForm.vue';
import { reactive } from 'vue';
import { type UserRegistrationData } from '@/shared/composables/registrationData';
import { Button } from 'primevue';

const { registerCustomer } = useUser();

//todo create a seller registration setup
const userForm = reactive<
  UserRegistrationData
>({
    name: '',
    email: '',
    password: '',
    error: null,
    success: null
})

async function onSubmit() {

  //first the user account
  userForm.success = null;
  userForm.error = null;

  const userAccountResponse = await registerCustomer(
    userForm.name,
    userForm.email,
    userForm.password,
  );

  if (userAccountResponse === true) {
    userForm.success = "Account successfully registered";
  } else {
    userForm.error = userAccountResponse;
  }
}

</script>

<template>
  <div class="login-grid">
    <div>
      <LoginForm />
    </div>
    <div class="form-div">
      <form @submit.prevent="onSubmit">
        <h1>Register</h1>
        <p class="form-error" style="color: black !important;">
          Don't have an account yet? Register now.
        </p>

        <RegisterForm v-model="userForm" />
        <br>
        <Button type="submit" label="Register" />
      </form>
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
