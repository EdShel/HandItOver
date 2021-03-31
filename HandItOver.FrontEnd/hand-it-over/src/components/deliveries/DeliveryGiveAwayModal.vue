<template>
  <modal-window
    ref="modalWindow"
    header="Give away delivery"
    close-text="Cancel"
    ok-text="Give away"
    v-on:ok="giveAwayPressed"
  >
    <div>
      <div v-if="selectedUser">
        {{ selectedUser.email }} - {{ selectedUser.fullName }}
      </div>
      <div v-else>
        Type in new addressee's email or full name and select from the list
        below.
      </div>
      <search-panel
        v-bind:dataProvider="searchUsers"
        v-bind:mainTextProvider="emailSelector"
        v-bind:secondaryTextProvider="secondaryTextSelector"
        v-on:found-item="onUserFound"
      />
    </div>
  </modal-window>
</template>

<script>
import api from "~/util/api";
import ModalWindow from "~/components/controls/ModalWindow";
import SearchPanel from "~/components/search/SearchPanel";

export default {
  name: "DeliveryGiveAwayModal",
  components: {
    ModalWindow,
    SearchPanel,
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
    show() {
      this.$refs.modalWindow.openModal();
    },
    hide() {
      this.$refs.modalWindow.closeModal();
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
    searchUsers(searchQuery) {
      return api
        .sendGet("/user/paginated", {
          pageIndex: 0,
          pageSize: 5,
          search: searchQuery,
        })
        .then((r) => r.data.users);
    },
    emailSelector(user) {
      return user.email;
    },
    secondaryTextSelector(user) {
      return {
        fullName: user.fullName,
      };
    },
    onUserFound(user) {
      this.selectedUser = user;
    },
  },
};
</script>

<style>
</style>