<template>
  <modal-window
    ref="modalWindow"
    v-bind:header="$t('mailboxes.editMailbox')"
    v-bind:close-text="$t('common.closeAction')"
    v-bind:ok-text="$t('groups.edit')"
    v-on:ok="editPressed"
  >
    <div>
      <label for="addressText">{{$t('delivery.mailboxAddress')}}</label>
      <input id="addressText" type="text" v-model="edit.address" autocomplete="off" />
    </div>
  </modal-window>
</template>

<script>
import api from "~/util/api";
import ModalWindow from "~/components/controls/ModalWindow";

export default {
  name: "MailboxEditModal",
  components: { ModalWindow },
  data: function () {
    return {
      edit: {
        address: "",
      },
    };
  },
  mounted() {},
  methods: {
    editPressed() {
      api
        .sendPatch(`/mailbox/${this.edit.id}`, null, {
          address: this.edit.address,
        })
        .then((r) => {
          this.edit.address = r.data.address;

          this.$emit("edited-mailbox", r.data);
          this.hide();
        })
        .catch((e) => {});
    },
    show(mailbox) {
      this.edit = {...mailbox};
      this.$refs.modalWindow.show();
    },
    hide() {
      this.$refs.modalWindow.hide();
    },
  },
};
</script>

<style scoped>

label {
    font-size: 1.2em;
}

label, input {
    display: block;
    width: 100%;
}

</style>