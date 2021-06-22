package ua.nure.sheliemietiev.handitover.views.accessPointItems

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.models.MailboxNetwork

class MailboxNetworkAdapter(
    private val context: Context,
    private var networksList: List<MailboxNetwork>
) : BaseAdapter() {

    private val inflater: LayoutInflater = context.getSystemService(
        Context.LAYOUT_INFLATER_SERVICE
    ) as LayoutInflater

    fun setNetworksList(networks: Iterable<MailboxNetwork>) {
        networksList = networks.toList()
        notifyDataSetChanged()
    }

    override fun getCount(): Int {
        return networksList.count()
    }

    override fun getItem(position: Int): MailboxNetwork {
        return networksList[position]
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val row = convertView ?: inflater.inflate(
            R.layout.access_point_item, parent, false
        )
        val currentScanResult = networksList[position]
        val wifiNetworkName = row.findViewById<TextView>(R.id.wifi_name)
        wifiNetworkName.text = currentScanResult.ssid

        return row
    }
}