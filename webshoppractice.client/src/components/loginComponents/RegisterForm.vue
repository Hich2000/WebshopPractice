<script lang="ts">
import { defineComponent } from 'vue';

interface UserRegistrationData {
  name: string,
  username: string,
  password: string,
  message: string | null
}

export default defineComponent({
  data(): UserRegistrationData {
    return {
      name: '',
      username: '',
      password: '',
      message: null
    }
  },
});
</script>

<template>
  <div class="formDiv">
    <form @submit.prevent="">
      <p>Register</p>
      <p>
        <input v-model="name" class="loginInput" type="text" placeholder="Full name" required>
      </p>
      <p>
        <input v-model="username" class="loginInput" type="text" placeholder="E-mail" required>
      </p>
      <p>
        <input v-model="password" class="loginInput" type="password" placeholder="Password" required>
      </p>
      <PButton type="submit" label="Login" />
    </form>
  </div>
</template>

<style scoped>
.formDiv {
  text-align: center;
  padding: 6rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  height: 100%;
}

.loginInput {
  width: 100%;
}
</style>
