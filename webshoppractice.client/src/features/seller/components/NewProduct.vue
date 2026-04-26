<script setup lang="ts">
import { Button } from 'primevue';
import { ref, type Ref } from 'vue';
import { useProduct } from '@/features/product/composables/product';

const { createProduct } = useProduct();

const productName: Ref<string> = ref('')
const productPrice: Ref<number> = ref(1.00)
const success: Ref<boolean | null> = ref(null)

async function onSubmit() {
  success.value = null;
  success.value = await createProduct(productName.value, productPrice.value);
}
</script>

<template>
  <form @submit.prevent="onSubmit">

    <p v-if="success === true" class="form-success">
      New product successfully created.
    </p>
    <p v-else-if="success === false" class="form-error">
      Could not create product.
    </p>

    <p>
      <label for="name" class="form-label">Product name</label>
      <input id="name" v-model="productName" class="form-input" type="text" placeholder="product name"
        autocomplete="new-password">
    </p>

    <p>
      <label for="price" class="form-label">Product price</label>
      <input id="price" v-model="productPrice" class="form-input" type="numeric" placeholder="1.00">
    </p>

    <Button type="submit" label="Create" />
  </form>
</template>
