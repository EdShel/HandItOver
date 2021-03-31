<template>
  <modal-window
    ref="modalWindow"
    header="Register"
    close-text="Close"
    ok-text="Register"
    v-on:ok="loginPressed"
  >
    <div>
      <label for="emailText">Email</label>
      <input type="text" id="emailText" v-bind="email" />
    </div>
    <div>
      <label for="passwordText">Password</label>
      <input type="text" id="passwordText" v-bind="password" />
    </div>
  </modal-window>
</template>

<script>
import ModalWindow from "~/components/controls/ModalWindow";
import api from '~/util/api'

export default {
  name: "RegisterModal",
  components: {
    ModalWindow,
  },
  data() {
    return {
      email: "",
      password: ""
    };
  },
  methods: {
      show() {
          this.$refs.modalWindow.openModal();
          this.visible = true;
      },
      hide() {
          this.$refs.modalWindow.closeModal();
          this.visible = false;
      },
      loginPressed(){
        api.login(this.email, this.password)
        .then(function(){
            this.hideLogin();
        })
        .catch(() => {
            console.log("So sad, can't login.")
        })
      }
  }
};
</script>

<style>
</style>