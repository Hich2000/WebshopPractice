<script lang="ts">
import { defineComponent } from 'vue';

interface UserRegistrationData {
  name: string,
  username: string,
  message: string | null
}

export default defineComponent({
  data(): UserRegistrationData {
    return {
      name: '',
      username: '',
      message: null
    }
  },
});
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="">
      <p>Register</p>
      <p>
        <input v-model="name" class="form-input" type="text" placeholder="Full name" required>
      </p>
      <p>
        <input v-model="username" class="form-input" type="text" placeholder="E-mail" required>
      </p>
      <PButton type="submit" label="Update" />
    </form>
  </div>
</template>
