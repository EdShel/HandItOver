<template>
    <table>
        <tr>
            <th>Begin</th>
            <th>End</th>
        </tr>
        <tr v-for="(interval, i) in intervals" :key="i">
            <td> {{ timeString(interval.begin) }} </td>
            <td> {{ timeString(interval.end) }} </td>
        </tr>
    </table>
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

<style>

</style>