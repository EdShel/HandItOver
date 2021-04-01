<template>
  <div v-if="expirationDate">
      <span>
      The SSL certificate will expire at
      </span>
      <b>
          {{ formatDate(this.expirationDate) }}
      </b>
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

<style>
</style>