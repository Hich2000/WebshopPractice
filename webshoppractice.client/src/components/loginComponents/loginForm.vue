<script lang="ts">
import { defineComponent } from 'vue';

interface LoginFormData {
    username: string,
    password: string,
    error: null | string
}

export default defineComponent({
    data(): LoginFormData {
        return {
            username: '',
            password: '',
            error: null
        }
    },
    methods: {
        async login() {
            this.error = null;

            try {
                const response = await fetch("/login/login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    credentials: "include",
                    body: JSON.stringify({
                        username: this.username,
                        password: this.password
                    })
                })
                if (!response.ok) {
                    throw new Error();
                }
                // Todo now that we are logged in we can redirect to a user info page
                console.log(response);
            } catch(e: any) {
                this.error = "Invalid username or password.";
            }
        }
    }
});
</script>

<template>
    <div class="formDiv">
        <form @submit.prevent="login">
            <p v-if="error" style="color: red;">
                {{ error }}
            </p>

            <p>Login</p>
            <p>
                <input v-model="username" class="loginInput" type="text" placeholder="E-mail" required>
            </p>
            <p>
                <input v-model="password" class="loginInput" type="password" placeholder="Password" required>
            </p>
            <button type="submit">
                Login
            </button>
        </form>
    </div>
</template>

<style scoped>
.formDiv {
    text-align: center;
    border: 1px solid #ddd;
    border-radius: 10px;
    padding: 16px;
    width: 300px;
    background: #fff;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

.loginInput {
    width: 100%;
}
</style>
