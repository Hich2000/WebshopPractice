import './assets/main.css'
import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'

import { createApp } from 'vue'
import App from './App.vue'

const app = createApp(App)
    .use(PrimeVue, {
        theme: {
            preset: Aura
        }
    });

// components
import Button from 'primevue/button'
app.component('Button', Button)

app.mount('#app');
