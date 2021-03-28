<template>
  <div v-if="group">
    <h3>{{ group.name }}</h3>
    <div>
      <div>
        <label for="groupNameText">Group name</label>
        <input id="groupNameText" type="text" v-model="edit.name" />
      </div>
      <div>
        <label for="whitelistCheckbox">Only whitelisted users can rent</label>
        <input
          id="whitelistCheckbox"
          type="checkbox"
          v-model="edit.whitelistOnly"
        />
      </div>
      <div>
        <label for="maxRentHoursRamge">Max rent hours</label>
        <input
          id="maxRentHoursRange"
          type="range"
          min="1"
          max="72"
          step="1"
          v-model="edit.maxRentHours"
        />
      </div>
      <div>
        <button v-on:click="saveEdit">Save changes</button>
      </div>
      <div>
        <label for="whitelistCheckbox"></label>
      </div>
    </div>
  </div>
</template>

<script>
import api from "~/util/api";
import date from "~/util/date";

export default {
  name: "GroupEdit",
  props: {
    groupId: String,
  },
  data() {
    return {
      group: null,
      edit: {
        name: null,
        whitelistOnly: false,
        maxRentHours: 0,
      },
    };
  },
  mounted() {
      this.updateGroupInfo();
  },
  methods: {
    updateGroupInfo() {
      api
        .sendGet(`/mailboxGroup/${this.groupId}`)
        .then((r) => {
          let data = r.data;

          this.group = data;

          this.edit.name = data.name;
          this.edit.whitelistOnly = data.whitelistOnly;
          this.edit.maxRentHours = date.secondsToHoursFloor(
            date.hhMmSsToSeconds(data.maxRentTime)
          );
        })
        .catch((e) => {});
    },
    saveEdit() {
      api
        .sendPut(`/mailboxGroup/${this.groupId}`, null, {
          name: this.edit.name,
          whitelistOnly: this.edit.whitelistOnly,
          maxRentTime: date.hoursToHMmSs(this.edit.maxRentHours),
        })
        .then((r) => {
            this.updateGroupInfo();
        })
        .catch((e) => {});
    },
  },
};
</script>

<style>
</style>