<template>
  <div>
    <div v-if="status == 0">
      <button v-on:click="onJoinPressed">Join</button>
    </div>
    <div v-else-if="status == 1">Waiting...</div>
    <div v-else-if="status == 2">Now you can rent this group.</div>
    <div v-else-if="status == 3">
      Sorry, invalid token or you already belong to the whitelist.
    </div>
  </div>
</template>

<script>
import api from "~/util/api";

export default {
  name: "JoinGroupPage",
  props: {
    groupId: String,
    token: String,
  },
  data() {
    return {
      status: 0,
      timeout: null,
    };
  },
  methods: {
    onJoinPressed() {
      this.status;
      api
        .sendPost(`/mailboxGroup/${this.groupId}/whitelist/join`, null, {
          joinToken: this.token,
        })
        .then((r) => {
          this.onJoined();
        })
        .catch((e) => {
          this.onRejected();
        });
    },
    onJoined() {
      this.status = 2;
      this.timeout = setTimeout(() => {
        this.$router.push("/");
      });
    },
    onRejected() {
      this.status = 3;
    },
  },
  beforeDestroy() {
    if (this.timeout) {
      clearTimeout(this.timeout);
    }
  },
};
</script>

<style>
</style>