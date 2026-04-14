<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useUser } from '@/composables/user'
import { Button } from 'primevue'

const { currentUser, fetchCurrentUser, updateInfo } = useUser()

const id = ref<string>('')
const name = ref<string>('')
const email = ref<string>('')

const error = ref<string | null>(null)
const success = ref<string | null>(null)

onMounted(async () => {
  await fetchCurrentUser()

  if (currentUser.value) {
    id.value = currentUser.value.id
    name.value = currentUser.value.name
    email.value = currentUser.value.email
  }
})

async function onSubmit() {
  error.value = null
  success.value = null

  const response = await updateInfo(
    id.value,
    name.value,
    email.value
  )

  if (response) {
    success.value = 'Account information updated successfully'
  } else {
    error.value = 'Could not update account information.'
  }
}
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="onSubmit">
      <h1>My information</h1>

      <p v-if="error" class="form-error">
        {{ error }}
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

      <br>

      <Button type="submit" label="Update" />
    </form>
  </div>
</template>
