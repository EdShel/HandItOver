package ua.nure.sheliemietiev.handitover.views

import android.content.Intent
import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ArrayAdapter
import android.widget.ListView
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.util.SecureStorage
import java.util.*
import javax.inject.Inject

class LanguageActivity : AppCompatActivity() {
    @Inject
     lateinit var secureStorage: SecureStorage

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_language)

        val supportedLanguages = resources.getStringArray(R.array.supported_languages)
        val supportedLocales = resources.getStringArray(R.array.supported_locales)
        val languagesList = findViewById<ListView>(R.id.languages_list)
        languagesList.adapter = ArrayAdapter(
            this,
            R.layout.support_simple_spinner_dropdown_item,
            supportedLanguages
        )
        languagesList.setOnItemClickListener { adapter, parent, position, id ->
            val localeName = supportedLocales[position]
            setAppLocale(localeName)
            val refresh = Intent(this, MainActivity::class.java)
            refresh.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
            startActivity(refresh)
        }
    }

    @Suppress("DEPRECATION")
    private fun setAppLocale(localeCode: String) {
        secureStorage.saveString("locale", localeCode)
        val locale = Locale(localeCode)
        Locale.setDefault(locale)

        val resources = resources
        val config = resources.configuration
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
            config.setLocale(locale)
        } else {
            config.locale = locale
        }

        val displayMetrics = resources.displayMetrics
        resources.updateConfiguration(config, displayMetrics)
    }
}