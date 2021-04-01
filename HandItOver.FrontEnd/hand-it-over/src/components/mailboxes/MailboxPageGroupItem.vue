<template>
  <div>
    {{ mailboxGroup.name }}
    <router-link v-bind:to="`/group/edit/${mailboxGroup.groupId}`" class="nav-link"
      >Edit group</router-link
    >
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "MailboxPageGroupItem",
  props: {
    mailboxGroup: {},
  },
};
</script>

<style scoped>
div {
  border: 1px solid black;
  padding-left: 15px;
}
</style>