package ua.nure.sheliemietiev.handitover.views

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.ListView
import android.widget.Toast
import com.google.android.gms.tasks.OnCompleteListener
import com.google.firebase.messaging.FirebaseMessaging
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.api.AuthorizationInfo
import ua.nure.sheliemietiev.handitover.views.menuItems.LogoutStrategy
import ua.nure.sheliemietiev.handitover.views.menuItems.MainMenuItem
import ua.nure.sheliemietiev.handitover.views.menuItems.MenuItemsAdapter
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
             MainMenuItem("Logout", R.drawable.ic_scan_qr_code, LogoutStrategy(authInfo))
        )
        val listAdapter = MenuItemsAdapter(this, menuItems)
        menuItemsList.adapter = listAdapter
        menuItemsList.setOnItemClickListener { parent, view, position, id ->
            val clickedMenuItem = listAdapter.getItem(position) as MainMenuItem
            clickedMenuItem.clickHandler.onClicked(this@MainActivity)
        }

//        val button = findViewById<Button>(R.id.firebaseButton)
//        button.setOnClickListener {
//            FirebaseMessaging.getInstance().token.addOnCompleteListener(OnCompleteListener { task ->
//                if (!task.isSuccessful) {
//                    Toast.makeText(this, "Shit", Toast.LENGTH_SHORT).show()
//                    return@OnCompleteListener
//                }
//
//                val token = task.result
//
//                Toast.makeText(this, token, Toast.LENGTH_SHORT).show()
//                Log.w("LOL", token ?: "ooops")
//
//            })
//        }
    }
}