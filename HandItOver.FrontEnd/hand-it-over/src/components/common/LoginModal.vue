<template>
  <modal-window
    ref="modalWindow"
    v-bind:header="$t('common.login')"
    v-bind:close-text="$t('common.closeAction')"
    v-bind:ok-text="$t('common.loginButton')"
    v-on:ok="loginPressed"
  >
    <validation-errors v-bind:errors="errors" />
    <div class="row mb-2">
      <label for="emailText" class="col-sm-3">{{ $t("users.email") }}</label>
      <input
        type="email"
        id="emailText"
        v-model="email"
        v-on:input="validateThrottled"
        class="col-sm-9"
      />
    </div>
    <div class="row">
      <label for="passwordText" class="col-sm-3">{{
        $t("users.password")
      }}</label>
      <input
        type="password"
        id="passwordText"
        v-model="password"
        v-on:input="validateThrottled"
        class="col-sm-9"
      />
    </div>
  </modal-window>
</template>

<script>
import ModalWindow from "~/components/controls/ModalWindow";
import api from "~/util/api.js";
import ValidationErrors from "~components/controls/ValidationErrors";
import authConstants from "~/util/authConstants";
import { throttle } from "~/util/throttle";

export default {
  name: "LoginModal",
  components: {
    ModalWindow,
    ValidationErrors,
  },
  data() {
    return {
      email: "",
      password: "",
      errors: [],
    };
  },
  computed: {
    validateThrottled() {
      const validateTimeout = 500;
      return throttle(this.validate, validateTimeout);
    },
  },
  methods: {
    show() {
      this.$refs.modalWindow.show();
    },
    hide() {
      this.$refs.modalWindow.hide();
    },
    async loginPressed() {
      if (!this.validate()) {
        return;
      }
      try {
        await api.login(this.email, this.password);
        this.hide();
        this.$router.go(0);
      } catch (e) {
        this.errors.push(this.$t("common.invalidEmailOrPassword"));
      }
    },
    validate() {
      let errors = [];

      if (!authConstants.emailRegex.test(this.email)) {
        errors.push(this.$t("common.invalidEmail"));
      }

      if (!authConstants.passwordRegex.test(this.password)) {
        errors.push(this.$t("common.invalidPassword"));
      }

      this.errors = errors;
      return errors.length == 0;
    },
  },
};
</script>

<style>
</style>