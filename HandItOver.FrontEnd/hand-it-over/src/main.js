import Vue from 'vue'
import App from './App.vue'
import VueRouter from 'vue-router'
import HelloWorld from "./components/HelloWorld.vue"
import Admin from './components/Admin.vue'

Vue.config.productionTip = false
Vue.use(VueRouter)

const routes = [
  { path: '/', component: HelloWorld },
  { path: '/admin', component: Admin }
]

const router = new VueRouter({ routes, mode: 'history' })

new Vue({
  render: h => h(App),
  router: router
}).$mount('#app')
