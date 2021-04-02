<template>
  <div>
    <h3>{{$t('rent.rents')}}</h3>
    <table class="table">
      <tr>
        <th>#</th>
        <th>{{$t('rent.from')}}</th>
        <th>{{$t('rent.until')}}</th>
        <th>{{$t('mailboxes.mailboxSize')}}</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(rent, i) in rents" :key="rent.id">
        <td>{{ i + 1 }}</td>
        <td>{{ formatDate(rent.from) }}</td>
        <td>{{ formatDate(rent.until) }}</td>
        <td>{{ rent.mailboxSize }}</td>
        <td><router-link v-bind:to="'/rent/' + rent.id">{{$t('rent.viewRentAction')}}</router-link></td>
      </tr>
    </table>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";

export default {
  name: "UserRents",
  components: {},
  props: {
    userId: String,
  },
  data() {
    return {
      rents: [],
    };
  },
  mounted() {
    api
      .sendGet(`/mailboxGroup/rent/user/${this.userId}`)
      .then((r) => {
        this.rents = r.data.map((rent) => ({
          id: rent.rentId,
          mailboxId: rent.mailboxId,
          mailboxSize: rent.mailbox.size,
          from: new Date(rent.from),
          until: new Date(rent.until),
        }));
      })
      .catch((e) => {});
  },
  methods: {
    formatDate(date) {
      return dateUtil.localString(date);
    },
  },
};
</script>

<style scoped>
td, th {
  text-align: center;
}
</style>