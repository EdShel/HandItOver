<template>
  <div>
      <p v-for="(error, i) in errors" :key="i">
          {{ error }}
      </p>
  </div>
</template>

<script>
export default {
    name: 'ValidationErrors',
    props: {
        errors: Array
    }
}
</script>

<style scoped>
p {
    color: #721010;
}
</style>