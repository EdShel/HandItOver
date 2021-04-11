package ua.nure.sheliemietiev.handitover.views.menuItems

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.ImageView
import android.widget.TextView
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.api.AuthorizationInfo
import ua.nure.sheliemietiev.handitover.views.LoginActivity
import kotlin.reflect.KClass

interface MenuItemClickStrategy{
    fun onClicked(context: Activity)
}

open class NextActivityStrategy(private val activityClass: KClass<*>) : MenuItemClickStrategy {
    override fun onClicked(context: Activity) {
        val nextIntent = Intent(context, activityClass.java)
        context.startActivity(nextIntent)
    }
}

class LogoutStrategy(private val authInfo: AuthorizationInfo)
    : NextActivityStrategy(LoginActivity::class) {
    override fun onClicked(context: Activity) {
        authInfo.deauthorize()
        context.finish()
        super.onClicked(context)
    }
}

class MainMenuItem(
    val text: String,
    val imageId: Int,
    val clickHandler: MenuItemClickStrategy
)

class MenuItemsAdapter(
    private val context: Context,
    private val menuItemsList: List<MainMenuItem>
) : BaseAdapter() {

    private val inflater: LayoutInflater = context.getSystemService(
        Context.LAYOUT_INFLATER_SERVICE
    ) as LayoutInflater

    override fun getCount(): Int {
        return menuItemsList.count()
    }

    override fun getItem(position: Int): MainMenuItem {
        return menuItemsList[position]
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val row = convertView ?: inflater.inflate(
            R.layout.main_menu_item, parent, false
        )
        val currentMenuItem =  menuItemsList[position];

        row.findViewById<TextView>(
            R.id.menu_item_text
        ).text = currentMenuItem.text

        row.findViewById<ImageView>(
            R.id.menu_item_image
        ).setImageResource(currentMenuItem.imageId)

        return row
    }
}