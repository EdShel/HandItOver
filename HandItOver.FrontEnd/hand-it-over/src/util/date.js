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

function setToMidnight(date) {
    date = new Date(date);
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    return date;
}

function max(firstDate, secondDate) {
    if (firstDate.getTime() > secondDate.getTime()) {
        return firstDate;
    }
    return secondDate;
}

function min(firstDate, secondDate) {
    if (firstDate.getTime() < secondDate.getTime()) {
        return firstDate;
    }
    return secondDate;
}

export default {
    hoursToHMmSs,
    hhMmSsToSeconds,
    secondsToHoursFloor,
    setToMidnight,
    max,
    min
}