<template>
  <div>
    <table class="table">
      <tr>
        <th>#</th>
        <th><i class="fas fa-hourglass-start"></i> {{$t('rent.from')}}</th>
        <th><i class="fas fa-hourglass-end"></i> {{$t('rent.until')}}</th>
        <th><i class="fas fa-user"></i> {{$t('rent.renter')}}</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(rent, i) in rents" :key="rent.rentId">
        <td>{{ i + 1 }}</td>
        <td>
           {{ formatDate(rent.from) }}
        </td>
        <td>
           {{ formatDate(rent.until) }}
        </td>
        <td>
          <router-link v-bind:to="`/account/${rent.renter.id}`">
            {{ rent.renter.fullName }}
          </router-link>
        </td>
        <td>
          <router-link v-bind:to="`/rent/${rent.rentId}`">
            {{$t('rent.viewRentAction')}}
          </router-link>
        </td>
      </tr>
    </table>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";

export default {
  name: "RentsEditTab",
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
    formatDate(dateStr) {
      return dateUtil.localString(new Date(dateStr));
    },
  },
};
</script>

<style scoped>
td, th {
  text-align: center;
}
</style>