<template>
  <div>
    <div class="tabs">
      <router-link v-bind:to="`/group/edit/${groupId}`" class="nav-link"
        >{{$t('groups.edit')}}</router-link
      >

      <router-link v-bind:to="`/group/rents/${groupId}`" class="nav-link"
        >{{$t('groups.rents')}}</router-link
      >

      <router-link v-bind:to="`/group/wl/${groupId}`" class="nav-link"
        >{{$t('groups.whitelist')}}</router-link
      >

      <router-link v-bind:to="`/group/joinlinks/${groupId}`" class="nav-link"
        >{{$t('groups.joinLinks')}}</router-link
      >
    </div>
    <div>
      <group-edit-tab v-if="tab === 'edit'" v-bind:groupId="groupId" />
      <rents-edit-tab v-if="tab === 'rents'" v-bind:groupId="groupId" />
      <group-whitelist v-if="tab === 'wl'" v-bind:groupId="groupId" />
      <join-tokens-tab v-if="tab === 'joinlinks'" v-bind:groupId="groupId" />
    </div>
  </div>
</template>

<script>
import GroupEditTab from "~/components/groups/GroupEditTab";
import RentsEditTab from "~/components/groups/RentsEditTab";
import GroupWhitelist from "~/components/groups/GroupWhitelist";
import JoinTokensTab from "~/components/groups/JoinTokensTab";

export default {
  name: "GroupPage",
  components: { GroupEditTab, RentsEditTab, GroupWhitelist, JoinTokensTab },
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
  margin-bottom: 40px;
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