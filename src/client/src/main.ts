import './styles/main.css'
import '@mdi/font/css/materialdesignicons.css' // Ensure you are using css-loader

import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

const app = createApp(App)

const vuetify = createVuetify({
	components,
	directives,
	icons: {
		defaultSet: 'mdi',
  },
})

app.use(vuetify)
app.use(router)

app.mount('#app')
