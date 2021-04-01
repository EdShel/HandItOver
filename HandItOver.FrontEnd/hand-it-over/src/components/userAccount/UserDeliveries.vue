<template>
  <div>
    <h3>Current deliveries</h3>
    <table class="table table-hover">
      <tr>
        <th>#</th>
        <th>Arrived</th>
        <th>Terminal time</th>
        <th>Weight</th>
        <th>&nbsp;</th>
      </tr>
      <tr v-for="(d, i) in deliveries" :key="d.id">
        <td>{{ i + 1 }}</td>
        <td>{{ formatDate(d.arrived) }}</td>
        <td>{{ formatDate(d.terminal) }}</td>
        <td>{{ $t('units.mass', [toLocalMass(d.weight).toFixed(2)]) }}</td> 
        <td><router-link v-bind:to="'/delivery/' + d.id">View</router-link></td>
      </tr>
    </table>
  </div>
</template>

<script>
import api from '~/util/api';
import dateUtil from '~/util/date';
import { localWeight } from '~/util/units';

export default {
    name: 'UserDeliveries',
    components: {

    },
    props: {
        userId: String
    },
    data() {
        return{
            deliveries: []
        };
    },
    mounted() {
        api.sendGet(`/delivery/active/${this.userId}`).then(r => {
            this.deliveries = r.data.map(d => ({
                id: d.id,
                weight: d.weight,
                mailboxId: d.mailboxId,
                arrived: new Date(d.arrived),
                terminal: new Date(d.terminalTime)
            }));
        }).catch(e => {

        });
    },
    methods: {
        formatDate(date) {
            return dateUtil.localString(date);
        },
        toLocalMass(mass){
          return localWeight(this.$t('units.massUnit'), mass);
        }
    }
}
</script>

<style>
</style>