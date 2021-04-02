<template>
  <div>
    <div v-if="status == 0">
      <span v-on:click="onJoinPressed" class="join-button">Click to join</span>
    </div>
    <p v-else-if="status == 1">Waiting...</p>
    <p v-else-if="status == 2">Now you can rent this group.</p>
    <p v-else-if="status == 3">
      Sorry, invalid token or you already belong to the whitelist.
    </p>
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

<style scoped>
.join-button {
  display: block;
  text-align: center;
  margin-top: 40px;
  font-size: 3em;
  cursor: pointer;
  font-weight: bold;
}

.join-button:hover {
  text-decoration: underline;
}

p {
  font-size: 2em;
  text-align: center;
  width: 80%;
  margin: 0 auto;
}
</style>