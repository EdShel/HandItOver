import Vue from 'vue'
import App from './App.vue'
import VueRouter from 'vue-router'
import MainPage from "~/components/main/MainPage"
import Admin from './components/Admin.vue'
import MailboxPage from '~/components/mailboxes/MailboxPage'
import GroupPage from '~/components/groups/GroupPage'

Vue.config.productionTip = false
Vue.use(VueRouter)

const routes = [
  { path: '/', component: MainPage },
  { path: '/mailbox', component: MailboxPage },
  { path: '/admin', component: Admin },
  {
    path: '/group/:tab/:id', component: GroupPage, props: r => (
      { tab: r.params.tab, groupId: r.params.id })
  },
]
const router = new VueRouter({ routes, mode: 'history' })

new Vue({
  render: h => h(App),
  router: router
}).$mount('#app')