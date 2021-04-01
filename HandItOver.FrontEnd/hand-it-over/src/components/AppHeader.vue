<template>
  <div id="navSection" v-cloak>
    <login-modal ref="loginModal"></login-modal>
    <register-modal ref="registerModal"></register-modal>

    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <router-link to="/" class="navbar-brand">Hand It Over</router-link>
      <button
        class="navbar-toggler"
        type="button"
        v-on:click="$refs.hiddenNav.classList.toggle('show')"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" ref="hiddenNav">
        <ul class="navbar-nav mr-auto">
          <template v-if="!isAuthorized">
            <li>
              <a class="nav-link" href="#" v-on:click.prevent="showLogin"
                >Login</a
              >
            </li>
            <li>
              <a class="nav-link" href="#" v-on:click.prevent="showRegister"
                >Register</a
              >
            </li>
          </template>
          <template v-else>
            <template v-if="isAdmin">
              <li>
                <router-link to="/admin" class="nav-link">
                  Admin panel
                </router-link>
              </li>
              <li>
                <router-link to="/users" class="nav-link">
                  System users
                </router-link>
              </li>
            </template>
            <li>
              <router-link to="/myaccount" class="nav-link">
                My account
              </router-link>
            </li>
            <li>
              <router-link to="/mailbox" class="nav-link"
                >My mailboxes
              </router-link>
            </li>
            <li>
              <a class="nav-link" href="#" v-on:click.prevent="logout"
                >Logout</a
              >
            </li>
          </template>
        </ul>
        <div class="form-inline my-2 my-lg-0">
          <lang-selector />
        </div>
      </div>
    </nav>
  </div>
</template>

<script>
import LoginModal from "~/components/LoginModal";
import RegisterModal from "~/components/RegisterModal";
import LangSelector from "~/components/common/LangSelector";
import api from "../util/api.js";

export default {
  name: "AppHeader",

  components: {
    LoginModal,
    RegisterModal,
    LangSelector
  },
  computed: {
    isAuthorized() {
      return api.isAuthorized();
    },
    isAdmin() {
      return api.isAdmin();
    },
  },
  methods: {
    showLogin() {
      this.$refs.loginModal.show();
    },
    showRegister() {
      this.$refs.registerModal.show();
    },
    logout() {
      api.logout().finally(function () {
        location.reload();
      });
    },
  },
};
</script>

<style>
</style>