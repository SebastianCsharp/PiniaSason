import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// PrimeVue - SOLO esto
import PrimeVue from 'primevue/config'

// Componentes PrimeVue
import Card from 'primevue/card'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'

const app = createApp(App)

app.use(PrimeVue)
app.use(router)

// Componentes globales
app.component('Card', Card)
app.component('Button', Button)
app.component('DataTable', DataTable)
app.component('Column', Column)
app.component('Tag', Tag)

app.mount('#app')