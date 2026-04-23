<script setup lang="ts">
import { useUser } from '@/composables/user';
import { router } from '@/main';
import { Button, Checkbox } from 'primevue';
import { ref } from 'vue';

const { currentUser, deleteUser, logout } = useUser();
const checkboxChecked = ref(false);
const error = ref("");

if (currentUser.value == null) {
  router.push('/');
}

async function deleteAccount() {
  const success = await deleteUser(currentUser.value!.id);
  if (success) {
    await logout();
    router.push('/AccountDeleted');
  } else {
    error.value = "Could not delete account information, please try again later."
  }
}
</script>

<template>
  <div class="form-div">
    <p v-if="error" class="form-error">
      {{ error }}
    </p>
    <p>
      <label>
        <Checkbox v-model="checkboxChecked" binary />
        I understand that by performing this action my data will be deleted and cannot be recovered.
      </label>
    </p>
    <br>
    <p>
      <Button :disabled="!checkboxChecked" label="Delete my account" color="red" @click="deleteAccount" />
    </p>
  </div>
</template>
