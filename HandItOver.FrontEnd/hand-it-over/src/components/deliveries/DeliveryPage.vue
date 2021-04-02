<template>
  <div v-if="delivery">
    <div>
      <delivery-give-away-modal
        ref="giveAwayModal"
        v-bind:deliveryId="deliveryId"
        v-on:given-away="onGivenAway"
      />

      <h3>Viewing delivery</h3>

      <p v-if="addressee">
        <b><i class="fas fa-user"></i> Addressee: </b>
        <router-link v-bind:to="`/account/${addressee.id}`">
          {{ addressee.fullName }} ({{ addressee.email }})
        </router-link>
      </p>
      <p v-if="mailbox">
        <b><i class="fas fa-map-marker-alt"></i> Mailbox address:</b>
        {{ mailbox.address }}
        <router-link v-bind:to="`/account/${mailbox.ownerId}`">
          (mailbox owner)
        </router-link>
      </p>
      <p>
        <b><i class="fas fa-weight-hanging"></i> Weight:</b>
        {{ toLocalMass(delivery.weight) }}
      </p>
      <p>
        <b><i class="fas fa-truck-loading"></i> Arrived: </b>
        {{ formatDateString(delivery.arrived) }}
      </p>
      <p>
        <b><i class="fas fa-calendar-times"></i> Terminal time: </b>
        {{
          delivery.terminalTime
            ? formatDateString(delivery.terminalTime)
            : "Not limited"
        }}
      </p>
      <p>
        <b><i class="fas fa-calendar"></i> Taken: </b>
        {{ delivery.taken ? formatDateString(delivery.taken) : "Not taken" }}
      </p>
      <p>
        <b><i class="fas fa-calendar-alt"></i> Predicted to be taken:</b>
        {{ formatDateString(delivery.predictedTakingTime) }}
      </p>
    </div>
    <div>
      <button v-on:click="giveAwayPressed" class="btn btn-warning">
        Give away
      </button>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";
import DeliveryGiveAwayModal from "~/components/deliveries/DeliveryGiveAwayModal";
import { localWeight } from "~/util/units";

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
    giveAwayPressed() {
      this.$refs.giveAwayModal.show();
    },
    onGivenAway(newAddressee) {
      this.addressee = newAddressee;
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