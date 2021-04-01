<template>
  <div>
    <p>{{ mailbox.address }}</p>
    <div v-if="!mailbox.groupId">
      <button @click="createGroup">Add to new group</button>
      <button @click="onAddToGroupPressed">Add to existing group</button>
    </div>
    <button v-if="mailbox.groupId" @click="removeFromGroup">
      Remove from group
    </button>
    <button @click="editMailbox">Edit</button>
    <button v-on:click="onShowDeliveriesPressed">Show recent deliveries</button>
    <div v-if="deliveriesAreVisible">
      <div v-for="delivery in deliveries" :key="delivery.id">
        <span> Arrived: {{ delivery.arrived }} </span>
        <span v-if="delivery.taken === null">
          Predicted taking: {{ delivery.predictedTakingTime }}
        </span>
        <span v-else> Taken: {{ delivery.taken }} </span>
      </div>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";

export default {
  name: "MailboxPageItem",
  props: {
    mailbox: {},
  },
  data() {
    return {
      deliveries: [],
      deliveriesAreVisible: false,
    };
  },
  methods: {
    createGroup() {
      this.$emit("create-group");
    },
    removeFromGroup() {
      this.$emit("remove-from-group");
    },
    editMailbox() {
      this.$emit("edit-mailbox");
    },
    async onShowDeliveriesPressed() {
      if (this.deliveriesAreVisible) {
        this.deliveriesAreVisible = false;
        return;
      }
      let r = await api.sendGet(`/delivery/recent/${this.mailbox.id}`, {
        count: 5,
      });
      this.deliveries = r.data;
      this.deliveriesAreVisible = true;
    },
    onAddToGroupPressed() {
      this.$emit('add-to-group');
    }
  },
};
</script>

<style scoped>
div {
  border: 1px solid black;
}
</style>