<script setup lang="ts">
import { onMounted, reactive } from 'vue'
import { type SellerRegistrationData, useSeller } from '../composables/seller';
import { router } from '@/main';
import RegisterSellerForm from './RegisterSellerForm.vue';
import { Button } from 'primevue';

const { currentSeller, fetchSellerData, updateSellerInfo } = useSeller();

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

onMounted(async () => {
  await fetchSellerData()
  if (currentSeller === null) {
    router.push('/login');
  }

  sellerForm.organizationName = currentSeller.value!.organizationName;
  sellerForm.commerceNumber = currentSeller.value!.commerceNumber
  sellerForm.country = currentSeller.value!.country
  sellerForm.city = currentSeller.value!.city
  sellerForm.postalCode = currentSeller.value!.postalCode
  sellerForm.address = currentSeller.value!.address
});

async function onSubmit() {
  const updateResponse = await updateSellerInfo(sellerForm);

  if (updateResponse) {
    sellerForm.success = "Successfully updated information.";
  } else {
    sellerForm.errors = ["Could not update information."];
  }
}
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="onSubmit">
      <h1>Seller information</h1>
      <RegisterSellerForm v-model="sellerForm" />
      <br>
      <Button type="submit" label="Update" />
    </form>
  </div>
</template>
