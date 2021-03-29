<template>
  <div>
    <h3>Rent options</h3>
    <div v-if="group">
      <div>
        <label for="packageSizeSelect">Package size</label>
        <select id="packageSizeSelect" v-model="rentInfo.packageSize">
          <option value="0">Small</option>
          <option value="1">Medium</option>
          <option value="2">Large</option>
        </select>
      </div>
      <div>
        <label for="rentFromDate">Rent start</label>
        <input id="rentFromDate" type="datetime" v-model="rentInfo.rentFrom" />
        <date-picker v-bind:daysForwardCount="Number(14)"></date-picker>
      </div>
      <div>
        <label for="rentDurationRange">Rent duration (minutes)</label>
        <input
          type="range"
          min="15"
          v-bind:max="group.maxRentMinutes"
          step="5"
          v-model="rentInfo.rentDurationMinutes"
        />
      </div>
      <div>
        <button v-on:click="renting">Rent</button>
      </div>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import date from "~/util/date";
import DatePicker from "~/components/controls/DatePicker";

export default {
  name: "RentPage",
  props: {
    groupId: String,
  },
  components: {
    DatePicker
  },
  data() {
    return {
      group: {},
      rentInfo: {
        packageSize: 1,
        rentFrom: new Date(),
        rentDurationMinutes: 120,
      },
    };
  },
  mounted() {
    api
      .sendGet(`/mailboxGroup/${this.groupId}`)
      .then((r) => {
        let data = r.data;
        this.group = {
          name: data.name,
          maxRentMinutes: date.hhMmSsToSeconds(data.maxRentTime) / 60,
          whitelistOnly: data.whitelistOnly,
        };
      })
      .catch((e) => {});
  },
  methods: {
    renting() {
      console.log("Renting mailbox");
    },
  },
};
</script>

<style>
</style>