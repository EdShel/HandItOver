<template>
  <div>
    <div>
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
    </div>
  </div>
</template>

<script>
import GroupEdit from '~/components/groups/GroupEdit';

export default {
  name: "GroupPage",
  components: {GroupEdit},
  props: {
    tab: String,
    groupId: String,
  },
};
</script>

<style>
</style>