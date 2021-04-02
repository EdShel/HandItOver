<template>
  <user-search-modal
    ref="modalWindow"
    v-bind:header="$t('groups.addToWhitelistHeader')"
    v-bind:close-text="$t('common.cancelAction')"
    v-bind:ok-text="$t('common.addAction')"
    v-on:ok="whitelistPressed"
    v-on:selected-user="onUserSelected"
  >
  <span>Find a user to add him to the whitelist.</span>
  </user-search-modal>
</template>

<script>
import api from "~/util/api";
import UserSearchModal from "~/components/search/UserSearchModal";

export default {
  name: "AddToWhitelistModal",
  components: {
    UserSearchModal,
  },
  props: {
    groupId: String,
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
    async whitelistPressed() {
      let r = await api.sendPost(
        `/mailboxGroup/${this.groupId}/whitelist`,
        null,
        {
          userEmail: this.selectedUser.email,
        }
      );
      this.hide();
      this.$emit("added-to-whitelist", this.selectedUser);
      this.selectedUser = null;
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