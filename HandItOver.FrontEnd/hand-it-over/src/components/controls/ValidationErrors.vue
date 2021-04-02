<template>
  <transition-group name="fadeHeight" tag="p">
    <p v-for="error in errors" :key="error">
      {{ error }}
    </p>
  </transition-group>
</template>

<script>
export default {
  name: "ValidationErrors",
  props: {
    errors: Array,
  },
};
</script>

<style scoped>
p {
  color: #721010;
}

.fadeHeight-enter-active,
.fadeHeight-leave-active {
  transition: all 0.2s;
  max-height: 230px;
}
.fadeHeight-enter,
.fadeHeight-leave-to {
  opacity: 0;
  max-height: 0px;
}
</style>