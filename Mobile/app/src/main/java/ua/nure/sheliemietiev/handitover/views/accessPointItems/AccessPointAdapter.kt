package ua.nure.sheliemietiev.handitover.views.accessPointItems

import android.content.Context
import android.net.wifi.ScanResult
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import ua.nure.sheliemietiev.handitover.R

class AccessPointAdapter(
    private val context: Context,
    private var accessPoints: List<ScanResult>
) : BaseAdapter() {

    private val inflater: LayoutInflater = context.getSystemService(
        Context.LAYOUT_INFLATER_SERVICE
    ) as LayoutInflater

    override fun getCount(): Int {
        return accessPoints.count()
    }

    override fun getItem(position: Int): ScanResult {
        return accessPoints[position]
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val row = convertView ?: inflater.inflate(
            R.layout.access_point_item, parent, false
        )
        val currentScanResult = accessPoints[position]
        val wifiNetworkName = row.findViewById<TextView>(R.id.wifi_name)
        wifiNetworkName.text = currentScanResult.SSID

        return row
    }
}