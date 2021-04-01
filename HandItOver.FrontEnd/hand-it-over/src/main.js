import Vue from 'vue'
import App from './App.vue'
import VueRouter from 'vue-router'
import MainPage from "~/components/main/MainPage"
import AdminPage from '~/components/admin/AdminPage'
import UsersPage from '~/components/admin/UsersPage'
import MailboxPage from '~/components/mailboxes/MailboxPage'
import GroupPage from '~/components/groups/GroupPage'
import MakeRentPage from '~/components/rent/MakeRentPage'
import RentPage from '~/components/rent/RentPage'
import UserPage from '~/components/userAccount/UserPage'
import DeliveryPage from '~/components/deliveries/DeliveryPage'
import JoinGroupPage from '~/components/joinGroup/JoinGroupPage'

Vue.config.productionTip = false
Vue.use(VueRouter)

const routes = [
  { path: '/', component: MainPage },
  { path: '/mailbox', component: MailboxPage },
  { path: '/admin', component: AdminPage },
  { path: '/users', component: UsersPage },
  {
    path: '/group/:tab/:id', component: GroupPage, props: r => (
      { tab: r.params.tab, groupId: r.params.id })
  },
  {
    path: '/rentMailbox/:id', component: MakeRentPage, props: r => (
      { groupId: r.params.id })
  },
  {
    path: '/rent/:id', component: RentPage, props: r => (
      { rentId: r.params.id })
  },
  {
    path: '/account/:id', component: UserPage, props: r => (
      { userId: r.params.id })
  },
  {
    path: '/myaccount', component: UserPage, props: r => (
      { userId: null })
  },
  {
    path: '/delivery/:id', component: DeliveryPage, props: r => (
      { deliveryId: r.params.id })
  },
  {
    path: '/join/:groupId/:token', component: JoinGroupPage, props: r => (
      { groupId: r.params.groupId, token: r.params.token })
  },
]
const router = new VueRouter({ routes, mode: 'history' })

new Vue({
  render: h => h(App),
  router: router
}).$mount('#app')