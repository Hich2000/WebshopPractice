<script setup lang="ts">
import RegisterForm from '@/features/user/components/RegisterForm.vue';
import { useUser } from '@/shared/composables/user';
import LoginForm from '@/features/user/components/LoginForm.vue';
import { reactive, ref, type Ref } from 'vue';
import { type SellerRegistrationData, type UserRegistrationData } from '@/shared/composables/registrationData';
import { Button, Checkbox } from 'primevue';
import RegisterSellerForm from '@/features/seller/components/RegisterSellerForm.vue';

const { registerCustomer } = useUser();
const isSeller = ref<boolean>(false);
const userCreationSuccess: Ref<boolean|null> = ref(null);

//todo create a seller registration setup
const userForm = reactive<UserRegistrationData>({
    name: '',
    email: '',
    password: '',
    error: null,
    success: null
})

const sellerForm = reactive<SellerRegistrationData>({
  organizationName: '',
  commerceNumber: '',
  country: '',
  city: '',
  postalCode: '',
  address: '',
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
    userCreationSuccess.value = true;
  } else {
    userForm.error = userAccountResponse;
    userCreationSuccess.value = false;
  }

  //then the seller information
  if (isSeller.value && userCreationSuccess.value) {
    console.log("I am here");
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

        <p style="text-align: start;">
          <br>
          <label>
            <Checkbox v-model="isSeller" binary />
            I am a seller.
          </label>
        </p>

        <div v-if="isSeller">
          <p style="color: blue;">
            If you already have an account, you can register as a seller from your profile page.
          </p>
          <RegisterSellerForm  v-model="sellerForm" />
        </div>

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
