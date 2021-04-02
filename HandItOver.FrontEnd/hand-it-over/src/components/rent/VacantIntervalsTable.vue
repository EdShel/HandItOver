<template>
<div>
    <h3>{{$t('rent.vacantIntervals')}}</h3>
    <table class="table">
        <tr>
            <th>{{$t('rent.begin')}}</th>
            <th>{{$t('rent.end')}}</th>
        </tr>
        <tr v-for="(interval, i) in intervals" :key="i">
            <td> {{ timeString(interval.begin) }} </td>
            <td> {{ timeString(interval.end) }} </td>
        </tr>
    </table>
</div>
</template>

<script>
import date from '~/util/date'

export default {
    name: 'VacantIntervalsTable',
    props: {
        intervals: Array
    },
    methods: {
        timeString(dateToFormat) {
            return date.localTimeString(dateToFormat);
        }
    }
}
</script>

<style scoped>
h3 {
    text-align: center;
}
</style>