<script lang="ts">
import ProfileLinks from '@/components/profileComponents/ProfileLinks.vue';
import { defineComponent } from 'vue';

export default defineComponent({
  components: {
    ProfileLinks
  }
})
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
