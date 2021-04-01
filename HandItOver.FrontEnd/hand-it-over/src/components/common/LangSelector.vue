<template>
  <div>
    <select id="languageSelect" v-model="currentLanguage">
      <option
        v-for="lang in allowedLanguages"
        :key="lang.code"
        v-bind:value="lang.code"
      >
        {{ lang.name }}
      </option>
    </select>
  </div>
</template>

<script>
import { supportedLanguages, loadLanguageAsync } from "~/i18n";

export default {
  name: "LangSelector",
  data() {
    return {
      allowedLanguages: supportedLanguages,
    };
  },
  computed: {
    currentLanguage: {
      get() {
        return this.$i18n.locale;
      },
      set(value) {
          localStorage.setItem('language', value);
          loadLanguageAsync(value);
      },
    },
  },
};
</script>

<style>
</style>