package ua.nure.sheliemietiev.handitover.views

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ListView
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.api.AuthorizationInfo
import ua.nure.sheliemietiev.handitover.views.menuItems.LogoutStrategy
import ua.nure.sheliemietiev.handitover.views.menuItems.MainMenuItem
import ua.nure.sheliemietiev.handitover.views.menuItems.MenuItemsAdapter
import ua.nure.sheliemietiev.handitover.views.menuItems.NextActivityStrategy
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
                "Current deliveries",
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(DeliveriesActivity::class)
            ),
            MainMenuItem(
                "Configure mailbox",
                R.drawable.ic_scan_qr_code,
                NextActivityStrategy(ConnectMailboxActivity::class)
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