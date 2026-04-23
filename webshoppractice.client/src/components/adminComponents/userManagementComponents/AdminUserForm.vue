<script setup lang="ts">
import RegisterForm from '@/features/user/components/RegisterForm.vue';
import { useUser } from '@/shared/composables/user';

const { registerAdmin } = useUser();
</script>

<template>
  <RegisterForm :register="registerAdmin" intro-text="Register a new admin user account here"/>
</template>
