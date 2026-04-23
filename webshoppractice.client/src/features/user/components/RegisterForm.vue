<script setup lang="ts">
import type { UserRegistrationData } from '@/shared/composables/registrationData';

const model = defineModel<UserRegistrationData>({ required: true });
</script>

<template>
  <p v-for="e in model.error" :key="e.code" class="form-error">
    {{ e.description }}
  </p>
  <p v-if="model.success" class="form-success">
    {{ model.success }}
  </p>
  <p>
    <label for="name" class="form-label">Name</label>
    <input id="name" v-model="model.name" class="form-input" type="text" placeholder="Full name" autocomplete="off"
      >
  </p>
  <p>
    <label for="email" class="form-label">E-mail</label>
    <input id="email" v-model="model.email" class="form-input" type="text" placeholder="E-mail"
      autocomplete="new-username" >
  </p>
  <p>
    <label for="password" class="form-label">Password</label>
    <input id="password" v-model="model.password" class="form-input" type="password" placeholder="Password"
      autocomplete="new-password" >
  </p>
</template>
