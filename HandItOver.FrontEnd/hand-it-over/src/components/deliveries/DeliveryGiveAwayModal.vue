<template>
  <user-search-modal
    ref="modalWindow"
    header="Give away delivery"
    close-text="Cancel"
    ok-text="Give away"
    v-on:ok="giveAwayPressed"
    v-on:selected-user="onUserSelected"
  >
    <div v-if="selectedUser">
      {{ selectedUser.email }} - {{ selectedUser.fullName }}
    </div>
    <div v-else>
      Type in new addressee's email or full name and select from the list below.
    </div>
  </user-search-modal>
</template>

<script>
import api from "~/util/api";
import UserSearchModal from "~/components/search/UserSearchModal";

export default {
  name: "DeliveryGiveAwayModal",
  components: {
    UserSearchModal,
  },
  props: {
    deliveryId: String,
  },
  data() {
    return {
      selectedUser: null,
    };
  },
  methods: {
    onUserSelected(user) {
      this.selectedUser = user;
    },
    async giveAwayPressed() {
      let r = await api.sendPost(
        `/delivery/${this.deliveryId}/giveaway`,
        null,
        {
          newAddresseeId: this.selectedUser.id,
        }
      );
      this.hide();
      this.$emit("given-away", this.selectedUser);
    },
    show() {
      this.$refs.modalWindow.show();
    },
    hide() {
      this.$refs.modalWindow.hide();
    },
  },
};
</script>

<style>
</style>