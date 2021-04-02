<template>
  <div v-if="group">
    <h3>{{ group.name }}</h3>
    <div>
      <div class="row mb-2">
        <label for="groupNameText" class="col-sm-4">Group name</label>
        <input
          id="groupNameText"
          type="text"
          v-model="edit.name"
          class="col-sm-8"
        />
      </div>
      <div class="row mb-2">
        <label for="whitelistCheckbox" class="col-sm-4"
          >Only whitelisted users can rent</label
        >
        <input
          id="whitelistCheckbox"
          type="checkbox"
          v-model="edit.whitelistOnly"
        />
      </div>
      <div class="row mb-2">
        <label for="maxRentHoursRange" class="col-sm-4"
          >Max rent hours {{ edit.maxRentHours }}</label
        >
        <input
          id="maxRentHoursRange"
          type="range"
          min="1"
          max="72"
          step="1"
          v-model="edit.maxRentHours"
          class="col-sm-8"
        />
      </div>
      <div>
        <button v-on:click="saveEdit" class="btn btn-primary">
          <i class="fas fa-save"></i>
          Save changes
        </button>
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
  name: "GroupEditTab",
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

<style scoped>
h3 {
  font-size: 2em;
}

label {
  text-align: right;
}

input[type="checkbox"] {
  margin-top: 6px;
}
</style>