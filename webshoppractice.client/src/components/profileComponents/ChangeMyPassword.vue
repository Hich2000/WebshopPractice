<script lang="ts">

import { defineComponent } from 'vue'
import { useUser, type PassWordError } from '@/composables/user'

const { fetchCurrentUser, changeMyPassword } = useUser();

interface ChangePasswordData {
  oldPassword: string,
  newPassword: string,
  verifyPassword: string,
  error: PassWordError[] | null,
  success: string | null
}

export default defineComponent({
  data(): ChangePasswordData {
    return {
      oldPassword: "",
      newPassword: "",
      verifyPassword: "",
      error: null,
      success: null
    }
  },
  async created() {
    await fetchCurrentUser();
  },
  methods: {
    async onSubmit() {
      this.error = null;
      this.success = null;

      if (this.newPassword != this.verifyPassword) {
        this.error = [{
          code: "VerificationFailure",
          description: "New password does not match verification"
        }];
        return;
      }

      const response = await changeMyPassword(this.oldPassword, this.newPassword, this.verifyPassword);
      if (response === true) {
        this.success = "Password has been updated.";
      }  else {
        this.error = response;
      }

      this.oldPassword = "";
      this.newPassword = "";
      this.verifyPassword = "";
    }
  }
})

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
        <input id="oldPassword" v-model="oldPassword" class="form-input" type="password" placeholder="Current password" required>
      </p>
      <p>
        <label for="newPassword" class="form-label">New Password</label>
        <input id="newPassword" v-model="newPassword" class="form-input" type="text" placeholder="New password"
          autocomplete="new-password" required>
      </p>
      <p>
        <label for="VerifyNewPassword" class="form-label">Verify new Password</label>
        <input id="VerifyNewPassword" v-model="verifyPassword" class="form-input" type="text" placeholder="Verify new password"
          autocomplete="verify-new-password" required>
      </p>
      <br>
      <PButton type="submit" label="Update" />
    </form>
  </div>
</template>
