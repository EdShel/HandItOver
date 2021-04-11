package ua.nure.sheliemietiev.handitover.util

import android.content.Context
import ua.nure.sheliemietiev.handitover.R

fun localizedWeight(context: Context, massInKilo: Float): Float {
    val massUnit = context.getString(R.string.mass_unit)
    if (massUnit == "kilo") {
        return massInKilo
    }
    // Pounds
    return massInKilo * 2.20462f;
}