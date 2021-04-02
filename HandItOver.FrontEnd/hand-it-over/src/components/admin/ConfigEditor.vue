<template>
  <div v-if="configTree" class="config-tree">
    <h3>System configurations</h3>
    <config-property-item
      propertyName="appsettings.json"
      v-bind:propertyValue="configTree"
    />
    <button v-on:click="onSaveConfigurationsPressed" class="btn btn-primary">
      <i class="fas fa-save"></i>
      Save configurations
    </button>
    <button v-on:click="onDownloadConfigurationsPressed" class="btn btn-success">
      <i class="fas fa-download"></i>
      Download configurations
    </button>
  </div>
</template>

<script>
import api from "~/util/api";
import ConfigPropertyItem from "~/components/admin/ConfigPropertyItem";

export default {
  name: "ConfigEditor",
  components: {
    ConfigPropertyItem,
  },
  data() {
    return {
      configTree: null,
    };
  },
  async mounted() {
    await this.fetchConfiguration();
  },
  methods: {
    async onSaveConfigurationsPressed() {
      await this.pushConfiguration();
    },
    async fetchConfiguration() {
      let configResponse = await api.sendGet(`/admin/config/appsettings.json`);
      this.configTree = configResponse.data;
    },
    async pushConfiguration() {
      await api.sendPut(
        `/admin/config/appsettings.json`,
        null,
        this.configTree
      );
    },
    async onDownloadConfigurationsPressed() {
      let link = document.createElement("a");
      link.setAttribute("download", "appsettings.json");
      link.href = "data:text/plain;charset=utf-8," + encodeURIComponent(JSON.stringify(this.configTree));
      document.body.appendChild(link);
      link.click();
      link.remove();
    },
  },
};
</script>

<style scoped>
.config-tree {
  margin-top: 40px;
}

button {
  margin-right: 10px;
}
</style>