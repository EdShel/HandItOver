<template>
  <div class="clock-container">
    <div class="clock"></div>
    <div
      class="hand hour-hand"
      v-bind:style="{ transform: `rotate(${angleOfHourHand}deg)` }"
    ></div>
    <div class="hand minute-hand"
      v-bind:style="{ transform: `rotate(${angleOfMinuteHand}deg)` }"
    ></div>
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "ClockBackground",
  props: {
    hours: Number,
    minutes: Number,
  },
  computed: {
    angleOfHourHand() {
      return (((this.hours + this.minutes / 60) % 12) / 12) * 360 - 90;
    },
    angleOfMinuteHand() {
      return (this.minutes / 60) * 360 - 90;
    },
  },
};
</script>

<style scoped>
.clock-container {
  position: relative;
  width: 100%;
  padding-top: 100%;
}

.clock-container > * {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
}

.clock {
  margin: 10px;
  border: 14px solid rgb(179, 179, 179);
  border-radius: 99999px;
}

.hand {
  background: rgb(179, 179, 179);
  left: 50%;
  top: 50%;
}

.hour-hand {
  transform-origin: 0 3px;
  height: 6px;
  width: calc(50% - 60px);
}

.minute-hand {
  transform-origin: 0 2px;
  height: 4px;
  width: calc(50% - 30px);
}
</style>