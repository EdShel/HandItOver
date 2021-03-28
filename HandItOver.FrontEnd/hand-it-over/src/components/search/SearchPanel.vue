<template>
  <div>
    <input
      type="text"
      v-model="searchQuery"
      v-on:input="search"
      class="search-field"
    />
    <div>
      <search-item
        v-for="r in searchResults"
        :key="r.groupId"
        v-bind:query="searchQuery"
        v-bind:mainText="mainTextProvider(r)"
        v-bind:secondaryText="secondaryTextProvider(r)"
      />
    </div>
  </div>
</template>

<script>
import SearchItem from "~/components/search/SearchItem";

export default {
  name: "SearchPanel",
  components: { SearchItem },
  props: {
    dataProvider: Function,
    mainTextProvider: Function,
    secondaryTextProvider: Function,
  },
  data() {
    return {
      searchQuery: "",
      searchResults: [],
    };
  },
  methods: {
    search() {
      if (!this.searchQuery) {
        this.searchResults = [];
        return;
      }
      this.dataProvider(this.searchQuery)
        .then((r) => {
          this.searchResults = r;
        })
        .catch((e) => {
          this.searchResults = [];
        });
    }
  },
};
</script>

<style scoped>
.search-field {
  width: 100%;
  font-family: Verdana, Geneva, Tahoma, sans-serif;
}
</style>