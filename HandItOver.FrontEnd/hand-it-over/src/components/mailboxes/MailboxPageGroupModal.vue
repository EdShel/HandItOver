<template>
  <modal-window
    ref="modalWindow"
    header="Create group"
    close-text="Close"
    ok-text="Create"
    v-on:ok="createPressed"
  >
    <div>
      <label for="groupNameText">{{$t('groups.groupName')}} ({{$t('mailboxes.unique')}})</label>
      <input id="groupNameText" type="text" v-model="groupName" />
    </div>
    <div>
      <label for="whitelistOnlyCheckbox">{{$t('groups.whitelistOnly')}}</label>
      <input
        id="whitelistOnlyCheckbox"
        type="checkbox"
        v-model="whitelistOnly"
      />
    </div>
    <div>
      <label for="maxRentHours">{{$t('groups.maxRentHours')}}</label>
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
import date from '~/util/date'
import ModalWindow from "~/components/controls/ModalWindow";
export default {
  name: "MailboPageGroupModal",
  props: {
    firstMailboxId: String,
  },
  components: { ModalWindow },
  data: function () {
    return {
      groupName: this.$t('mailboxes.exampleGroupName'),
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
          maxRentTime: date.hoursToHMmSs(this.maxRentHours),
        })
        .then((r) => {
          this.$emit("created-group");
        })
        .catch((e) => {
        });
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
label {
    font-size: 1.2em;
}

label, input {
    display: block;
    width: 100%;
}
<style>
