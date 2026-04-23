<script setup lang="ts">
import RegisterForm from '@/features/user/components/RegisterForm.vue';
import type { UserRegistrationData } from '@/shared/composables/registrationData';
import { useUser } from '@/shared/composables/user';
import { Button } from 'primevue';
import { reactive } from 'vue';

const { registerAdmin } = useUser();

const adminUserForm = reactive<
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
  adminUserForm.success = null;
  adminUserForm.error = null;

  const userAccountResponse = await registerAdmin(
    adminUserForm.name,
    adminUserForm.email,
    adminUserForm.password,
  );

  if (userAccountResponse === true) {
    adminUserForm.success = "Account successfully registered";
  } else {
    adminUserForm.error = userAccountResponse;
  }
}
</script>

<template>
  <form @submit.prevent="onSubmit">
    <h1>Register</h1>
    <p class="form-error" style="color: black !important;">
      Register a new admin user account here.
    </p>

    <RegisterForm v-model="adminUserForm" />
    <br>
    <Button type="submit" label="Register" />
  </form>
</template>
