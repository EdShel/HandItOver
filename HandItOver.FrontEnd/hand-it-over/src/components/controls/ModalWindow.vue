<template>
  <transition name="invoice">
    <div
      class="modal"
      @mousedown.self="hide"
      v-if="isVisible"
      :class="{ visible: isVisible }"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLongTitle">
              {{ header }}
            </h5>
            <button type="button" class="close" v-on:click="hide">
              <span>&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <slot></slot>
          </div>
          <div class="modal-footer">
            <div class="bttn bttn-secondary" v-on:click="hide">
              <span class="bttn-text">
                {{ closeText }}
              </span>
            </div>
            <div class="bttn bttn-primary" v-on:click="$emit('ok')">
              <span class="bttn-text">
                {{ okText }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  name: "modal",
  props: ["header", "closeText", "okText"],
  data: function () {
    return {
      isVisible: false,
    };
  },
  methods: {
    show() {
      this.isVisible = true;
    },
    hide() {
      this.isVisible = false;
      this.$emit("cancel");
    },
  },
};
</script>

<style scoped>
.modal:not(.visible) {
  visibility: hidden;
}

.modal.visible {
  background: rgba(0, 0, 0, 0.8);
  display: block;
  visibility: visible;
}

.modal-content {
  border-width: 16px;
  border-style: solid;
  border-image: url(../../assets/9patchDeliveryNote.png) 16 repeat;
}

.invoice-enter-active,
.invoice-leave-active {
  transition: opacity 0.5s;
}
.invoice-enter,
.invoice-leave-to {
  opacity: 0;
}

.modal-content {
  transition: transform 0.5s;
}

.invoice-enter .modal-content {
  transform: translate(0, -100%) rotate(30deg) scale(0);
}
.invoice-leave-to .modal-content {
  transform: translate(100%, 0) rotate(360deg) scale(0);
}

.bttn {
  width: 96px;
  height: 96px;
  border: none;
  font-family: "Courier New", Courier, monospace;
  font-weight: bold;
  font-size: 1.2em;
  transition: transform 0.5s;
  outline: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  cursor: pointer
}

.bttn-primary {
  background: url(../../assets/okButton.png);
  transform: rotate(-10deg);
  color: #fff;
  position:relative;
}

.bttn-primary > .bttn-text {
  position:absolute;
  width: calc(100% + 10px);
  background-color: #2867e0;
  border-radius: 5px;
}

.bttn-secondary {
  background: url(../../assets/closeButton.png);
  background-repeat: no-repeat;
  background-size: 96px 96px;
  color: #e03c28;
  transform: rotate(5deg);
}

.bttn:hover {
  transform: scale(1.25);
  z-index: 2;
}

.bttn:focus {
  transform: translate(0, -12px);
}

.bttn:active {
  transform: scale(1.5);
  background-color: none;
}
</style>