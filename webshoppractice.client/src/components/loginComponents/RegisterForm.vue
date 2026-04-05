<script lang="ts">
import { defineComponent } from 'vue';

interface UserRegistrationData {
  name: string,
  email: string,
  password: string,
  message: string | null
}

export default defineComponent({
  data(): UserRegistrationData {
    return {
      name: '',
      email: '',
      password: '',
      message: null
    }
  },
});
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="">
      <h1>Register</h1>

      <p class="form-error" style="color: black !important;">
        Don't have an account yet? Register now!
      </p>

      <p>
        <label for="name" class="form-label">Name</label>
        <input id="name" v-model="name" class="form-input" type="text" placeholder="Full name" required>
      </p>
      <p>
        <label for="email" class="form-label">E-mail</label>
        <input id="email" v-model="email" class="form-input" type="text" placeholder="E-mail" required>
      </p>
      <p>
        <label for="password" class="form-label">Password</label>
        <input id="password" v-model="password" class="form-input" type="password" placeholder="Password" required>
      </p>
      <br>
      <PButton type="submit" label="Register" />
    </form>
  </div>
</template>
