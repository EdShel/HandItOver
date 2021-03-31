<template>
  <div>
    <div v-if="delivery">
      <span> Weight: {{ delivery.weight }} </span>
      <span> Arrived: {{ formatDateString(delivery.arrived) }} </span>
      <span>
        TerminalTime:
        {{
          delivery.terminalTime
            ? formatDateString(delivery.terminalTime)
            : "Not limited"
        }}
      </span>
      <span>
        Taken:
        {{
          delivery.terminalTime
            ? formatDateString(delivery.terminalTime)
            : "Not taken"
        }}
      </span>
      <span>
        Predicted to be taken:
        {{ formatDateString(delivery.predictedTakingTime) }}
      </span>
    </div>
    <div v-if="mailbox">
      <h3>Mailbox</h3>
      <p>
        <span> Size: {{ mailbox.size }} </span>
        <span> Address: {{ mailbox.address }} </span>
      </p>
    </div>
    <div v-if="addressee">
      <h3>Addressee</h3>
      <p>
        <span> Email: {{ addressee.email }} </span>
        <span> Full name: {{ addressee.fullName }} </span>
      </p>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";

export default {
  name: "DeliveryPage",
  props: {
    deliveryId: String,
  },
  data() {
    return {
      delivery: {},
      mailbox: {},
      addressee: {},
    };
  },
  async mounted() {
    let dResult = await api.sendGet(`/delivery/${this.deliveryId}`);
    this.delivery = dResult.data;
    let mbResult = await api.sendGet(`/mailbox/${dResult.mailboxId}`);
    this.mailbox = mbResult.data;
    let aResult = await api.sendGet(`/user/byId/${dResult.owner}`);
    this.addressee = aResult;
  },
  methods: {
    formatDateString(date) {
      return dateUtil.localString(new Date(date));
    },
    formatDate(date) {
      return dateUtil.localString(date);
    },
  },
};
</script>

<style>
</style>