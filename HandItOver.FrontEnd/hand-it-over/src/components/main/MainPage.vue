<template>
  <div>
    <rent-search></rent-search>
  </div>
</template>

<script>
import RentSearch from "~/components/main/RentSearch";

export default {
  name: "MainPage",
  components: { RentSearch },
  props: {
    msg: String,
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
