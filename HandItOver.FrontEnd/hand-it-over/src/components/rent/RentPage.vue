<template>
  <div v-if="rent">
    <p v-if="rent.mailbox">
      <b>Mailbox: </b><span>{{ rent.mailbox.address }}</span>
    </p>
    <p v-if="rent.renter">
      <b>Renter: </b>
      <router-link v-bind:to="`/account/${rent.renter.id}`">
        {{ rent.renter.fullName }} ({{ rent.renter.email }})
      </router-link>
    </p>
    <p>
      <b>Start time: </b><span>{{ formatDate(rent.from) }}</span>
    </p>
    <p>
      <b>End time: </b><span>{{ formatDate(rent.until) }}</span>
    </p>
    <div>
      <button v-on:click="onDeleteRentPressed">Delete rent</button>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";

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
  methods: {
    async onDeleteRentPressed() {
      await api.sendDelete(`/mailboxGroup/rent/${this.rent.rentId}`);
      this.$router.back();
    },
    formatDate(dateStr) {
      return dateUtil.localString(new Date(dateStr));
    },
  },
};
</script>

<style>
</style>