<template>
  <div>
    <add-to-whitelist-modal
      ref="addToWhitelistModal"
      v-bind:groupId="groupId"
      v-on:added-to-whitelist="onAddedToWhitelist"
    />

    <div>
      <h3>{{$t('groups.whitelistedPeople')}}</h3>
      <button v-on:click="onAddToWhitelistPressed" class="btn btn-success">
        <i class="fas fa-plus"></i> {{$t('groups.addUserToWh')}}
      </button>
      <table class="table">
        <tr>
          <th>#</th>
          <th><i class="fas fa-at"></i> {{$t('users.email')}}</th>
          <th><i class="fas fa-user"></i> {{$t('users.fullName')}}</th>
          <th>&nbsp;</th>
          <th>&nbsp;</th>
        </tr>
        <tr v-for="(user, i) in whitelisted" :key="user.id">
          <td>{{ i + 1 }}</td>
          <td>{{ user.email }}</td>
          <td>{{ user.fullName }}</td>
          <td>
            <router-link v-bind:to="`/account/${user.id}`"
              >{{$t('users.viewUser')}}</router-link
            >
          </td>
          <td v-on:click="removeUserPressed(user)" class="remove">{{$t('common.removeAction')}}</td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import AddToWhitelistModal from "~/components/groups/AddToWhitelistModal";

export default {
  name: "GroupWhitelist",
  components: {
    AddToWhitelistModal,
  },
  props: {
    groupId: String,
  },
  data() {
    return {
      whitelisted: [],
    };
  },
  async mounted() {
    let r = await api.sendGet(`/mailboxGroup/${this.groupId}/whitelist`);
    this.whitelisted = r.data.entries;
  },
  methods: {
    async removeUserPressed(user) {
      await api.sendDelete(
        `/mailboxGroup/${this.groupId}/whitelist/${user.email}`
      );
      let elIndex = this.whitelisted.indexOf(user);
      this.$delete(this.whitelisted, elIndex);
    },
    onAddToWhitelistPressed() {
      this.$refs.addToWhitelistModal.show();
    },
    onAddedToWhitelist(user) {
      this.whitelisted.unshift(user);
    },
  },
};
</script>

<style scoped>
table {
  margin-top: 20px;
}

td,
th {
  text-align: center;
}

.remove {
  color: rgb(167, 17, 17);
  cursor: pointer;
}

.remove:hover {
  text-decoration: underline;
}
</style>