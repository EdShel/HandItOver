<template>
  <div>
    <div>Search mailbox to rent</div>
    <p>
      You can find a smart mailbox to deliver your packages there. Just search
      by its name, owner or address.
    </p>
    <div>
      <input type="text" v-model="searchQuery" v-on:input="search" />
      <div>
        <div v-for="r in searchResults" :key="r.groupId">
          <search-item
            v-bind:query="searchQuery"
            v-bind:mainText="r.name"
            v-bind:secondaryText="getSearchSecondaryText(r)"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import SearchItem from "~/components/main/SearchItem";

export default {
  name: "RentSearch",
  components: { SearchItem },
  data() {
    return {
      searchQuery: "",
      searchResults: [],
    };
  },
  methods: {
    search() {
      if (!this.searchQuery){
        this.searchResults = [];
        return;
      }
      api
        .sendGet("/mailboxGroup", { search: this.searchQuery })
        .then((r) => {
          let data = r.data;
          this.searchResults = data;
        })
        .catch((e) => {});
    },
    getSearchSecondaryText(result) {
      return {
        owner: result.owner,
        addresses: result.addresses
      };
    }
  },
};
</script>

<style>
</style>