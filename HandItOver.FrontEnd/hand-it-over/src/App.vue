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
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.container {
  background: #fff;
  flex-grow: 1;
  padding-top: 20px;
  padding-bottom: 40px;
}

footer {
  margin-top: auto;

  text-align: center;
  background-image: url(./assets/cardboard.png);
  background-position: 0;
  background-size: 239px;
  font-size: 1.2em;
  font-weight: bold;
}

footer > div {
  box-shadow: #0000001c 0 -10px 10px;
  padding: 20px;
}
</style>

<style >
h1,
h3 {
  color: rgb(105 46 8);
}
</style>
