package ua.nure.sheliemietiev.handitover.views

import android.net.Uri
import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ListView
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.BuildConfig
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.api.AuthorizationInfo
import ua.nure.sheliemietiev.handitover.views.menuItems.*
import javax.inject.Inject

class MainActivity : AppCompatActivity() {
    @Inject
    lateinit var authInfo: AuthorizationInfo

    private lateinit var menuItemsList: ListView

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        menuItemsList = findViewById(R.id.menu_items_list)
        val menuItems = listOf(
            MainMenuItem(
                getString(R.string.current_deliveries),
                R.drawable.ic_delivery,
                NextActivityStrategy(DeliveriesActivity::class)
            ),
            MainMenuItem(
                getString(R.string.configure_mailbox),
                R.drawable.ic_configure,
                NextActivityStrategy(ConnectMailboxActivity::class)
            ),
            MainMenuItem(
                getString(R.string.scan_join_link),
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(QRScanner::class)
            ),
            MainMenuItem(
                getString(R.string.change_lang),
                R.drawable.ic_language,
                NextActivityStrategy(LanguageActivity::class)
            ),
            MainMenuItem(
                getString(R.string.go_to_web),
                R.drawable.ic_web,
                OpenUriStrategy(Uri.parse(BuildConfig.WEB_URL))
            ),
            MainMenuItem(getString(R.string.logout), R.drawable.ic_exit, LogoutStrategy(authInfo))
        )
        val listAdapter = MenuItemsAdapter(this, menuItems)
        menuItemsList.adapter = listAdapter
        menuItemsList.setOnItemClickListener { _, _, position, _ ->
            val clickedMenuItem = listAdapter.getItem(position)
            clickedMenuItem.clickHandler.onClicked(this@MainActivity)
        }
    }
}