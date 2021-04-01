<template>
  <div>
    <rent-search></rent-search>
    {{ $t('duck.ducklin') }}
  </div>
</template>

<script>
import RentSearch from "~/components/main/RentSearch";

export default {
  name: "MainPage",
  components: { RentSearch },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>