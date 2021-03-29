<template>
  <clock-background v-bind:hours="hours" v-bind:minutes="minutes">
    <div class="time-picker">
      <div class="selector hours">
        <label for="hourSelect">Hours</label>
        <select id="hourSelect" v-model="hours">
          <option v-for="(x, i) in 24" :key="i" v-bind:value="i">
            {{ i.toString().padStart(2, "0") }}
          </option>
        </select>
      </div>
      <div class="selector minutes">
        <label for="minuteSelect">Minutes</label>
        <select id="minuteSelect" v-model="minutes">
          <option v-for="(x, i) in 12" :key="i" v-bind:value="i * 5">
            {{ (i * 5).toString().padStart(2, "0") }}
          </option>
        </select>
      </div>
    </div>
  </clock-background>
</template>

<script>
import ClockBackground from "~/components/controls/ClockBackground";

export default {
  name: "TimePicker",
  components: {
    ClockBackground,
  },
  data() {
    let now = new Date();
    return {
      hours: now.getHours(),
      minutes: Math.floor(now.getMinutes() / 5) * 5,
    };
  },
};
</script>

<style scoped>
.time-picker {
  display: flex;
  width: 100%;
}
.time-picker > * {
  flex-grow: 1;
}

.selector {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

label {
    font-size: 1.2em;
    font-weight: bold;
}

select {
  min-width: auto;
  width: 50%;
  border: 2px solid rgb(179, 179, 179);
}
</style>