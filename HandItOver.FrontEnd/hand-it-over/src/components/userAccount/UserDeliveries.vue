<template>
  <div>
    <h3>{{ $t("account.currentDeliveries") }}</h3>
    <table class="table table-hover">
      <tr>
        <th>#</th>
        <th>{{ $t("delivery.arrived") }}</th>
        <th>{{ $t("delivery.terminalTime") }}</th>
        <th>{{ $t("delivery.weight") }}</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(d, i) in deliveries" :key="d.id">
        <td>{{ i + 1 }}</td>
        <td>{{ formatDate(d.arrived) }}</td>
        <td>
          {{ d.terminal ? formatDate(d.terminal) : $t("delivery.notLimited") }}
        </td>
        <td>{{ toLocalMass(d.weight) }}</td>
        <td>
          <router-link v-bind:to="'/delivery/' + d.id">{{
            $t("account.viewDeliveryAction")
          }}</router-link>
        </td>
      </tr>
    </table>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";
import { localWeight } from "~/util/units";

export default {
  name: "UserDeliveries",
  components: {},
  props: {
    userId: String,
  },
  data() {
    return {
      deliveries: [],
    };
  },
  mounted() {
    api
      .sendGet(`/delivery/active/${this.userId}`)
      .then((r) => {
        this.deliveries = r.data.map((d) => ({
          id: d.id,
          weight: d.weight,
          mailboxId: d.mailboxId,
          arrived: new Date(d.arrived),
          terminal: d.terminalTime ? new Date(d.terminalTime) : null,
        }));
      })
      .catch((e) => {});
  },
  methods: {
    formatDate(date) {
      return dateUtil.localString(date);
    },
    toLocalMass(mass) {
      return this.$t("units.mass", [
        localWeight(this.$t("units.massUnit"), mass).toFixed(2),
      ]);
    },
  },
};
</script>

<style scoped>
td,
th {
  text-align: center;
}
</style>
