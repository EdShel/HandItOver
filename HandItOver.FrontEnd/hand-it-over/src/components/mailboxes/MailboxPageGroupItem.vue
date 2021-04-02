<template>
  <div class="group">
    <div class="actions">
      <b>
        <i class="fas fa-boxes"></i>
        {{ mailboxGroup.name }}
      </b>
      <router-link
        v-bind:to="`/group/edit/${mailboxGroup.groupId}`"
        class="nav-link"
      >
        <i class="fas fa-cogs"></i> 
        {{$t('common.configureAction')}}
      </router-link>
    </div>
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
.group {
  border-radius: 10px;
  margin: 10px 0;
  padding: 17px;
  background-color: #efeef3;
}

.actions > * {
  display: inline-block;
  margin: 5px;
}

.nav-link {
  color: rgb(194, 11, 118);
  text-decoration: underline;
  padding: 5px;
  border-radius: 24px;
}
</style>