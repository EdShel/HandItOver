<template>
  <div>
    <h1>{{ $t('admin.header') }}</h1>
    <cert-expiration-display />
    <db-backup />
    <config-editor />
  </div>
</template>

<script>
import CertExpirationDisplay from '~/components/admin/CertExpirationDisplay';
import DbBackup from '~/components/admin/DbBackup';
import ConfigEditor from '~/components/admin/ConfigEditor';

export default {
  name: "AdminPage",
  components: {
    CertExpirationDisplay,
    DbBackup,
    ConfigEditor
  },
  data: function () {
    return {
      name: "Loser",
    };
  },
};
</script>