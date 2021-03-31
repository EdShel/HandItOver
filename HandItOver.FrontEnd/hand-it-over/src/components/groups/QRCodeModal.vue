<template>
  <modal-window
    ref="modalWindow"
    header="QR code"
    close-text="Close"
    ok-text="Download PNG"
    v-on:ok="onOkPressed"
  >
    <vue-qrcode id="qrCodeImage" v-bind:value="link" />
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

<style>
</style>