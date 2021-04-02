<template>
  <modal-window
    ref="modalWindow"
    v-bind:header="$t('common.register')"
    v-bind:close-text="$t('common.closeAction')"
    v-bind:ok-text="$t('common.regiserAction')"
    v-on:ok="onRegisterPressed"
  >
    <validation-errors v-bind:errors="errors" />
    <div class="row mb-2">
      <label for="emailText" class="col-sm-3">{{$t('users.email')}}</label>
      <input
        type="email"
        id="emailText"
        v-model="email"
        v-on:input="validateThrottled"
        class="col-sm-9"
      />
    </div>
    <div class="row mb-2">
      <label for="fullNameText" class="col-sm-3">{{$t('users.fullName')}}</label>
      <input
        type="text"
        id="fullNameText"
        v-model="fullName"
        v-on:input="validateThrottled"
        class="col-sm-9"
      />
    </div>
    <div class="row mb-2">
      <label for="passwordText" class="col-sm-3">{{$t('users.password')}}</label>
      <input
        type="password"
        id="passwordText"
        v-model="password"
        v-on:input="validateThrottled"
        class="col-sm-9"
      />
    </div>
    <div class="row mb-2">
      <label for="passwordRepeatText" class="col-sm-3">{{$t('users.repeatPassword')}}</label>
      <input
        type="password"
        id="passwordRepeatText"
        v-model="passwordRepeat"
        v-on:input="validateThrottled"
        class="col-sm-9"
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
        this.errors.push(this.$t('common.passwordNotSame'));
      }
    },
    validate() {
      let errors = [];

      if (!authConstants.emailRegex.test(this.email)) {
        errors.push(this.$t('common.invalidEmail'));
      }

      if (!authConstants.passwordRegex.test(this.password)) {
        errors.push(this.$t('common.invalidPassword'));
      }

      if (this.password !== this.passwordRepeat) {
        errors.push(this.$t('common.userRegisterd'));
      }

      this.errors = errors;
      return errors.length === 0;
    },
  },
};
</script>

<style>
</style>