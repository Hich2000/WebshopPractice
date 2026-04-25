<script setup lang="ts">
import type { SellerRegistrationData } from '../composables/seller';

const model = defineModel<SellerRegistrationData>({ required: true });
</script>

<template>
  <p v-for="e in model.errors" :key="e" class="form-error">
    {{ e }}
  </p>
  <p v-if="model.success" class="form-success">
    {{ model.success }}
  </p>
  <p>
    <label for="organizationName" class="form-label">Organization name</label>
    <input id="organizationName" v-model="model.organizationName" class="form-input" type="text" placeholder="Organization name" autocomplete="off"
      required>
  </p>
  <p>
    <label for="commerceNumber" class="form-label">Commerce Number</label>
    <input id="commerceNumber" v-model="model.commerceNumber" class="form-input" type="text" placeholder="Commerce number"
      autocomplete="off" required>
  </p>
  <p>
    <label for="country" class="form-label">Country</label>
    <input id="country" v-model="model.country" class="form-input" type="text"
      placeholder="country" autocomplete="off" required>
  </p>
  <p>
    <label for="city" class="form-label">City</label>
    <input id="city" v-model="model.city" class="form-input" type="text" placeholder="city" autocomplete="off"
      required>
  </p>
  <p>
    <label for="postalCode" class="form-label">Postal Code</label>
    <input id="postalCode" v-model="model.postalCode" class="form-input" type="text" placeholder="Postal code" autocomplete="off" required>
  </p>
  <p>
    <label for="address" class="form-label">Address</label>
    <input id="address" v-model="model.address" class="form-input" type="text" placeholder="address" autocomplete="off" required>
  </p>
</template>
