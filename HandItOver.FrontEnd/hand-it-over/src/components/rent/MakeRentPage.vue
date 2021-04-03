<template>
  <div>
    <h3>{{ $t("rent.rentOptions") }}</h3>
    <div v-if="group">
      <div>
        <div class="row">
          <label for="packageSizeSelect" class="col-sm-3">{{
            $t("rent.packageSize")
          }}</label>
          <select
            id="packageSizeSelect"
            v-model="rentPackageSize"
            class="col-sm-9"
          >
            <option value="0">{{ $t("rent.small") }}</option>
            <option value="1">{{ $t("rent.medium") }}</option>
            <option value="2">{{ $t("rent.large") }}</option>
          </select>
        </div>
        <div class="row">
          <label for="rentDurationRange" class="col-sm-3"
            >{{ $t("rent.rentDuration") }}: {{ rentDurationMinutes }}
            {{ $t("rent.minutes") }}</label
          >
          <input
            type="range"
            min="15"
            v-bind:max="group.maxRentMinutes"
            step="5"
            v-model="rentDurationMinutes"
            class="col-sm-9"
          />
        </div>
      </div>
    </div>
    <div class="row">
      <label for="rentFromDate" class="col-sm-3">{{
        $t("rent.rentTime")
      }}</label>
      <div class="col-sm-9 date-picker">
        <date-picker
          ref="datePicker"
          v-bind:daysForwardCount="Number(14)"
        />
        <div class="row" v-if="$refs.datePicker">
          <div class="col col-lg-4 col-md-6 col-sm-6 col-xs-6">
            <time-picker ref="timePicker"></time-picker>
          </div>
          <div class="col col-lg-8 col-md-6 col-sm-6 col-xs-6">
            <vacant-intervals-table
              v-bind:intervals="vacantIntervalsForSelectedDay"
            />
          </div>
        </div>
      </div>
    </div>
    <div>
      <button v-on:click="renting" class="btn btn-primary rent-button">{{ $t("rent.rent") }}</button>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import date from "~/util/date";
import DatePicker from "~/components/controls/DatePicker";
import TimePicker from "~/components/controls/TimePicker";
import VacantIntervalsTable from "~/components/rent/VacantIntervalsTable";

export default {
  name: "MakeRentPage",
  props: {
    groupId: String,
  },
  components: {
    DatePicker,
    TimePicker,
    VacantIntervalsTable,
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
      let selectedDay = new Date(this.rentDate);
      let nextDay = new Date(selectedDay);
      nextDay.setDate(nextDay.getDate() + 1);
      nextDay.setSeconds(nextDay.getSeconds() - 1);

      let vacantIntervalsForDay = [];
      for (let interval of this.vacantIntervals) {
        let intervalOverlapsSelectedDay =
          selectedDay.getTime() <= interval.end.getTime() &&
          interval.begin.getTime() <= nextDay.getTime();
        if (!intervalOverlapsSelectedDay) {
          continue;
        }
        let intervalStart = date.max(selectedDay, interval.begin);
        let intervalEnd = date.min(nextDay, interval.end);
        vacantIntervalsForDay.push({ begin: intervalStart, end: intervalEnd });
      }
      return vacantIntervalsForDay;
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
        packageSize: this.rentPackageSize,
      })
      .then((r) => {
        this.vacantIntervals = r.data.map((interval) => ({
          begin: new Date(interval.begin),
          end: interval.end
            ? new Date(interval.end)
            : new Date(8640000000000000),
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

<style scoped>
h3 {
  font-size: 2em;
}

label {
  text-align: right;
}
.date-picker {
  padding: 0;
}
.rent-button {
  float: right;
}
</style>