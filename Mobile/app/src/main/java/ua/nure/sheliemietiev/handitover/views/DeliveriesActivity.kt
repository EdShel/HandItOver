package ua.nure.sheliemietiev.handitover.views

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ListView
import android.widget.Toast
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.viewModels.DeliveriesViewModel
import ua.nure.sheliemietiev.handitover.views.deliveryItems.DeliveriesListAdapter
import javax.inject.Inject

class DeliveriesActivity : AppCompatActivity() {
    @Inject lateinit var viewModelFactory: ViewModelProvider.Factory

    private lateinit var deliveriesViewModel: DeliveriesViewModel

    private lateinit var deliveriesListView: ListView

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_deliveries)
        deliveriesViewModel = ViewModelProvider(this, viewModelFactory)
            .get(DeliveriesViewModel::class.java)

        deliveriesListView = findViewById(R.id.deliveries_list)

        initializeDeliveriesList()
        observeActiveDeliveries()
    }

    private fun initializeDeliveriesList() {
        deliveriesListView.adapter = DeliveriesListAdapter(this, emptyList())
        deliveriesListView.setOnItemClickListener{parent, view, position, id ->
            val delivery = (deliveriesListView.adapter as DeliveriesListAdapter).getItem(position)
            val intent = Intent(this, DeliveryActivity::class.java)
            intent.putExtra("deliveryId", delivery.id)
            startActivity(intent)
        }
    }

    private fun observeActiveDeliveries() {
        deliveriesViewModel.currentDeliveries.observe(this, Observer {deliveries ->
            (deliveriesListView.adapter as DeliveriesListAdapter).setDeliveriesList(
                deliveries.toList()
            )
        })
    }
}