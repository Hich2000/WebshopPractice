<script setup lang="ts">
import { Button } from 'primevue';
import { ref, type Ref } from 'vue';
import { useProduct, type ProductFormData } from '@/features/product/composables/product';
import ProductForm from './ProductForm.vue';

const { createProduct } = useProduct();

const model: Ref<ProductFormData> = ref({
  productName: '',
  productPrice: 1.00,
  success: null
})

async function onSubmit() {
  model.value.success = null;
  model.value.success = await createProduct(model.value.productName, model.value.productPrice);
}
</script>

<template>
  <form class="form-div" @submit.prevent="onSubmit">
    <ProductForm v-model="model" />
    <br>
    <Button type="submit" label="Create" />
  </form>
</template>
