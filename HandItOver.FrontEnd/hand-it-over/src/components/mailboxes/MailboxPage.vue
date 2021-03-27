<template>
  <div>
    <mailbox-page-group-modal
      ref="groupCreateModal"
      v-bind:firstMailboxId="selectedMailboxesIds[0]"
      v-on:created-group="createdGroupForMailbox"
    />

    <h3>Mailboxes</h3>
    <mailbox-page-item
      v-for="box in mailboxes"
      :key="box.id"
      v-bind:mailbox="box"
      v-on:create-group="creatingGroupForMailbox(box.id)"
    />
  </div>
</template>

<script>
import api from "~/util/api";
import MailboxPageItem from "~/components/mailboxes/MailboxPageItem";
import MailboxPageGroupModal from "~/components/mailboxes/MailboxPageGroupModal";

export default {
  name: "MailboxPage",
  components: { MailboxPageItem, MailboxPageGroupModal },
  data: function () {
    return {
      mailboxes: [],
      selectedMailboxesIds: [],
    };
  },
  mounted() {
    this.updateMailboxesList();
  },
  methods: {
    updateMailboxesList() {
      api
        .sendGet("/mailbox/my")
        .then((r) => {
          let data = r.data;
          this.mailboxes = data;
        })
        .catch((e) => {});
    },
    creatingGroupForMailbox(mailboxId) {
      this.selectedMailboxesIds = [mailboxId];
      this.$refs.groupCreateModal.show();
    },
    createdGroupForMailbox() {
      this.updateMailboxesList();
    },
  },
};
</script>

<style scoped>
</style>