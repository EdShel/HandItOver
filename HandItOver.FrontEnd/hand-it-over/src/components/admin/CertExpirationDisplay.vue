<template>
  <div v-if="expirationDate">
      <b>
      The SSL certificate will expire at
      </b>
      <i>
          {{ formatDate(this.expirationDate) }}
      </i>
  </div>
</template>

<script>
import api from "~/util/api";
import dateUtil from '~/util/date';

export default {
  name: "CertExpirationDisplay",
  data() {
    return {
      expirationDate: null,
    };
  },
  async mounted() {
      let r = await api.sendGet(`/admin/sslExpiration`);
      this.expirationDate = new Date(r.data.expires);
  },
  methods: {
      formatDate(date) {
          return dateUtil.localString(date);
      }
  }
};
</script>

<style scoped>
div {
  margin: 30px 0;
}
</style>