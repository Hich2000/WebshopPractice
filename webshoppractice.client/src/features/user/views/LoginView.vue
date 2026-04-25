<script setup lang="ts">
import RegisterForm from '@/features/user/components/RegisterForm.vue';
import LoginForm from '@/features/user/components/LoginForm.vue';
import RegisterSellerForm from '@/features/seller/components/RegisterSellerForm.vue';


import { reactive, ref } from 'vue';
import { Button, Checkbox } from 'primevue';
import { useSeller, type Seller, type SellerRegistrationData } from '@/features/seller/composables/seller';
import { useUser, type UserRegistrationData } from '@/shared/composables/user';


const { registerCustomer } = useUser();
const { registerSeller } = useSeller();
const isSeller = ref<boolean>(false);

const userForm = reactive<UserRegistrationData>({
    name: '',
    email: '',
    password: '',
    errors: null,
    success: null
})

const sellerForm = reactive<SellerRegistrationData>({
  organizationName: '',
  commerceNumber: '',
  country: '',
  city: '',
  postalCode: '',
  address: '',
  errors: null,
  success: null
})

async function onSubmit() {
  //first the user account
  userForm.success = null;
  userForm.errors = null;

  let newUserId: string | null = null;

  const userAccountResponse = await registerCustomer(
    userForm.name,
    userForm.email,
    userForm.password,
  );

  if (userAccountResponse.success) {
    userForm.success = "Account successfully registered";
    userForm.email = '';
    userForm.name = '';
    userForm.password = '';
    newUserId = userAccountResponse.data;
  } else {
    userForm.errors = userAccountResponse.errors;
  }

  //then the seller information
  if (isSeller.value && newUserId !== null) {
    const newSeller: Seller = {
      id: null,
      organizationName: sellerForm.organizationName,
      commerceNumber: sellerForm.commerceNumber,
      country: sellerForm.country,
      city: sellerForm.city,
      postalCode: sellerForm.postalCode,
      address: sellerForm.address,
      verified: null
    };
    const sellerAccountResponse = await registerSeller(newSeller, newUserId);

    if (sellerAccountResponse.success) {
      sellerForm.success = "Seller account successfully registered";
    } else {
      sellerForm.errors = sellerAccountResponse.errors;
    }
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
        <p>
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
