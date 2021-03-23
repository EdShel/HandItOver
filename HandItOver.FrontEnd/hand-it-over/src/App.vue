<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png" />
    <router-link to="/">{{ userName }}</router-link>
    <router-link to="/admin">Haha lol</router-link>
    <router-view></router-view>
    <button @click="authorize">Authoruze</button>
    <button @click="logout">Deauthourzr</button>
  </div>
</template>

<script>
import api from "./util/api.js";
export default {
  name: "App",
  data: function () {
    return {
      userName: null,
    };
  },
  mounted() {
    this.getAuthInfo();
  },
  methods: {
    getAuthInfo() {
      let auth = api.getAuth();
      if (auth){
        this.userName = auth.email;
      }
      else{
        this.userName = "Unauthorized";
      }
    },
    authorize() {
      api
        .login("eduard.sheliemietiev@nure.ua", "qwerty")
        .then(() => {
          console.log(api.getAuth());
          this.getAuthInfo();
        })
        .catch(() => {
          alert("cannot login");
        });
    },
    logout() {
      api.logout().then(() => {
        this.getAuthInfo();
      })
    }
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
