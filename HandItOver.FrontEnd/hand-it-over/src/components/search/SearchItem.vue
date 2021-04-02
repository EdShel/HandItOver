<template>
  <div v-on:click="selected">
    <span v-html="mainTextHtml"></span>
    <span v-if="secondaryTextHtml.length"> - </span>
    <i
      v-html="secondaryMatch"
      v-for="(secondaryMatch, i) in secondaryTextHtml"
      :key="i"
    >
    </i>
  </div>
</template>

<script>
export default {
  name: "SearchItem",
  props: { query: String, mainText: String, secondaryText: Object },
  computed: {
    mainTextHtml() {
      const regex = new RegExp(this.query, "gi");
      return this.mainText.replace(regex, "<b>$&</b>");
    },
    secondaryTextHtml() {
      const regex = new RegExp(this.query, "gi");
      let matchedProps = [];
      for (let property in this.secondaryText) {
        const value = this.secondaryText[property];
        if (Array.isArray(value)) {
          for (let arrayElement of value) {
            if (arrayElement.match(regex)) {
              matchedProps.push(arrayElement.replace(regex, "<b>$&</b>"));
            }
          }
        } else if (value.match(regex)) {
          matchedProps.push(value.replace(regex, "<b>$&</b>"));
        }
      }
      return matchedProps;
    },
  },
  methods: {
    selected() {
      this.$emit("selected-item");
    },
  },
};
</script>

<style scoped>
div {
  width: 100%;
  cursor: pointer;
  border: 1px solid black;
  font-family: Verdana, Geneva, Tahoma, sans-serif;
  padding: 5px;
}

div:not(:first-child) {
  border-top: none;
  background: rgb(224, 224, 224);
}

div:first-child {
  background: rgb(240, 240, 240);
}

div:hover {
  background: #fff;
}
</style>