<template>
  <div>
    <div v-if="delivery">
      <delivery-give-away-modal
        ref="giveAwayModal"
        v-bind:deliveryId="deliveryId"
        v-on:given-away="onGivenAway"
      />

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
    <div>
      <button v-on:click="giveAwayPressed">Give away</button>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";
import DeliveryGiveAwayModal from "~/components/deliveries/DeliveryGiveAwayModal";

export default {
  name: "DeliveryPage",
  components: {
    DeliveryGiveAwayModal,
  },
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
    let mbResult = await api.sendGet(`/mailbox/${dResult.data.mailboxId}`);
    this.mailbox = mbResult.data;
    let aResult = await api.sendGet(`/user/byId/${mbResult.data.ownerId}`);
    this.addressee = aResult.data;
  },
  methods: {
    formatDateString(date) {
      return dateUtil.localString(new Date(date));
    },
    formatDate(date) {
      return dateUtil.localString(date);
    },
    giveAwayPressed(){
        this.$refs.giveAwayModal.show();
    },
    onGivenAway(newAddressee) {
      this.addressee = newAddressee;
    },
  },
};
</script>

<style>
</style>