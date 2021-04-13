package ua.nure.sheliemietiev.handitover.util

import android.content.Context
import ua.nure.sheliemietiev.handitover.R

fun localizedWeightString(context: Context, massInKilo: Float): String {
    val massUnit = context.getString(R.string.mass_unit)
    val massFormat = context.getString(R.string.mass_format)
    if (massUnit == "kilo") {
        return String.format(massFormat, massInKilo)
    }

    val massInPounds = massInKilo * 2.20462f;
    return String.format(massFormat, massInPounds)
}