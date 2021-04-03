import Vue from 'vue'
import VueRouter from 'vue-router'
import MainPage from "~/components/main/MainPage"
import NotFoundPage from "~/components/errors/NotFoundPage"
import NoAccessPage from "~/components/errors/NoAccessPage"
import AdminPage from '~/components/admin/AdminPage'
import UsersPage from '~/components/admin/UsersPage'
import MailboxPage from '~/components/mailboxes/MailboxPage'
import GroupPage from '~/components/groups/GroupPage'
import MakeRentPage from '~/components/rent/MakeRentPage'
import RentPage from '~/components/rent/RentPage'
import UserPage from '~/components/userAccount/UserPage'
import DeliveryPage from '~/components/deliveries/DeliveryPage'
import JoinGroupPage from '~/components/joinGroup/JoinGroupPage'
import { i18n, loadLanguageAsync } from '~/i18n'
import api from '~/util/api'

Vue.use(VueRouter);

const routes = [
  { path: '/', component: MainPage, meta: { title: 'page.main' } },
  { path: '/mailbox', component: MailboxPage, meta: { title: 'page.mailbox', access: 'auth' } },
  { path: '/admin', component: AdminPage, meta: { title: 'page.admin', access: 'admin' } },
  { path: '/users', component: UsersPage, meta: { title: 'page.users', access: 'admin' } },
  {
    path: '/group/:tab/:id', component: GroupPage, props: r => (
      { tab: r.params.tab, groupId: r.params.id }), meta: { title: 'page.group' }
  },
  {
    path: '/rentMailbox/:id', component: MakeRentPage, props: r => (
      { groupId: r.params.id }), meta: { title: 'page.rentBox', access: 'auth' }
  },
  {
    path: '/rent/:id', component: RentPage, props: r => (
      { rentId: r.params.id }), meta: { title: 'page.rent', access: 'auth' }
  },
  {
    path: '/account/:id', component: UserPage, props: r => (
      { userId: r.params.id }), meta: { title: 'page.account', access: 'auth' }
  },
  {
    path: '/myaccount', component: UserPage, props: r => (
      { userId: null }), meta: { title: 'page.myAccount', access: 'auth' }
  },
  {
    path: '/delivery/:id', component: DeliveryPage, props: r => (
      { deliveryId: r.params.id }), meta: { title: 'page.delivery', access: 'auth' }
  },
  {
    path: '/join/:groupId/:token', component: JoinGroupPage, props: r => (
      { groupId: r.params.groupId, token: r.params.token }), meta: { title: 'page.join', access: 'auth' }
  },
  { path: '/noAccess', component: NoAccessPage, meta: { title: 'page.403' } },
  { path: '*', component: NotFoundPage, meta: { title: 'page.404' } },
]
const router = new VueRouter({ routes, mode: 'history' });
router.beforeEach((to, from, next) => {
  if (to.meta.access === 'admin'){
    if (!api.isAdmin()) {
      next({path: '/noAccess'});
      return;
    }
  } else if (to.meta.access === 'auth'){
    if (!api.isAuthorized()){
      next({path: '/noAccess'});
      return;
    }
  }
  let language = localStorage.getItem('language') || i18n.fallbackLocale;
  document.title = i18n.t(to.meta.title || 'page.fallback');
  loadLanguageAsync(language).then(() => next());
});

export default router;