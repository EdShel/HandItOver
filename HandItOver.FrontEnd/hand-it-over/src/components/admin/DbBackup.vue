<template>
  <div>
    <div class="row">
      <label for="backupFileText" class="col-sm-4"
        >{{$t('admin.backupDbHeader')}}</label
      >
      <input
        id="backupFileText"
        type="text"
        v-model="backupFileName"
        class="col-sm-8"
      />
    </div>
    <div>
      <button v-on:click="onMakeBackupPressed" class="btn btn-primary">
        <i class="fas fa-save"></i>
        {{$t('admin.makeBackup')}}
      </button>
      <button v-on:click="onDownloadBackupPressed" class="btn btn-success">
        <i class="fas fa-download"></i>
        {{$t('admin.downloadBackup')}}
      </button>
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
      this.showMessage("Back up is succesfully made.");
    },
    async onDownloadBackupPressed() {
      await api.downloadBlobFile(
        "/admin/backup",
        {
          file: this.backupFileName,
        },
        this.backupFileName
      );
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

<style scoped>
label {
  text-align: right;
}

button {
  margin: 0 10px;
}
</style>