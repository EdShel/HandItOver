<template>
  <modal-window
    ref="modalWindow"
    header="Create group"
    close-text="Close"
    ok-text="Create"
    v-on:ok="createPressed"
  >
    <div>
      <label for="groupNameText">Group name (unique)</label>
      <input id="groupNameText" type="text" v-model="groupName" />
    </div>
    <div>
      <label for="whitelistOnlyCheckbox">Only whitelisted users can rent</label>
      <input
        id="whitelistOnlyCheckbox"
        type="checkbox"
        v-model="whitelistOnly"
      />
    </div>
    <div>
      <label for="maxRentHours">Max rent hours</label>
      <input
        id="maxRentHours"
        type="range"
        min="1"
        max="72"
        step="1"
        v-model="maxRentHours"
      />
    </div>
  </modal-window>
</template>

<script>
import api from "~/util/api";
import ModalWindow from "~/components/ModalWindow";
export default {
  name: "MailboPageGroupModal",
  props: {
    firstMailboxId: String,
  },
  components: { ModalWindow },
  data: function () {
    return {
      groupName: "E.g. Kharkiv ATB Market #2",
      whitelistOnly: false,
      maxRentHours: 8,
    };
  },
  methods: {
    createPressed() {
      api
        .sendPost("/mailboxGroup", null, {
          name: this.groupName,
          firstMailboxId: this.firstMailboxId,
          whitelistOnly: this.whitelistOnly,
          maxRentTime: hoursToDdHhMmSs(this.maxRentHours),
        })
        .then((r) => {
          this.$emit("created-group");
        })
        .catch((e) => {
          console.log("Can't create the group");
        });
    },
    show() {
      this.$refs.modalWindow.openModal();
    },
    hide() {
      this.$refs.modalWindow.closeModal();
    },
  },
};

function hoursToDdHhMmSs(totalHours) {
  let days = Math.floor(totalHours / 24);
  let hours = totalHours - days * 24;

  let daysStr = days.toString().padStart(2, "0");
  let hoursStr = hours.toString().padStart(2, "0");
  return `${daysStr}:${hoursStr}:00:00`;
}
</script>
label {
    font-size: 1.2em;
}

label, input {
    display: block;
    width: 100%;
}
<style>
