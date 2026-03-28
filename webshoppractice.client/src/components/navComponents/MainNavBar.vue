<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
    data() {
        return {
            items: [
                {
                    label: "Home",
                    route: "/"
                },
                {
                    label: "Login",
                    route: "/Login"
                }
            ]
        }
    }
})
</script>

<template>
    <Menubar class="mainNavBar" :model="items">
        <template #item="{ item }">
            <RouterLink :to="item.route">
                {{ item.label }}
            </RouterLink>
        </template>
    </Menubar>
</template>

<style lang="css" scoped>
.mainNavBar {
    display: block;
    position: absolute;
    top: 2rem;
    left: 16rem;
    right: 16rem;
    z-index: 9999;
    padding: 0 20px
}

.mainNavBar ul {
    list-style: none;
    display: flex;
    gap: 20px;  /* space between menu items */
    margin: 0;
    padding: 0;
}

.mainNavBar li {
    cursor: pointer;
}
</style>