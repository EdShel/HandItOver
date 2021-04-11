package ua.nure.sheliemietiev.handitover.util

import android.content.Context
import ua.nure.sheliemietiev.handitover.R
import java.text.ParseException
import java.text.SimpleDateFormat
import java.util.*

fun iso8601ToDate(formatted: String): Date {
    return try {
        val format = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", Locale.ENGLISH)
        format.timeZone = TimeZone.getTimeZone("UTC")
        format.parse(formatted)!!
    } catch (e: ParseException) {
        val format = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.ENGLISH)
        format.timeZone = TimeZone.getTimeZone("UTC")
        format.parse(formatted)!!
    }
}

fun Date.toLocaleTimeString(context: Context): String {
    val timeFormat = context.getString(R.string.locale_time)
    val dateFormat = SimpleDateFormat(timeFormat, Locale.ENGLISH)
    dateFormat.timeZone = TimeZone.getDefault()
    return dateFormat.format(this)
}

fun Date.toLocaleDateTimeString(context: Context): String {
    val timeFormat = context.getString(R.string.locale_date_time)
    val dateFormat = SimpleDateFormat(timeFormat, Locale.ENGLISH)
    dateFormat.timeZone = TimeZone.getDefault()
    return dateFormat.format(this)
}

fun currentTimeUtc(): Long {
    return System.currentTimeMillis()
}

fun Date.differenceHours(other: Date): Int {
    val myTimeStamp = this.time
    val otherTimeStamp = other.time
    val timeStampDifference = myTimeStamp - otherTimeStamp
    return (timeStampDifference / 1000 / 3600).toInt()
}