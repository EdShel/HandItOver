<template>
  <div>
    <p>{{ mailbox.address }}</p>
    <button v-if="!mailbox.groupId" @click="createGroup">Add to group</button>
    <button v-if="mailbox.groupId" @click="removeFromGroup">
      Remove from group
    </button>
  </div>
</template>

<script>
export default {
  name: "MailboxPageItem",
  props: {
    mailbox: {},
  },
  methods: {
    createGroup() {
      this.$emit("create-group");
    },
    removeFromGroup() {
      this.$emit("remove-from-group");
    },
  },
};
</script>

<style scoped>
div {
  border: 1px solid black;
}
</style>