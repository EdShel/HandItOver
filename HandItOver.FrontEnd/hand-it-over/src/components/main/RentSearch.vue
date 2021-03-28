<template>
  <div>
    <div>Search mailbox to rent</div>
    <p>
      You can find a smart mailbox to deliver your packages there. Just search
      by its name, owner or address.
    </p>
    <div>
      <search-panel
        v-bind:dataProvider="searchMailboxes"
        v-bind:mainTextProvider="nameSelector"
        v-bind:secondaryTextProvider="secondaryTextSelector"
      />
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import SearchPanel from '~/components/search/SearchPanel'

export default {
  name: "RentSearch",
  components: {SearchPanel},
  methods: {
    searchMailboxes(searchQuery) {
      return api
        .sendGet("/mailboxGroup", { search: searchQuery })
        .then((r) => {
          let data = r.data;
          return data;
        });
    },
    nameSelector(group){
      return group.name;
    },
    secondaryTextSelector(result) {
      return {
        owner: result.owner,
        addresses: result.addresses,
      };
    },
  },
};
</script>

<style scoped>

</style>