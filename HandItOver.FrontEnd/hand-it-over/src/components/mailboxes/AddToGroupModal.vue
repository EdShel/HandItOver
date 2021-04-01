<template>
  <modal-window
    ref="modalWindow"
    header="Add to group"
    close-text="Cancel"
    ok-text="Add"
    v-on:ok="onAddPressed"
  >
    <div v-if="allGroups.length > 0">
      <label for="groupSelect">Choose your group</label>
      <select id="groupSelect" v-model="selectedGroup">
        <option
          v-for="group in allGroups"
          :key="group.groupId"
          v-bind:value="group"
        >
          {{ group.name }}
        </option>
      </select>
    </div>
    <div v-else>Create at least one group.</div>
  </modal-window>
</template>

<script>
import api from "~/util/api";
import ModalWindow from "~/components/controls/ModalWindow";

export default {
  name: "AddToGroupModal",
  components: { ModalWindow },
  props: {
    allGroups: Array,
    mailbox: Object,
  },
  data() {
    return {
      selectedGroup: null,
    };
  },
  methods: {
    async onAddPressed() {
      if (this.allGroups.length == 0) {
        return;
      }
      let r = await api.sendPost(
        `/mailboxGroup/${this.selectedGroup.groupId}/mailboxes`,
        null,
        {
          mailboxId: this.mailbox.id,
        }
      );
      this.hide();
      this.$emit("added-to-group", {
        mailbox: this.mailbox,
        group: this.selectedGroup,
      });
    },
    show() {
      if (this.allGroups.length > 0) {
        this.selectedGroup = this.allGroups[0];
      }
      this.$refs.modalWindow.show();
    },
    hide() {
      this.$refs.modalWindow.hide();
    },
  },
};
</script>
