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
      v-on:value-changed="onValueChanged"
    />
  </div>
  <div v-else-if="typeof propertyValue === 'number'" class="group">
    <label v-bind:for="propertyName + myId">{{ propertyName }}</label>
    <input
      v-bind:id="propertyName + myId"
      v-model="changedValue"
      type="number"
    />
  </div>
  <div v-else-if="typeof propertyValue === 'string'" class="group">
    <label v-bind:for="propertyName + myId">{{ propertyName }}</label>
    <input v-bind:id="propertyName + myId" v-model="changedValue" type="text" />
  </div>
  <div v-else-if="typeof propertyValue === 'boolean'" class="group">
    <label v-bind:for="propertyName + myId">{{ propertyName }}</label>
    <input
      v-bind:id="propertyName + myId"
      v-model="changedValue"
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
  data() {
    return {
      newValue: null,
    };
  },
  computed: {
    myId() {
      return this._uid;
    },
    changedValue: {
      get() {
        return this.newValue;
      },
      set(val) {
        this.newValue = val;
        this.$emit("value-changed", { prop: this.propertyName, val: val });
      },
    },
  },
  mounted() {
    this.changedValue = this.propertyValue;
  },
  methods: {
    onValueChanged(obj) {
      if (!this.changedValue){
        return;
      }
      this.changedValue[obj.prop] = obj.val;
      this.$emit("value-changed", {
        prop: this.propertyName,
        val: this.changedValue,
      });
    },
  },
};
</script>

<style scoped>
div {
  margin-left: 10px;
}

.group {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.group > label {
  margin-right: 10px;
  margin-bottom: 0;
}

.group > input:not([type="checkbox"]) {
  flex-grow: 1;
}
</style>