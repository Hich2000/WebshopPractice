<script setup lang="ts">
import { ref } from 'vue'
import { router } from '@/main'
import { useRoute } from 'vue-router'
import { useUser } from '@/shared/composables/user'
import { Button } from 'primevue'

const { fetchCurrentUser, login } = useUser()
const route = useRoute()

const email = ref<string>('')
const password = ref<string>('')
const error = ref<string | null>(null)
const busy = ref<boolean>(false)

async function onsubmit() {
  error.value = null
  busy.value = true

  const success = await login(email.value, password.value)

  if (success) {
    busy.value = false
    fetchCurrentUser(true)
    router.push((route.query.redirect as string) || '/')
  } else {
    error.value = 'Invalid email or password.'
    busy.value = false
  }
}
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

      <Button type="submit" :disabled="busy">
        {{ busy ? 'Logging in...' : 'Login' }}
      </Button>
    </form>
  </div>
</template>
