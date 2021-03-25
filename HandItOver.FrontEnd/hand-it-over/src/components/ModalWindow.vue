<template>
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
            class="btn btn-secondary"
            v-on:click="closeModal"
          >
            {{ closeText }}
          </button>
          <button
            type="button"
            class="btn btn-primary"
            v-on:click="$emit('ok')"
          >
            {{ okText }}
          </button>
        </div>
      </div>
    </div>
  </div>
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