package ua.nure.sheliemietiev.handitover.views

import android.content.Context
import android.net.wifi.SupplicantState
import android.net.wifi.WifiManager
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.EditText
import android.widget.ListView
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.util.WifiConnector
import ua.nure.sheliemietiev.handitover.viewModels.ConfigureMailboxViewModel
import ua.nure.sheliemietiev.handitover.views.accessPointItems.MailboxNetworkAdapter
import javax.inject.Inject

class ConfigureMailboxActivity : AppCompatActivity() {
    @Inject
    lateinit var viewModelFactory: ViewModelProvider.Factory

    lateinit var configureMailboxViewModel: ConfigureMailboxViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_configure_mailbox)
        configureMailboxViewModel = ViewModelProvider(this, viewModelFactory)
            .get(ConfigureMailboxViewModel::class.java)

        val networksListView = findViewById<ListView>(R.id.devices_list)
        val password = findViewById<EditText>(R.id.password)

        val networksAdapter = MailboxNetworkAdapter(this, emptyList())
        networksListView.adapter = networksAdapter
        networksListView.setOnItemClickListener { _, _, position, _ ->
            val network = networksAdapter.getItem(position)
            configureMailboxViewModel.setConfiguration(network.ssid, password.text.toString())
        }

        val extras = intent.extras!!
        val macAddress = extras.getString("mac")!!
        val ssid = extras.getString("ssid")!!
        val networkCapabilities = extras.getString("networkCapabilities")!!

        configureMailboxViewModel.loadAccessToken(macAddress)

        configureMailboxViewModel.mailboxAccessToken.observe(this, Observer { token ->
            object : Thread() {
                override fun run() {
                    val wifiManager =
                        applicationContext.getSystemService(Context.WIFI_SERVICE) as WifiManager
                    WifiConnector(wifiManager).connect(ssid, networkCapabilities)
                    sleep(1000)
                    while (true) {
                        val connectionStatus = wifiManager.connectionInfo.supplicantState
                        if (connectionStatus == SupplicantState.COMPLETED) {
                            sleep(1000)
                            break
                        }
                        sleep(500)
                    }

                    configureMailboxViewModel.loadNetworks()
                }
            }.start()
        })

        configureMailboxViewModel.networksList.observe(this, Observer { networks ->
            networksAdapter.setNetworksList(networks)
        })

        configureMailboxViewModel.mailboxConfigured.observe(this, Observer {
            finish()
        })
    }

}