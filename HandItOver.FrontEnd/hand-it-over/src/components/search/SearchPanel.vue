<template>
  <div class="search-panel">
    <div class="search-container">
      <input
        type="text"
        v-model="searchQuery"
        v-on:input="throttledSearch"
        v-on:keyup.enter="found(searchResults[0])"
        class="search-field"
      />
      <div class="search-icon">TODO: place magnifier icon</div>
    </div>
    <div class="suggestions">
      <search-item
        v-for="r in searchResults"
        :key="r.groupId"
        v-bind:query="searchQuery"
        v-bind:mainText="mainTextProvider(r)"
        v-bind:secondaryText="secondaryTextProvider(r)"
        v-on:selected-item="found(r)"
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
    found(item) {
        if (item){
            this.searchQuery = this.mainTextProvider(item);
            this.$emit('found-item', item);
        }
    }
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

.search-panel {
    position: relative;
}

.search-container {
    position: relative;
}

.search-icon {
    position: absolute;
    right: 0;
    top: 0;
}

.suggestions {
    position: absolute;
    width: 100%;
    z-index: 2;
}

search-item {
    width: 100%;
}
</style>