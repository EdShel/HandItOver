<template>
  <div v-if="user">
    <div>Email: {{ user.email }}</div>
    <div>Full name: {{ user.fullName }}</div>
    <div>
      <user-rents v-bind:userId="user.id" />
    </div>
    <div>
      <user-deliveries v-bind:userId="user.id" />
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import UserRents from "~/components/userAccount/UserRents";
import UserDeliveries from "~/components/userAccount/UserDeliveries";

export default {
  name: "UserPage",
  components: {
    UserRents,
    UserDeliveries,
  },
  props: {
    userId: String,
  },
  data() {
    return {
      user: null,
    };
  },
  computed: {},
  mounted() {
    let userRequest = this.userId
      ? api.sendGet(`/user/byId/${this.userId}`)
      : (userRequest = api.sendGet("/user/me"));

    userRequest
      .then((r) => {
        this.user = r.data;
      })
      .catch((e) => {});
  },
  methods: {},
};
</script>

<style>
</style>