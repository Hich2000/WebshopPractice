import './assets/main.css'
import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'
import 'primeicons/primeicons.css'

import { createApp } from 'vue'
import App from './App.vue'

const app = createApp(App)
    .use(PrimeVue, {
        theme: {
            preset: Aura,
        }
    });

// components
import Button from 'primevue/button'
app.component('Button', Button)
import Menubar from 'primevue/menubar'
app.component('Menubar', Menubar)

app.mount('#app');
