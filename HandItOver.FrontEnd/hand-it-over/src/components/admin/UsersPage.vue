<template>
  <div>
    <h3>Users of the system</h3>
    <div>
      <label for="searchText">Search by name or email</label>
      <input
        for="searchText"
        type="text"
        v-model="searchQuery"
        v-on:input="searchUsersThrottled"
      />
    </div>
    <table class="table">
      <tr>
        <th>#</th>
        <th>Full name</th>
        <th>Email</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(user, i) in users" :key="user.id">
        <td>{{ i + 1 }}</td>
        <td>{{ user.fullName }}</td>
        <td>{{ user.email }}</td>
        <td>
          <router-link v-bind:to="`/account/${user.id}`">View</router-link>
        </td>
      </tr>
    </table>
    <div>
      <button
        v-for="(pageNum, pageIndex) in pagesCount"
        :key="pageIndex"
        v-on:click="onPageNumberChanged(pageIndex)"
      >
        {{ pageNum }}
      </button>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import { throttle } from "~/util/throttle";
export default {
  name: "UsersPage",
  data() {
    return {
      searchQuery: "",
      pageIndex: 0,
      pageSize: 10,
      pagesCount: 1,
      users: [],
    };
  },
  computed: {
    searchUsersThrottled() {
      const delayMs = 500;
      return throttle(this.searchUsers, delayMs);
    },
  },
  async mounted() {
    await this.searchUsers();
  },
  methods: {
    async searchUsers() {
      let usersResponse = await api.sendGet("/user/paginated", {
        pageIndex: this.pageIndex,
        pageSize: this.pageSize,
        search: this.searchQuery,
      });
      this.pagesCount = usersResponse.data.totalPages;
      this.users = usersResponse.data.users;
    },
    async onPageNumberChanged(newIndex) {
      if (this.pageIndex === newIndex) {
        return;
      }
      this.pageIndex = newIndex;
      await this.searchUsers();
    },
  },
};
</script>

<style>
</style>