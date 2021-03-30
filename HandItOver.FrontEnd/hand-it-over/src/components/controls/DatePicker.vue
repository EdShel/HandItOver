<template>
  <div class="dates-container">
    <div
      class="date"
      v-bind:class="{ selected: selectedDateIndex == i }"
      v-for="(day, i) in getDays()"
      :key="i"
      v-on:click="changeSelectedDate(i)"
    >
      <div class="day-of-week">{{ getDayOfWeekShort(day) }}</div>
      <div class="day-of-month">{{ day.getDate() }}</div>
      <div class="month">{{ getMonthShort(day) }}</div>
    </div>
  </div>
</template>

<script>
import date from '~/util/date'

export default {
  name: "DatePicker",
  props: {
    daysForwardCount: Number,
  },
  data() {
    return {
      selectedDateIndex: 0,
    };
  },
  computed: {
    date() {
      let selectedDate = new Date();
      selectedDate.setDate(selectedDate.getDate() + this.selectedDateIndex);
      return date.setToMidnight(selectedDate);
    },
  },
  methods: {
    *getDays() {
      let dateItem = new Date();
      for (let i = 0; i < this.daysForwardCount; i++) {
        yield dateItem;
        dateItem.setDate(dateItem.getDate() + 1);
      }
    },
    getDayOfWeekShort(date) {
      return daysOfWeek[date.getDay()];
    },
    getMonthShort(date) {
      return months[date.getMonth()];
    },
    changeSelectedDate(selectedIndex) {
      this.selectedDateIndex = selectedIndex;
    },
  },
};

const daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
const months = [
  "Jan",
  "Feb",
  "Mar",
  "Apr",
  "May",
  "June",
  "July",
  "Aug",
  "Sept",
  "Oct",
  "Nov",
  "Dec",
];
</script>

<style scoped>
.dates-container {
  display: flex;
  width: 100%;
  overflow-x: scroll;
}

.date {
  position: relative;
  min-width: 80px;
  flex-grow: 0;
  flex-basis: 0;
  border: 1px solid black;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 5px;
  cursor: pointer;
}

.date.selected {
  background-color: rgb(138, 168, 212);
}

.day-of-week {
  align-self: flex-start;
}

.day-of-month {
  align-self: center;
  font-size: 1.6em;
  font-weight: bold;
  margin: 0 10px;
}

.month {
  align-self: flex-end;
}
</style>