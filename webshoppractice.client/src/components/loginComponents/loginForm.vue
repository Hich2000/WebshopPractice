<script lang="ts">
import { router } from '@/main';
import { defineComponent } from 'vue';
import { useUser } from '@/composables/user'

const { fetchCurrentUser, login } = useUser()

interface LoginFormData {
  email: string,
  password: string,
  error: null | string,
  busy: boolean
}

export default defineComponent({
  data(): LoginFormData {
    return {
      email: '',
      password: '',
      error: null,
      busy: false
    }
  },
  methods: {
    async onsubmit() {
      this.error = null;
      this.busy = true;

      const success = await login(this.email, this.password)
      console.log(success);

      if (success) {
        this.busy = false
        fetchCurrentUser(true)
        router.push('/')
      } else {
        this.error = "Invalid email or password.";
        this.busy = false
      }
    },
  }
});
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="onsubmit">
      <h1>Login</h1>

      <p class="form-error">
        {{ error }}
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
      <PButton type="submit" label="Login" :loading="busy" />
    </form>
  </div>
</template>
