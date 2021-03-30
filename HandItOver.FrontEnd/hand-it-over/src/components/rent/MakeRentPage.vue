<template>
  <div>
    <h3>Rent options</h3>
    <div v-if="group">
      <div class="row">
        <div class="col col-lg-8 col-md-6 col-sm-6 col-xs-6">
          <div>
            <label for="packageSizeSelect">Package size</label>
            <select id="packageSizeSelect" v-model="rentPackageSize">
              <option value="0">Small</option>
              <option value="1">Medium</option>
              <option value="2">Large</option>
            </select>
          </div>
          <div>
            <label for="rentDurationRange"
              >Rent duration: {{ rentDurationMinutes }} minutes</label
            >
            <input
              type="range"
              min="15"
              v-bind:max="group.maxRentMinutes"
              step="5"
              v-model="rentDurationMinutes"
            />
          </div>
        </div>
        <div class="col col-lg-4 col-md-6 col-sm-6 col-xs-6">
          <time-picker ref="timePicker"></time-picker>
        </div>
      </div>
      <div>
        <label for="rentFromDate">Rent start</label>
        <date-picker
          ref="datePicker"
          v-bind:daysForwardCount="Number(14)"
        ></date-picker>
      </div>
      <div v-if="$refs.datePicker">
      {{ vacantIntervalsForSelectedDay }}

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
import TimePicker from "~/components/controls/TimePicker";

export default {
  name: "MakeRentPage",
  props: {
    groupId: String,
  },
  components: {
    DatePicker,
    TimePicker,
  },
  data() {
    return {
      group: {},
      rentPackageSize: 1,
      rentDurationMinutes: 120,
      vacantIntervals: [],
    };
  },
  computed: {
    rentTimeHours() {
      return this.$refs.timePicker.hours;
    },
    rentTimeMinutes() {
      return this.$refs.timePicker.minutes;
    },
    rentDate() {
      return this.$refs.datePicker.date;
    },
    rentFromTime() {
      let from = new Date(this.rentDate.getTime());
      let rentStartHours = this.rentTimeHours;
      let rentStartMinutes = this.rentTimeMinutes;
      from.setHours(rentStartHours);
      from.setMinutes(rentStartMinutes);
      from.setSeconds(0);
      from.setMilliseconds(0);
      return from;
    },
    rentUntilTime() {
      let until = new Date(this.rentFromTime);
      until.setMinutes(until.getMinutes() + 1 * this.rentDurationMinutes);
      return until;
    },
    vacantIntervalsForSelectedDay() {
      let selectedDay = date.setToMidnight(this.rentFromTime);
      let nextDay = new Date(selectedDay);
      nextDay.setDate(nextDay.getDate() + 1);

      let vacantIntervals = [];
      for (let interval of this.vacantIntervals) {
        if (interval.end < selectedDay && interval.start < nextDay) {
          continue;
        }
        let intervalStart = date.max(selectedDay, interval.begin);
        let intervalEnd = date.min(nextDay, interval.end);
        vacantIntervals.push({begin: intervalStart, end: intervalEnd});
      }
      return vacantIntervals;
    },
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
    api
      .sendGet(`/mailboxGroup/${this.groupId}/rentTime`, {
        packageSize: 1, // TODO: wtf
      })
      .then((r) => {
        this.vacantIntervals = r.data.map((interval) => ({
          begin: new Date(interval.begin),
          end: interval.end ? new Date(interval.end) : new Date(8640000000000000),
        }));
      })
      .catch((e) => {});
  },
  methods: {
    renting() {
      api
        .sendPost(`/mailboxGroup/${this.groupId}/rent`, null, {
          packageSize: 1 * this.rentPackageSize,
          rentFrom: this.rentFromTime,
          rentUntil: this.rentUntilTime,
        })
        .then((r) => {
          let data = r.data;
          this.$router.push(`/rent/${data.rentId}`);
        })
        .catch((e) => {});
    },
   
  },
};
</script>

<style>
</style>