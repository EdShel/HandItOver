<template>
  <div id="app">
    <app-header></app-header>

    <div class="container">
      <router-view></router-view>
    </div>
    <footer>Eduard Sheliemietiev&#169; - 2021</footer>
  </div>
</template>

<script>
import AppHeader from '~/components/AppHeader'
import api from "~/util/api";

export default {
  name: "App",
  components: { AppHeader },
  data: function () {
    return {
      userName: null,
    };
  },
  mounted() {},
};
</script>

<style scoped>

</style>>
