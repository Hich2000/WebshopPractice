<script lang="ts">
import { router } from '@/main';
import { defineComponent } from 'vue';
import { useUser } from '@/composables/user'

const { fetchCurrentUser } = useUser()

interface LoginFormData {
  username: string,
  password: string,
  error: null | string,
  busy: boolean
}

export default defineComponent({
  data(): LoginFormData {
    return {
      username: '',
      password: '',
      error: null,
      busy: false
    }
  },
  methods: {
    async login() {
      this.error = null;
      this.busy = true;

      await fetch("/login/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        credentials: "include",
        body: JSON.stringify({
          username: this.username,
          password: this.password
        })
      }).then(() => {
        this.busy = false
        fetchCurrentUser(true)
        router.push('/')
      }).catch(() => {
        this.error = "Invalid username or password.";
        this.busy = false
      })

    },
  }
});
</script>

<template>
  <div class="formDiv">
    <form @submit.prevent="login">
      <p v-if="error" style="color: red;">
        {{ error }}
      </p>

      <p>Login</p>
      <p>
        <input v-model="username" class="formInput" type="text" placeholder="E-mail" required>
      </p>
      <p>
        <input v-model="password" class="formInput" type="password" placeholder="Password" required>
      </p>
      <PButton type="submit" label="Login" :loading="busy" />
    </form>
  </div>
</template>
