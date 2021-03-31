<template>
  <modal-window
    ref="modalWindow"
    v-bind:header="header"
    v-bind:close-text="closeText"
    v-bind:ok-text="okText"
    v-on:ok="okPressed"
  >
    <slot></slot>
    <search-panel
      v-bind:dataProvider="searchUsers"
      v-bind:mainTextProvider="emailSelector"
      v-bind:secondaryTextProvider="secondaryTextSelector"
      v-on:found-item="onUserFound"
    />
  </modal-window>
</template>

<script>
import api from "~/util/api";
import ModalWindow from "~/components/controls/ModalWindow";
import SearchPanel from "~/components/search/SearchPanel";

export default {
  name: "UserSearchModal",
  components: {
    ModalWindow,
    SearchPanel,
  },
  props: {
    header: String,
    closeText: String,
    okText: String,
  },
  data() {
    return {
      selectedUser: null,
    };
  },
  methods: {
    show() {
      this.$refs.modalWindow.show();
    },
    hide() {
      this.$refs.modalWindow.hide();
    },
    async okPressed() {
      this.$emit("ok", this.selectedUser);
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
      this.$emit('selected-user', user);
    },
  },
};
</script>

<style scoped>
</style>