<template>
  <modal-window
    ref="modalWindow"
    v-bind:header="$t('groups.qrCode')"
    v-bind:close-text="$t('common.closeAction')"
    v-bind:ok-text="$t('groups.downloadPng')"
    v-on:ok="onOkPressed"
  >
    <vue-qrcode id="qrCodeImage" v-bind:value="link" class="qrCode" />
  </modal-window>
</template>

<script>
import ModalWindow from "~/components/controls/ModalWindow";
import VueQrcode from "vue-qrcode";

export default {
  name: "QRCodeModal",
  components: {
    ModalWindow,
    VueQrcode,
  },
  props: {
    link: String,
  },
  methods: {
    onOkPressed() {
      this.downloadImage();
      this.hide();
    },
    downloadImage() {
      let link = document.createElement("a");
      link.setAttribute("download", "qr.png");
      link.href = document.getElementById("qrCodeImage").src;
      document.body.appendChild(link);
      link.click();
      link.remove();
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

<style scoped>
.qrCode {
  display: block;
  margin: 0 auto;
}
</style>