<template>
  <div v-if="rent">
    <h3>{{$t('rent.viewingRent')}}</h3>
    <p v-if="rent.mailbox">
      <b><i class="fas fa-box"></i> {{$t('mailboxes.mailbox')}}: </b><span>{{ rent.mailbox.address }}</span>
    </p>
    <p v-if="rent.renter">
      <b><i class="fas fa-user"></i> {{$t('rent.renter')}}: </b>
      <router-link v-bind:to="`/account/${rent.renter.id}`">
        {{ rent.renter.fullName }} ({{ rent.renter.email }})
      </router-link>
    </p>
    <p>
      <b><i class="fas fa-hourglass-start"></i> {{$t('rent.startTime')}}: </b><span>{{ formatDate(rent.from) }}</span>
    </p>
    <p>
      <b><i class="fas fa-hourglass-end"></i> {{$t('rent.endTime')}}: </b><span>{{ formatDate(rent.until) }}</span>
    </p>
    <div>
      <button v-on:click="onDeleteRentPressed" class="btn btn-danger"> 
        <i class="fas fa-times"></i> {{$t('rent.cancelRentAction')}}
        </button>
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

<style scoped>
h3 {
  font-size: 3em;
  margin-bottom: 30px;
}

p {
  font-size: 1.5em;
}

button {
  margin-top: 40px;
}
</style>