<template>
  <div>
      <slot></slot>
  </div>
</template>

<script>
export default {
  name: "MailboxPageGroupItem",
  props: {
    mailboxGroupId: {},
  },
};
</script>

<style scoped>
div {
  border: 1px solid black;
  padding-left: 15px;
}
</style>