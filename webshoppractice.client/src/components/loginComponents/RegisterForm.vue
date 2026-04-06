<script lang="ts">
import { defineComponent } from 'vue';
import { register } from '@/composables/user';

interface UserRegistrationData {
  name: string,
  email: string,
  password: string,
  error: string | null,
  success: string | null
}

export default defineComponent({
  data(): UserRegistrationData {
    return {
      name: '',
      email: '',
      password: '',
      error: null,
      success: null
    }
  },
  methods: {
    async register() {
      const response = await register(this.name, this.email, this.password);
      if (response) {
        this.success = "Account successfully registered";
      }  else {
        this.error = "Account registration failed";
      }
    }
  }
});
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="register">
      <h1>Register</h1>

      <p class="form-error" style="color: black !important;">
        Don't have an account yet? Register now.
      </p>

      <p v-if="error" class="form-error">
        {{ error }}
      </p>
      <p v-if="success" class="form-success">
        {{ success }}
      </p>

      <p>
        <label for="name" class="form-label">Name</label>
        <input id="name" v-model="name" class="form-input" type="text" placeholder="Full name" autocomplete="off" required>
      </p>
      <p>
        <label for="email" class="form-label">E-mail</label>
        <input id="email" v-model="email" class="form-input" type="text" placeholder="E-mail" autocomplete="new-username" required>
      </p>
      <p>
        <label for="password" class="form-label">Password</label>
        <input id="password" v-model="password" class="form-input" type="password" placeholder="Password" autocomplete="new-password" required>
      </p>
      <br>
      <PButton type="submit" label="Register" />
    </form>
  </div>
</template>
