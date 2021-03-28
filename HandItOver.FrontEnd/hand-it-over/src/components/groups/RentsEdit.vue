<template>
  <div>
    <div v-for="rent in rents" :key="rent.rentId">
        {{ rent.from }} - {{ rent.to }} of {{ rent.mailboxId }}
    </div>
  </div>
</template>

<script>
import api from "~/util/api";

export default {
  name: "RentsEdit",
  props: {
    groupId: String,
  },
  data() {
    return {
      rents: [],
    };
  },
  mounted() {
    this.updateRents();
  },
  methods: {
    updateRents() {
      api
        .sendGet(`/mailboxGroup/${this.groupId}/rent`)
        .then((r) => {
          let data = r.data;
          this.rents = data;
        })
        .catch((e) => {});
    },
  },
};
</script>

<style>
</style>