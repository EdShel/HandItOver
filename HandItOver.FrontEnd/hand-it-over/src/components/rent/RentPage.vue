<template>
  <div v-if="rent">{{ rent.rentId }} - {{ rent.from }} - {{ rent.until }}</div>
</template>

<script>
import api from "~/util/api";

export default {
  name: "RentPage",
  props: {
    rentId: String,
  },
  data() {
    return {
      rent: {},
    };
  },
  mounted() {
    api
      .sendGet(`/mailboxGroup/rent/${this.rentId}`)
      .then((r) => {
        this.rent = r.data;
      })
      .catch((e) => {});
  },
};
</script>

<style>
</style>