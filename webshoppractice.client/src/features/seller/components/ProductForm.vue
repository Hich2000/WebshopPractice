<script setup lang="ts">
import type { ProductFormData } from '@/features/product/composables/product';

const model = defineModel<ProductFormData>({ required: true });
</script>

<template>
  <p v-if="model.success === true" class="form-success">
    New product successfully created.
  </p>
  <p v-else-if="model.success === false" class="form-error">
    Could not create product.
  </p>

  <p>
    <label for="name" class="form-label">Product name</label>
    <input id="name" v-model="model.productName" class="form-input" type="text" placeholder="product name"
      autocomplete="new-password">
  </p>

  <p>
    <label for="price" class="form-label">Product price</label>
    <input id="price" v-model="model.productPrice" class="form-input" type="numeric" placeholder="1.00">
  </p>
</template>
