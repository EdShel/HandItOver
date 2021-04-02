<template>
  <div v-if="isAuthorized" class="rent-search">
    <h3>{{ $t("main.rentSearchHeader") }}</h3>
    <p>
      {{ $t("main.rentSearchDescr") }}
    </p>
    <div>
      <search-panel
        v-bind:dataProvider="searchMailboxes"
        v-bind:mainTextProvider="nameSelector"
        v-bind:secondaryTextProvider="secondaryTextSelector"
        v-on:found-item="onMailboxGroupFound"
      />
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import SearchPanel from "~/components/search/SearchPanel";

export default {
  name: "RentSearch",
  components: { SearchPanel },
  computed: {
    isAuthorized() {
      return api.isAuthorized();
    },
  },
  methods: {
    searchMailboxes(searchQuery) {
      return api.sendGet("/mailboxGroup", { search: searchQuery }).then((r) => {
        let data = r.data;
        return data;
      });
    },
    nameSelector(group) {
      return group.name;
    },
    secondaryTextSelector(result) {
      return {
        owner: result.owner,
        addresses: result.addresses,
      };
    },
    onMailboxGroupFound(group) {
      this.$router.push(`/rentMailbox/${group.groupId}`);
    },
  },
};
</script>

<style scoped>
.rent-search {
  margin-top: 30px;
  margin-left: auto;
  margin-right: auto;
  width: 50%;
}
</style>