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
  <div class="formDiv">
    <form @submit.prevent="onsubmit">
      <p v-if="error" style="color: red;">
        {{ error }}
      </p>

      <p>Login</p>
      <p>
        <input v-model="email" class="formInput" type="text" placeholder="E-mail" required>
      </p>
      <p>
        <input v-model="password" class="formInput" type="password" placeholder="Password" required>
      </p>
      <PButton type="submit" label="Login" :loading="busy" />
    </form>
  </div>
</template>
