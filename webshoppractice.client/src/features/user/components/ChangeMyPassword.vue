<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useUser, type PassWordError } from '@/composables/user'
import { Button } from 'primevue'

const { fetchCurrentUser, changeMyPassword } = useUser()

const oldPassword = ref<string>('')
const newPassword = ref<string>('')
const verifyPassword = ref<string>('')

const error = ref<PassWordError[] | null>(null)
const success = ref<string | null>(null)

onMounted(async () => {
  await fetchCurrentUser()
})

async function onSubmit() {
  error.value = null
  success.value = null

  if (newPassword.value !== verifyPassword.value) {
    error.value = [
      {
        code: 'VerificationFailure',
        description: 'New password does not match verification'
      }
    ]
    return
  }

  const response = await changeMyPassword(
    oldPassword.value,
    newPassword.value,
    verifyPassword.value
  )

  if (response === true) {
    success.value = 'Password has been updated.'
  } else {
    error.value = response
  }

  oldPassword.value = ''
  newPassword.value = ''
  verifyPassword.value = ''
}
</script>

<template>
  <div class="form-div">
    <form @submit.prevent="onSubmit">
      <h1>Change password</h1>

      <p v-for="e in error" :key="e.code" class="form-error">
        {{ e.description }}
      </p>

      <p v-if="success" class="form-success">
        {{ success }}
      </p>

      <p>
        <label for="oldPassword" class="form-label">Current Password</label>
        <input id="oldPassword" v-model="oldPassword" class="form-input" type="password" placeholder="Current password"
          required>
      </p>

      <p>
        <label for="newPassword" class="form-label">New Password</label>
        <input id="newPassword" v-model="newPassword" class="form-input" type="text" placeholder="New password"
          autocomplete="new-password" required>
      </p>

      <p>
        <label for="VerifyNewPassword" class="form-label">Verify new Password</label>
        <input id="VerifyNewPassword" v-model="verifyPassword" class="form-input" type="text"
          placeholder="Verify new password" autocomplete="verify-new-password" required>
      </p>

      <br>

      <Button type="submit" label="Update" />
    </form>
  </div>
</template>
