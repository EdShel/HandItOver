package ua.nure.sheliemietiev.handitover.views

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.lifecycle.ViewModelProvider
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.customViews.CircleButton
import ua.nure.sheliemietiev.handitover.viewModels.DeliveryViewModel
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

        openButton = findViewById(R.id.open_button)
        openButton.setLongClickListener{btn ->
            btn.canClick = false
            // TODO: replace with string resource
            btn.text = "Hold to open the mailbox"
        }
    }
}