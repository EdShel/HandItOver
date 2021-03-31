<template>
  <transition name="invoice">
    <div
      class="modal"
      @mousedown.self="closeModal"
      v-if="isVisible"
      :class="{ visible: isVisible }"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLongTitle">
              {{ header }}
            </h5>
            <button type="button" class="close" v-on:click="closeModal">
              <span>&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <slot></slot>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="bttn bttn-secondary"
              v-on:click="closeModal"
            >
              {{ closeText }}
            </button>
            <button
              type="button"
              class="bttn bttn-primary"
              v-on:click="$emit('ok')"
            >
              {{ okText }}
            </button>
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
    openModal() {
      this.isVisible = true;
    },
    closeModal() {
      this.isVisible = false;
      this.$emit("cancel");
    },
  },
};
</script>

<style scoped>
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
  text-align: center;
  font-size: 1.2em;
  transition: transform .5s;
  outline: 0;
}

.bttn-primary {
  background: url(../../assets/okButton.png);
  transform: rotate(-10deg);
  color: #fff;
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
  transform: scale(1.50);
  background-color: none;
}
</style>