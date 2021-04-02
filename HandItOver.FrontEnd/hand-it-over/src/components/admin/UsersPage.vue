<template>
  <div>
    <h3>{{$t('admin.usersHeader')}}</h3>
    <div class="row">
      <label for="searchText" class="col-sm-4">
        <i class="fas fa-search"></i>
        {{$t('admin.searchUser')}}
      </label>
      <input
        for="searchText"
        type="text"
        v-model="searchQuery"
        v-on:input="searchUsersThrottled"
        class="col-sm-8"
      />
    </div>
    <table class="table">
      <tr>
        <th>#</th>
        <th><i class="fas fa-user"></i> {{$t('users.fullName')}}</th>
        <th><i class="fas fa-at"></i> {{$t('users.email')}}</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(user, i) in users" :key="user.id">
        <td>{{ i + 1 }}</td>
        <td>{{ user.fullName }}</td>
        <td>{{ user.email }}</td>
        <td>
          <router-link v-bind:to="`/account/${user.id}`">{{$t('users.viewUser')}}</router-link>
        </td>
      </tr>
    </table>
    <div class="pages">
      <span
        v-for="(pageNum, i) in pagesCount"
        :key="i"
        v-on:click="onPageNumberChanged(i)"
        class="table-page"
        v-bind:class="{'current-page': pageIndex === i}"
      >
        {{ pageNum }}
      </span>
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
      pageSize: 5,
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

<style scoped>
table {
  margin-top: 20px;
}
td,
th {
  text-align: center;
}
label {
  text-align: right;
}
.pages {
  display: flex;
  flex-flow: row;
  align-items: center;
  justify-content: center;
}
.table-page {
  color: #007bff;
  cursor: pointer;
  font-size: 1.2em;
  margin: 0 10px;
}
.table-page:hover {
  text-decoration: underline;
}
.current-page {
  transform: translate(0, -4px);
  font-weight: bold;
}
</style>