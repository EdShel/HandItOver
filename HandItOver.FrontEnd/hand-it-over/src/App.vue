<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png" />
    <router-link to="/">{{ currentDate }}</router-link>
    <router-link to="/admin">Haha lol</router-link>
    <router-view></router-view>
  </div>
</template>

<script>
import api from "./util/api.js";
export default {
  name: "App",
  data: function () {
    return {
      currentDate: null
    };
  },
  mounted() {
    api.sendGet('/healthCheck')
    .then(r => {
      let data = r.data;
      this.currentDate = data.date;
    })
  }
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
