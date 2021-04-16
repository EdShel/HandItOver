package ua.nure.sheliemietiev.handitover.views.deliveryItems

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import androidx.annotation.IdRes
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.models.ActiveDelivery
import ua.nure.sheliemietiev.handitover.util.*
import java.util.*

class DeliveriesListAdapter(
    private val context: Context,
    private var deliveriesList: List<ActiveDelivery>
) : BaseAdapter() {

    private val inflater: LayoutInflater = context.getSystemService(
        Context.LAYOUT_INFLATER_SERVICE
    ) as LayoutInflater

    fun setDeliveriesList(list: List<ActiveDelivery>){
        deliveriesList = list;
        notifyDataSetChanged()
    }

    override fun getCount(): Int {
        return deliveriesList.count()
    }

    override fun getItem(position: Int): ActiveDelivery {
        return deliveriesList[position]
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val row = convertView ?: inflater.inflate(
            R.layout.deliveries_list_item, parent, false
        )
        val currentDelivery = deliveriesList[position]
        setTextFor(
            row,
            R.id.arrived_time_label,
            context.getString(R.string.arrived_at),
            currentDelivery.arrived.toLocaleDateTimeString(context)
        )

        setTextFor(
            row,
            R.id.terminal_time_label,
            context.getString(R.string.terminal_time_at),
            currentDelivery.terminalTime.toLocaleDateTimeString(context)
        )

        setTextFor(
            row,
            R.id.weight_label,
            context.getString(R.string.weight_is),
            localizedWeightString(context, currentDelivery.weight)
        )

        setTextForTimeLeftLabel(currentDelivery, row)

        return row
    }

    private fun setTextForTimeLeftLabel(
        currentDelivery: ActiveDelivery,
        row: View
    ) {
        val now = Date(currentTimeUtc());
        val textFormat: String
        val hoursDifference: Int
        if (now.after(currentDelivery.terminalTime)) {
            textFormat = context.getString(R.string.late_for_hour)
            hoursDifference = now.differenceHours(currentDelivery.terminalTime)
        } else {
            textFormat = context.getString(R.string.left_hours)
            hoursDifference = currentDelivery.terminalTime.differenceHours(now)
        }
        setTextFor(
            row,
            R.id.time_left_label,
            textFormat,
            hoursDifference
        )
    }

    private fun setTextFor(parent: View, @IdRes labelId: Int, format: String, value: Any) {
        parent.findViewById<TextView>(labelId).text = String.format(format, value);
    }
}