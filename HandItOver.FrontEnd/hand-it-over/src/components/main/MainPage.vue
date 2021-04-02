<template>
  <div>
    <h1>Hand It Over</h1>
    <p>
      {{ $t('main.descr') }}
    </p>
    <rent-search></rent-search>
    <box-banner />
  </div>
</template>

<script>
import RentSearch from "~/components/main/RentSearch";
import BoxBanner from "~/components/main/BoxBanner";

export default {
  name: "MainPage",
  components: { RentSearch, BoxBanner },
};
</script>

<style scoped>
h1 {
  font-size: 3em;
  text-transform: uppercase;
  text-align: center;
  margin-top: 20px;
  font-family: Verdana, Geneva, Tahoma, sans-serif;
  border-bottom: 1px solid black;
}

p {
  font-size: 1.2em;
  margin: 0 50px;
  text-align: center;
}
</style>