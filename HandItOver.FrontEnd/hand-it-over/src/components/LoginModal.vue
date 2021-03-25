<template>
  <modal-window
    ref="modalWindow"
    header="Login"
    close-text="Close"
    ok-text="Login"
    v-on:ok="loginPressed"
  >
    <div>
      <label for="emailText">Email</label>
      <input type="text" id="emailText" v-model="email" />
    </div>
    <div>
      <label for="passwordText">Password</label>
      <input type="text" id="passwordText" v-model="password" />
    </div>
  </modal-window>
</template>

<script>
import ModalWindow from "./ModalWindow";
import api from '../util/api.js'

export default {
  name: "LoginModal",
  components: {
    ModalWindow,
  },
  data() {
    return {
      email: "",
      password: "",
    };
  },
  methods: {
      show() {
          this.$refs.modalWindow.openModal();
      },
      hide() {
          this.$refs.modalWindow.closeModal();
      },
      loginPressed(){
        api.login(this.email, this.password)
        .then(() => {
            this.hide();
            location.reload();
        })
        .catch((e) => {
            console.log("So sad, can't login because of ");
            console.log(e);
        })
      }
  }
};
</script>

<style>
</style>