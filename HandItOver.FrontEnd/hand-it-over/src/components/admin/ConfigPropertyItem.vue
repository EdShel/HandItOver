<template>
  <div v-if="typeof propertyValue === 'object'" class="layer">
    <b>
      {{ propertyName }}
    </b>

    <config-property-item
      v-for="(propValue, propName) in propertyValue"
      :key="propName"
      v-bind:propertyName="propName"
      v-bind:propertyValue="propValue"
    />
  </div>
  <div v-else-if="typeof propertyValue === 'number'">
    <label v-bind:for="propertyName + myId">{{ propertyName }}</label>
    <input
      v-bind:id="propertyName + myId"
      v-model="propertyValue"
      type="number"
    />
  </div>
  <div v-else-if="typeof propertyValue === 'string'">
    <label v-bind:for="propertyName + myId">{{ propertyName }}</label>
    <input
      v-bind:id="propertyName + myId"
      v-model="propertyValue"
      type="text"
    />
  </div>
  <div v-else-if="typeof propertyValue === 'boolean'">
    <label v-bind:for="propertyName + myId">{{ propertyName }}</label>
    <input
      v-bind:id="propertyName + myId"
      v-model="propertyValue"
      type="checkbox"
    />
  </div>
</template>

<script>
export default {
  name: "ConfigPropertyItem",
  props: {
    propertyName: String,
    propertyValue: null,
  },
  computed: {
    myId() {
      return this._uid;
    },
  },
};
</script>

<style scoped>
div {
  margin-left: 10px;
}
</style>