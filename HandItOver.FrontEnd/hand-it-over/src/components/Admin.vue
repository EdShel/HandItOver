<template>
  <div>
    <h1>Admin panel</h1>
    <h3>{{ name }}</h3>
  </div>
</template>

<script>
export default {
  name: "Admin",
  data: function () {
    return {
      name: "Loser",
    };
  },
};
</script>