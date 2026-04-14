<script setup lang="ts">
import { ref } from 'vue';
import type { UserRegistrationData, PassWordError } from '@/composables/user';

const props = defineProps<{
  register: (name: string, email: string, password: string) => Promise<true | PassWordError[]>,
  introText: string
}>();

const name = ref<UserRegistrationData['name']>('');
const email = ref<UserRegistrationData['email']>('');
const password = ref<UserRegistrationData['password']>('');
const error = ref<UserRegistrationData['error']>(null);
const success = ref<UserRegistrationData['success']>(null);

async function onSubmit() {
  success.value = null;
  error.value = null;

  const response = await props.register(name.value, email.value, password.value);

  if (response === true) {
    success.value = "Account successfully registered";
  } else {
    error.value = response;
  }
}
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="onSubmit">
      <h1>Register</h1>

      <p class="form-error" style="color: black !important;">
        {{ introText }}
      </p>

      <p v-for="e in error" :key="e.code" class="form-error">
        {{ e.description }}
      </p>

      <p v-if="success" class="form-success">
        {{ success }}
      </p>

      <p>
        <label for="name" class="form-label">Name</label>
        <input id="name" v-model="name" class="form-input" type="text" placeholder="Full name" autocomplete="off"
          required>
      </p>

      <p>
        <label for="email" class="form-label">E-mail</label>
        <input id="email" v-model="email" class="form-input" type="text" placeholder="E-mail"
          autocomplete="new-username" required>
      </p>

      <p>
        <label for="password" class="form-label">Password</label>
        <input id="password" v-model="password" class="form-input" type="password" placeholder="Password"
          autocomplete="new-password" required>
      </p>

      <br>
      <PButton type="submit" label="Register" />
    </form>
  </div>
</template>
