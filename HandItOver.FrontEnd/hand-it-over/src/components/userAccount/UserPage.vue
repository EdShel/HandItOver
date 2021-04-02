<template>
  <div v-if="user">
    <h1>{{ user.fullName }}</h1>
    <p><b><i class="fas fa-envelope"></i> {{$t('users.email')}}:</b> {{ user.email }}</p>
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

<style scoped>
p {
  font-size: 1.5em;
}
</style>