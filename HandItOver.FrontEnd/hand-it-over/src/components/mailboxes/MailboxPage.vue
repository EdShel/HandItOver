<template>
  <div>
    <mailbox-page-group-modal
      ref="groupCreateModal"
      v-bind:firstMailboxId="selectedMailboxesIds[0]"
      v-on:created-group="createdGroupForMailbox"
    />

    <h3>Mailboxes</h3>
    <mailbox-page-group-item
      v-for="(mailboxes, groupId) in mailboxGroups"
      :key="groupId"
      v-bind:mailboxGroupId="groupId"
    >
      <mailbox-page-item
        v-for="box in mailboxes"
        :key="box.id"
        v-bind:mailbox="box"
        v-on:remove-from-group="removeMailboxFromGroup(box.id, groupId)"
      />
    </mailbox-page-group-item>

    <mailbox-page-item
      v-for="box in mailboxesWithoutGroups"
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
import MailboxPageGroupItem from "./MailboxPageGroupItem.vue";

export default {
  name: "MailboxPage",
  components: {
    MailboxPageItem,
    MailboxPageGroupModal,
    MailboxPageGroupItem,
  },
  data: function () {
    return {
      mailboxGroups: {},
      mailboxesWithoutGroups: [],
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
          let groupedByMailboxGroup = groupBy(data, "groupId");
          this.mailboxesWithoutGroups = groupedByMailboxGroup[null];
          delete groupedByMailboxGroup[null];
          this.mailboxGroups = groupedByMailboxGroup;
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
    removeMailboxFromGroup(mailboxId, groupId) {
      // TODO: check if have rents etc
      api
        .sendDelete(`/mailboxGroup/${groupId}/mailboxes/${mailboxId}`)
        .then((r) => {
          this.updateMailboxesList();
        })
        .catch((e) => {});
    },
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