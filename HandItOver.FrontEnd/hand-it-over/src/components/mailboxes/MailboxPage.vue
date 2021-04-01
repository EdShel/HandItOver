<template>
  <div>
    <mailbox-page-group-modal
      ref="groupCreateModal"
      v-bind:firstMailboxId="selectedMailboxesIds[0]"
      v-on:created-group="createdGroupForMailbox"
    />

    <mailbox-edit-modal
      ref="editModal"
      v-bind:mailbox="editedMailbox"
      v-on:edited-mailbox="onMailboxEdited"
    />

    <h3>Mailboxes</h3>
    <mailbox-page-group-item
      v-for="group in allGroups"
      :key="group.groupId"
      v-bind:mailboxGroup="group"
    >
      <mailbox-page-item
        v-for="box in group.mailboxes"
        :key="box.id"
        v-bind:mailbox="box"
        v-on:remove-from-group="removeMailboxFromGroup(box.id, group.groupId)"
        v-on:edit-mailbox="onMailboxEditing(box)"
      />
    </mailbox-page-group-item>

    <mailbox-page-item
      v-for="box in mailboxesWithoutGroups"
      :key="box.id"
      v-bind:mailbox="box"
      v-on:create-group="creatingGroupForMailbox(box.id)"
      v-on:edit-mailbox="onMailboxEditing(box)"
    />
  </div>
</template>

<script>
import api from "~/util/api";
import MailboxPageItem from "~/components/mailboxes/MailboxPageItem";
import MailboxPageGroupModal from "~/components/mailboxes/MailboxPageGroupModal";
import MailboxEditModal from "~/components/mailboxes/MailboxEditModal";
import MailboxPageGroupItem from "~/components/mailboxes/MailboxPageGroupItem";

export default {
  name: "MailboxPage",
  components: {
    MailboxPageItem,
    MailboxPageGroupModal,
    MailboxPageGroupItem,
    MailboxEditModal,
  },
  data: function () {
    return {
      allGroups: [],
      mailboxesWithoutGroups: [],
      selectedMailboxesIds: [],
      editedMailbox: {},
    };
  },
  mounted() {
    this.updateMailboxesList();
  },
  methods: {
    async updateMailboxesList() {
      let rGroup = await api.sendGet(`/mailboxGroup/my`);
      this.allGroups = rGroup.data;

      let rMailboxes = await api.sendGet("/mailbox/my");
      this.mailboxesWithoutGroups = rMailboxes.data.filter(mb => mb.groupId == null);
    },
    creatingGroupForMailbox(mailboxId) {
      this.selectedMailboxesIds = [mailboxId];
      this.$refs.groupCreateModal.show();
    },
    createdGroupForMailbox() {
      this.updateMailboxesList();
    },
    removeMailboxFromGroup(mailboxId, groupId) {
      // TODO: check if have rents etc
      api
        .sendDelete(`/mailboxGroup/${groupId}/mailboxes/${mailboxId}`)
        .then((r) => {
          this.updateMailboxesList();
        })
        .catch((e) => {});
    },
    onMailboxEditing(mailbox) {
      this.editedMailbox = mailbox;
      this.$refs.editModal.show();
    },
    onMailboxEdited(mailbox) {},
  },
};

function groupBy(collection, key) {
  return collection.reduce(function (map, x) {
    (map[x[key]] = map[x[key]] || []).push(x);
    return map;
  }, {});
}
</script>

<style scoped>
</style>