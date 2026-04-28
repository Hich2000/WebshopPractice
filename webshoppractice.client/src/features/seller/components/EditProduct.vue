<script setup lang="ts">
import type { ProductFormData } from '@/features/product/composables/product';
import ProductForm from './ProductForm.vue';
import { onMounted, ref, type Ref } from 'vue';
import { useProduct } from '@/features/product/composables/product';
import { useRoute } from 'vue-router';
import { router } from '@/main';
import { Button } from 'primevue';

const { getProductInfo, updateProductInfo } = useProduct();
const productId = (useRoute().params.id ?? '').toString();

const model: Ref<ProductFormData> = ref({
  productName: '',
  productPrice: 1.00,
  success: null
})

async function onSubmit() {
  model.value.success = null;
  model.value.success = await updateProductInfo(productId, model.value.productName, model.value.productPrice);
}

onMounted(async () => {
  const product = await getProductInfo(productId);
  if (product === false) {
    router.push('/Seller');
  } else {
    model.value.productName = product.name;
    model.value.productPrice = product.price
  }
});
</script>

<template>
  {{ $route.params.id }}
  <form class="form-div" @submit.prevent="onSubmit">
    <ProductForm v-model="model" />
    <br>
    <Button type="submit" label="Update" />
  </form>
</template>
