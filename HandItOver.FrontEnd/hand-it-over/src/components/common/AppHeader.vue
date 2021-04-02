<template>
  <div id="navSection" v-cloak>
    <login-modal ref="loginModal"></login-modal>
    <register-modal ref="registerModal"></register-modal>

    <nav class="navbar navbar-expand-lg">
      <router-link to="/" class="navbar-brand router-link">Hand It Over</router-link>
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
import LoginModal from "~/components/common/LoginModal";
import RegisterModal from "~/components/common/RegisterModal";
import LangSelector from "~/components/common/LangSelector";
import api from "../../util/api.js";

export default {
  name: "AppHeader",

  components: {
    LoginModal,
    RegisterModal,
    LangSelector,
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

<style scoped>
#navSection {
  background: #fff;
  box-shadow: #0000001c 0 10px 10px;
  color: black;
  z-index: 9999;
}
.navbar {
  padding: 0 10px 10px 10px;
}

a, span{
  color: black;
}

.navbar-toggler-icon {
  background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' width='30' height='30' viewBox='0 0 30 30'%3e%3cpath stroke='rgba%280, 0, 0, 1.0%29' stroke-linecap='round' stroke-miterlimit='10' stroke-width='4' d='M4 7h22M4 15h22M4 23h22'/%3e%3c/svg%3e");
  box-sizing: content-box;
  border: 4px solid black;
}

.nav-link {
  font-size: 1.2em;
  margin-top: 8px;
  padding: 0;
}

.navbar-expand-lg a {
  text-align: center;
}

.navbar-brand {
  font-size: 2em;
  font-weight: bold;
  padding: 0;
  border-bottom: 5px solid transparent;
}

.navbar-brand.router-link-exact-active {
  border-bottom: 5px solid #000;
} 

.nav-link:not(.router-link-exact-active) {
  border-bottom: 5px solid transparent;
}

.nav-link:hover {
  border-bottom: 5px solid #000;
}

.router-link-exact-active {
  border-bottom: 5px solid #000;
}
</style>