<template>
  <div id="app">
    <app-header></app-header>

    <div class="container">
      <router-view></router-view>
    </div>
    <footer>
      <div>&#169;Eduard Sheliemietiev - 2021</div>
    </footer>
  </div>
</template>

<script>
import AppHeader from "~/components/common/AppHeader";
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
#app {
    background-color: rgb(223, 221, 221);
    min-height: 100vh;
    display: flex;
    flex-direction: column;
}

.container {
    margin: 30px auto;
    background: #fff;
    flex-grow: 1;
}

footer {
    margin-top: auto;

  text-align: center;
  background: #fff;
}

footer > div {
  box-shadow: #0000001c 0 -10px 10px;
  padding: 20px;
}
</style>
