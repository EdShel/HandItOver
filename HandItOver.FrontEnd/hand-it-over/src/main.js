import Vue from 'vue'
import App from './App.vue'
import router from '~/router'
import { i18n } from '~/i18n'

Vue.config.productionTip = false;

new Vue({
  render: h => h(App),
  router: router,
  i18n: i18n
}).$mount('#app')