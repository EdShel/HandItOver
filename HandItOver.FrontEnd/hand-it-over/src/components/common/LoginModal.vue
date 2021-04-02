<template>
  <modal-window
    ref="modalWindow"
    header="Login"
    close-text="Close"
    ok-text="Login"
    v-on:ok="loginPressed"
  >
    <validation-errors v-bind:errors="errors" />
    <div>
      <label for="emailText">Email</label>
      <input
        type="email"
        id="emailText"
        v-model="email"
        v-on:input="validateThrottled"
      />
    </div>
    <div>
      <label for="passwordText">Password</label>
      <input
        type="password"
        id="passwordText"
        v-model="password"
        v-on:input="validateThrottled"
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
        this.errors.push("Invalid email or password.");
      }
    },
    validate() {
      let errors = [];

      if (!authConstants.emailRegex.test(this.email)) {
        errors.push("Invalid email address.");
      }

      if (!authConstants.passwordRegex.test(this.password)) {
        errors.push("Password must contain 6-20 characters.");
      }

      this.errors = errors;
      return errors.length == 0;
    },
  },
};
</script>

<style>
</style>