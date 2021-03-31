<template>
  <div>
    <div class="tabs">
      <router-link v-bind:to="`/group/edit/${groupId}`" class="nav-link"
        >Edit</router-link
      >

      <router-link v-bind:to="`/group/rents/${groupId}`" class="nav-link"
        >Manage rents</router-link
      >

      <router-link v-bind:to="`/group/wl/${groupId}`" class="nav-link"
        >Whitelist</router-link
      >
    </div>
    <div>
      <group-edit v-if="tab === 'edit'" v-bind:groupId="groupId" />
      <rents-edit v-if="tab === 'rents'" v-bind:groupId="groupId" />
      <group-whitelist v-if="tab === 'wl'" v-bind:groupId="groupId" />
    </div>
  </div>
</template>

<script>
import GroupEdit from "~/components/groups/GroupEdit";
import RentsEdit from "~/components/groups/RentsEdit";
import GroupWhitelist from "~/components/groups/GroupWhitelist";

export default {
  name: "GroupPage",
  components: { GroupEdit, RentsEdit, GroupWhitelist },
  props: {
    tab: String,
    groupId: String,
  },
};
</script>

<style scoped>
.tabs {
  display: flex;
  background: rgb(85, 85, 85);
  overflow: hidden;
}

.tabs > * {
  margin: 2px 3px 0 3px;
  border-radius: 10px 10px 0px 0px;
  border-bottom: none;
}

.tabs :not(.router-link-exact-active) {
  background: #eee;
  position: relative;
  top: 4px;
}

.tabs .router-link-exact-active {
  background: white;
  border-radius: 27px 7px 0px 0px;
}

a {
  color: #000;
  font-size: 1.2em;
}
</style>