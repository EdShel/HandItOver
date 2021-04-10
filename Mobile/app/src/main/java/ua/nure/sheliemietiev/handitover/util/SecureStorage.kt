package ua.nure.sheliemietiev.handitover.util

import android.content.SharedPreferences
import javax.inject.Inject

class SecureStorage @Inject constructor(
    private val sharedPreferences: SharedPreferences
) {
    fun saveString(key: String, value: String) {
        sharedPreferences.edit().putString(key, value).apply()
    }

    fun getStringOrNull(key: String): String? {
        return sharedPreferences.getString(key, null)
    }
}
