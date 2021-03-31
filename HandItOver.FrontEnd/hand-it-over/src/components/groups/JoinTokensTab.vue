<template>
  <div>
    <q-r-code-modal ref="qrcodeModal" v-bind:link="selectedTokenValue" />

    <button v-on:click="onGenerateLinkPressed">Generate new join link</button>
    <table class="table">
      <tr>
        <th>#</th>
        <th>Join link</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(token, i) in tokens" :key="token.id">
        <td>{{ i + 1 }}</td>
        <td>{{ createLinkFromToken(token.token) }}</td>
        <td>
          <button v-on:click="onGenerateQRCodePressed(token.token)">
            QR code
          </button>
        </td>
      </tr>
    </table>
  </div>
</template>

<script>
import api from "~/util/api";
import QRCodeModal from "~/components/groups/QRCodeModal";

export default {
  name: "JoinTokensTab",
  components: {
    QRCodeModal,
  },
  props: {
    groupId: String,
  },
  data() {
    return {
      tokens: [],
      selectedTokenValue: null,
    };
  },
  async mounted() {
    let r = await api.sendGet(`/mailboxGroup/${this.groupId}/whitelist/token`);
    this.tokens = r.data;
  },
  methods: {
    onGenerateLinkPressed() {
      api
        .sendPost(`/mailboxGroup/${this.groupId}/whitelist/token`)
        .then((r) => {
          this.tokens.unshift(r.data);
        })
        .catch((e) => {});
    },
    createLinkFromToken(token) {
      let origin = location.origin;
      let joinPath = `join/${this.groupId}/${token}`;
      return origin + "/" + joinPath;
    },
    onGenerateQRCodePressed(token) {
      this.selectedTokenValue = token;
      this.$refs.qrcodeModal.show();
    },
  },
};
</script>

<style>
</style>