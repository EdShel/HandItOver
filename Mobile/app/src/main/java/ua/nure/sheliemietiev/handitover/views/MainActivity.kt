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
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(DeliveriesActivity::class)
            ),
            MainMenuItem(
                "Configure mailbox",
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(ConnectMailboxActivity::class)
            ),
            MainMenuItem(
                "Scan join link",
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(QRScanner::class)
            ),
            MainMenuItem(
                "Language",
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(LanguageActivity::class)
            ),
            MainMenuItem(
                "Visit web-site",
                R.drawable.ic_scan_qr_code,
                OpenUriStrategy(Uri.parse(BuildConfig.WEB_URL))
            ),
            MainMenuItem("Logout", R.drawable.ic_scan_qr_code, LogoutStrategy(authInfo))
        )
        val listAdapter = MenuItemsAdapter(this, menuItems)
        menuItemsList.adapter = listAdapter
        menuItemsList.setOnItemClickListener { parent, view, position, id ->
            val clickedMenuItem = listAdapter.getItem(position)
            clickedMenuItem.clickHandler.onClicked(this@MainActivity)
        }
    }
}