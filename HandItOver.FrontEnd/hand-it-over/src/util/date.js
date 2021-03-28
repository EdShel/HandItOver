function hoursToHMmSs(hours) {
    let hoursStr = hours.toString();
    return `${hoursStr}:00:00`;
}

function hhMmSsToSeconds(ddHhMmSs) {
    let matchResult = ddHhMmSs.match(/(.+?):(.+?):(.+)$/);
    let hours = matchResult[1];
    let minutes = matchResult[2];
    let seconds = matchResult[3];

    return (hours * 60 * 60)
        + (minutes * 60)
        + (seconds * 1);
}

function secondsToHoursFloor(seconds) {
    return Math.floor(seconds / 3600);
}

export default {
    hoursToHMmSs,
    hhMmSsToSeconds,
    secondsToHoursFloor
}