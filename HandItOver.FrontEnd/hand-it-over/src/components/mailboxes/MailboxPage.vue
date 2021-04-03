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

    <add-to-group-modal
      ref="addToGroupModal"
      v-bind:all-groups="allGroups"
      v-bind:mailbox="editedMailbox"
      v-on:added-to-group="onAddedToGroup"
    />

    <h3>{{$t('page.mailbox')}}</h3>
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
      v-on:add-to-group="onAddToGroupRequested(box)"
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
import AddToGroupModal from "~/components/mailboxes/AddToGroupModal";

export default {
  name: "MailboxPage",
  components: {
    MailboxPageItem,
    MailboxPageGroupModal,
    MailboxPageGroupItem,
    MailboxEditModal,
    AddToGroupModal,
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
      this.allGroups = rGroup.data.map((group) => {
        group.mailboxes = group.mailboxes.map((mb) => {
          mb.groupId = group;
          return mb;
        });
        return group;
      });

      let rMailboxes = await api.sendGet("/mailbox/my");
      this.mailboxesWithoutGroups = rMailboxes.data.filter(
        (mb) => mb.groupId == null
      );
    },
    creatingGroupForMailbox(mailboxId) {
      this.selectedMailboxesIds = [mailboxId];
      this.$refs.groupCreateModal.show();
    },
    createdGroupForMailbox() {
      this.updateMailboxesList();
    },
    removeMailboxFromGroup(mailboxId, groupId) {
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
    onAddToGroupRequested(box) {
      this.editedMailbox = box;
      this.$refs.addToGroupModal.show();
    },
    onAddedToGroup(e) {
      this.updateMailboxesList();
    },
  },
};

</script>

<style scoped>
</style>