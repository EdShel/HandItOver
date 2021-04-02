<template>
  <modal-window
    ref="modalWindow"
    header="Register"
    close-text="Close"
    ok-text="Register"
    v-on:ok="onRegisterPressed"
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
      <label for="fullNameText">Full name</label>
      <input
        type="text"
        id="fullNameText"
        v-model="fullName"
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
    <div>
      <label for="passwordRepeatText">Repeat password</label>
      <input
        type="password"
        id="passwordRepeatText"
        v-model="passwordRepeat"
        v-on:input="validateThrottled"
      />
    </div>
  </modal-window>
</template>

<script>
import ModalWindow from "~/components/controls/ModalWindow";
import ValidationErrors from "~components/controls/ValidationErrors";
import authConstants from "~/util/authConstants";
import { throttle } from "~/util/throttle";
import api from "~/util/api.js";

export default {
  name: "RegisterModal",
  components: {
    ModalWindow,
    ValidationErrors,
  },
  data() {
    return {
      email: "",
      fullName: "",
      password: "",
      passwordRepeat: "",
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
    async onRegisterPressed() {
      if (!this.validate()) {
        return;
      }
      try {
        await api.register(this.email, this.fullName, this.password, "user");
        await api.login(this.email, this.password);
        this.hide();
        this.$router.go(0);
      } catch (e) {
        this.errors.push("The user with the email is already registered.");
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

      if (this.password !== this.passwordRepeat) {
        errors.push("Passwords are not the same.");
      }

      this.errors = errors;
      return errors.length === 0;
    },
  },
};
</script>

<style>
</style>