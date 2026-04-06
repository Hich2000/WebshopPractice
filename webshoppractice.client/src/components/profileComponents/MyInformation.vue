<script lang="ts">
import { useUser } from '@/composables/user';
import { defineComponent } from 'vue';

const { currentUser, fetchCurrentUser, updateInfo } = useUser()

interface UpdateInformationData {
  id: string
  name: string,
  email: string,
  error: string | null,
  success: string | null
}

export default defineComponent({
  data(): UpdateInformationData {
    return {
      id: '',
      name: '',
      email: '',
      error: null,
      success: null
    }
  },
  async created() {
    await fetchCurrentUser();
    this.id = currentUser.value!.id
    this.name = currentUser.value!.name
    this.email = currentUser.value!.email
  },
  methods: {
    async onSubmit() {
      const response = await updateInfo(this.id, this.name, this.email);
      console.log(response);
      if (response) {
        this.success = "Account information updated successfully";
      }  else {
        this.error = "Could not update account information.";
      }
    }
  }
});
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
        <input id="name" v-model="name" class="form-input" type="text" placeholder="Full name" autocomplete="off" required>
      </p>
      <p>
        <label for="email" class="form-label">E-mail</label>
        <input id="email" v-model="email" class="form-input" type="text" placeholder="E-mail" autocomplete="new-username" required>
      </p>
      <br>
      <PButton type="submit" label="Update" />
    </form>
  </div>
</template>
