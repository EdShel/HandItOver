<template>
  <div>
    <rent-search></rent-search>
    {{ $t('duck.ducklin', {name: 'Bob'}) }}
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

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>