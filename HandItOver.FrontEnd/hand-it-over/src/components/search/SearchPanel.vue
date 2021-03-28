<template>
  <div>
    <input
      type="text"
      v-model="searchQuery"
      v-on:input="throttledSearch"
      class="search-field"
    />
    <div>
      <search-item
        v-for="r in searchResults"
        :key="r.groupId"
        v-bind:query="searchQuery"
        v-on:keyup.enter="search"
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
  computed: {
    throttledSearch() {
      const searchDelayMs = 500;
      return throttle(this.search, searchDelayMs);
    },
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
    },
  },
};

function throttle(callback, limit) {
  let wait = false;
  let lastUnexecutedArg = null;
  return function (arg) {
    if (!wait) {
      callback.call(this, arg);
      wait = true;
      setTimeout(function () {
        if (lastUnexecutedArg) {
          callback.call(this, lastUnexecutedArg);
          lastUnexecutedArg = null;
        }
        wait = false;
      }, limit);
    } else {
      lastUnexecutedArg = arg;
    }
  };
}
</script>

<style scoped>
.search-field {
  width: 100%;
  font-family: Verdana, Geneva, Tahoma, sans-serif;
}
</style>