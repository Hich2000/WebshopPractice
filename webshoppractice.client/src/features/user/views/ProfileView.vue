<script setup lang="ts">
import ProfileLinks from '@/features/user/components/ProfileLinks.vue';
</script>

<template>
  <div class="profile-grid">
    <div>
      <ProfileLinks />
    </div>
    <div>
      <RouterView />
    </div>
  </div>
</template>

<style scoped lang="css">
.profile-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 10px;
}
</style>
