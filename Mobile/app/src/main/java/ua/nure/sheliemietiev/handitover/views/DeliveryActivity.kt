package ua.nure.sheliemietiev.handitover.views

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.TextView
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.customViews.CircleButton
import ua.nure.sheliemietiev.handitover.util.localizedWeightString
import ua.nure.sheliemietiev.handitover.util.toLocaleDateTimeString
import ua.nure.sheliemietiev.handitover.viewModels.DeliveryViewModel
import java.util.*
import javax.inject.Inject

class DeliveryActivity : AppCompatActivity() {
    @Inject
    lateinit var viewModelFactory: ViewModelProvider.Factory

    private lateinit var deliveryViewModel: DeliveryViewModel

    private lateinit var openButton: CircleButton

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_delivery)
        deliveryViewModel = ViewModelProvider(this, viewModelFactory)
            .get(DeliveryViewModel::class.java)

        val addressText = findViewById<TextView>(R.id.mailbox_address_value)
        val weightText = findViewById<TextView>(R.id.delivery_weight_value)
        val arrivedText = findViewById<TextView>(R.id.delivery_arrived_value)
        val terminalTimeText = findViewById<TextView>(R.id.delivery_terminal_time_value)

        deliveryViewModel.deliveryId = intent.extras!!.getString("deliveryId")
        deliveryViewModel.delivery.observe(this@DeliveryActivity, Observer { delivery ->
            val activity = this@DeliveryActivity
            addressText.text = delivery.mailboxAddress
            weightText.text = localizedWeightString(activity, delivery.weight)
            arrivedText.text = delivery.arrived.toLocaleDateTimeString(activity)
            terminalTimeText.text = delivery.terminalTime.toLocaleDateTimeString(activity)
        })

        openButton = findViewById(R.id.open_button)
        openButton.setLongClickListener { btn ->
            btn.canClick = false
            // TODO: replace with string resource
            btn.text = "Opened"
            deliveryViewModel.openMailboxButtonPressed()
        }

        deliveryViewModel.openResult.observe(this, Observer {
            val closeAfterMilliseconds = 5000L
            Timer("MovingBack", true).schedule(object: TimerTask() {
                override fun run() {
                    finish()
                }
            }, closeAfterMilliseconds)
        })
    }
}