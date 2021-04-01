<template>
  <div>
    <div>
      <label for="backupFileText">Backup file name (with extension)</label>
      <input id="backupFileText" type="text" v-model="backupFileName" />
    </div>
    <div>
      <button v-on:click="onMakeBackupPressed">Make backup</button>
      <button v-on:click="onDownloadBackupPressed">Download backup</button>
    </div>
    <div v-if="messageText">
      {{ messageText }}
    </div>
  </div>
</template>

<script>
import api from "~/util/api";

export default {
  name: "DbBackup",
  data() {
    return {
      backupFileName: "",
      messageDisappearTimeout: null,
      messageText: null,
    };
  },
  methods: {
    async onMakeBackupPressed() {
      await api.sendPost(`/admin/backup`, null, {
        backupFile: this.backupFileName,
      });
      this.showMessage('Back up is succesfully made.');
    },
    async onDownloadBackupPressed() {
        await api.downloadBlobFile('/admin/backup', {
            file: this.backupFileName
        }, this.backupFileName);
    },
    showMessage(text) {
      if (this.messageDisappearTimeout) {
        clearTimeout(this.messageDisappearTimeout);
      }

      const messageDurationMs = 5000;
      this.messageText = text;
      this.messageDisappearTimeout = setTimeout(() => {
        this.messageText = null;
      }, messageDurationMs);
    },
  },
  beforeDestroy() {},
};
</script>

<style>
</style>