<template>
  <div class="groupItem">
    <p>
      {{$t('delivery.mailboxAddress')}}: <i>{{ mailbox.address }}</i>
    </p>
    <div v-if="!mailbox.groupId">
      <button @click="createGroup">
        <i class="fas fa-folder"></i> {{$t('mailboxes.addToNewGroup')}}
      </button>
      <button @click="onAddToGroupPressed">
        <i class="fas fa-folder-plus"></i> {{$t('mailboxes.addToExistingGroup')}}
      </button>
    </div>
    <button v-if="mailbox.groupId" @click="removeFromGroup">
      <i class="fas fa-folder-minus"></i> {{$t('mailboxes.removeFromGroupAction')}}
    </button>
    <button @click="editMailbox"><i class="fas fa-edit"></i> {{$t('groups.edit')}}</button>
    <button v-on:click="onShowDeliveriesPressed">
      <i
        class="fas"
        v-bind:class="deliveriesAreVisible ? 'fa-box-open' : 'fa-box'"
      ></i>
      {{$t('mailboxes.recentDeliveries')}}
    </button>
    <div v-if="deliveriesAreVisible" class="delivery-container">
      <div v-for="delivery in deliveries" :key="delivery.id" class="delivery">
        <div><b><i class="fas fa-calendar"></i> {{$t('delivery.arrived')}}: </b> {{ formatDate(delivery.arrived) }}</div>
        <div v-if="delivery.taken === null">
          <b><i class="fas fa-calendar-alt"></i> {{$t('mailboxes.predictedTaking')}}: </b>
          {{ formatDate(delivery.predictedTakingTime) }}
        </div>
        <div v-else>
          <b><i class="fas fa-calendar-times"></i> {{$t('delivery.taken')}}: </b>
          {{ formatDate(delivery.taken) }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from "~/util/date";

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
      this.$emit("add-to-group");
    },
    formatDate(date) {
      return dateUtil.localString(new Date(date));
    },
  },
};
</script>

<style scoped>
.groupItem {
  background: rgb(193 202 230);
  margin: 10px 0;
  padding: 15px;
  border-radius: 10px;
}

.groupItem > * {
  display: inline-block;
}

button {
  border: none;
  outline: none;
  border-radius: 15px;
  margin: 0 10px;
  padding: 0 10px;
}
p {
  margin: 0;
}

.delivery-container {
  width: 100%;
}

.delivery {
  background: #fff;
  border-radius: 14px;
  padding: 0 10px;
  margin: 10px 0 0 10px;
  width: 100%;
}
</style>