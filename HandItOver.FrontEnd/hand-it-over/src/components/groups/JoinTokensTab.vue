<template>
  <div>
    <q-r-code-modal ref="qrcodeModal" v-bind:link="selectedTokenValue" />

    <button v-on:click="onGenerateLinkPressed" class="btn btn-outline-success">
      <i class="fas fa-plus"></i>
      Generate new join link
      </button>
    <table class="table">
      <tr>
        <th>#</th>
        <th>Join link</th>
        <th>QR code</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(token, i) in tokens" :key="token.id">
        <td>{{ i + 1 }}</td>
        <td>
          <a v-bind:href="createLinkFromToken(token.token)">Click to follow</a>
        </td>
        <td>
          <span v-on:click="onGenerateQRCodePressed(token.token)" class="link">
            <i class="fas fa-qrcode"></i>
            QR code
          </span>
        </td>
        <td>
          <span v-on:click="onDeleteTokenPressed(token)" class="delete">Delete</span>
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
    async onDeleteTokenPressed(t) {
      await api.sendDelete(
        `/mailboxGroup/${this.groupId}/whitelist/token/${t.id}`
      );
      let index = this.tokens.indexOf(t);
      this.tokens.splice(index, 1);
    },
  },
};
</script>

<style scoped>
table {
  margin-top: 20px;
}

.link {
  color: rgb(36, 22, 233);
  cursor: pointer;
}
.link:hover {
  text-decoration: underline;
}

.delete {
  color: rgb(206, 12, 12);
  cursor: pointer;
}
.delete:hover {
  text-decoration: underline;
}
</style>