<script lang="ts">
import { defineComponent } from 'vue';

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

      try {
        const response = await fetch("/login/login", {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          credentials: "include",
          body: JSON.stringify({
            username: this.username,
            password: this.password
          })
        })
        if (!response.ok) {
          throw new Error();
        }
        // Todo now that we are logged in we can redirect to a user info page
        console.log(response);
        this.busy = false
      } catch (e: unknown) {
        console.log(e);
        this.error = "Invalid username or password.";
        this.busy = false
      }
    },
    async me() {
      this.error = null;
      try {
        const response = await fetch("login/me", {
          credentials: "include",
          headers: {
            "Content-Type": "application/json"
          },
        })

        if (!response.ok) {
          throw new Error()
        }

        const result = await response.json()
        this.error = result.username;
      } catch (e: unknown) {
        console.log(e)
        this.error = "not logged in"
      }
    },
    async logout() {
      this.error = null;
      const response = await fetch("login/logout", {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json"
        },
      })
      if (!response.ok) {
        throw new Error();
      }
      this.error = "logged out";
    }
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
        <input v-model="username" class="loginInput" type="text" placeholder="E-mail" required>
      </p>
      <p>
        <input v-model="password" class="loginInput" type="password" placeholder="Password" required>
      </p>
      <PButton type="submit" label="Login" :loading="busy" />
    </form>

    <PButton type="button" label="me" v-on:click="me" />
    <PButton type="button" label="logout" v-on:click="logout" />
  </div>
</template>

<style scoped>
.formDiv {
  text-align: center;
  padding: 6rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

.loginInput {
  width: 100%;
}
</style>
