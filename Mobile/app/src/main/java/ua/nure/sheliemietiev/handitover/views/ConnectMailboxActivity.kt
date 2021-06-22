package ua.nure.sheliemietiev.handitover.views

import android.Manifest
import android.content.*
import android.net.wifi.WifiManager
import android.os.Bundle
import android.view.View
import android.widget.ListView
import android.widget.ProgressBar
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.util.LocationServicesEnabler
import ua.nure.sheliemietiev.handitover.util.WifiConnector
import ua.nure.sheliemietiev.handitover.util.SeverePermissionsEnabler
import ua.nure.sheliemietiev.handitover.viewModels.ConnectMailboxViewModel
import ua.nure.sheliemietiev.handitover.views.accessPointItems.AccessPointAdapter
import javax.inject.Inject

class ConnectMailboxActivity : AppCompatActivity() {
    @Inject
    lateinit var viewModelFactory: ViewModelProvider.Factory

    private lateinit var connectMailboxViewModel: ConnectMailboxViewModel

    private lateinit var accessPointsListView: ListView

    private lateinit var loadingBar: ProgressBar

    private lateinit var locationServicesEnabler: LocationServicesEnabler

    private lateinit var severePermissionsEnabler: SeverePermissionsEnabler

    private var wifiManager: WifiManager? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_connect_mailbox)
        connectMailboxViewModel = ViewModelProvider(this, viewModelFactory)
            .get(ConnectMailboxViewModel::class.java)

        accessPointsListView = findViewById(R.id.devices_list)
        loadingBar = findViewById(R.id.loading)

        askForPermissions()

        val accessPointAdapter =
            AccessPointAdapter(this, connectMailboxViewModel.accessPoints.value!!)
        accessPointsListView.adapter = accessPointAdapter
        accessPointsListView.setOnItemClickListener { _, _, position, _ ->
            val accessPoint = accessPointAdapter.getItem(position)
            val intent = Intent(this, ConfigureMailboxActivity::class.java)
            intent.putExtra("mac", accessPoint.BSSID)
            intent.putExtra("ssid", accessPoint.SSID)
            intent.putExtra("networkCapabilities", accessPoint.capabilities)
            startActivity(intent)
        }
        connectMailboxViewModel.accessPoints.observe(this, Observer {
            accessPointAdapter.notifyDataSetChanged()
        })

        hideLoading()
    }

    private fun askForPermissions() {
        severePermissionsEnabler = SeverePermissionsEnabler(
            this,
            listOf(Manifest.permission.ACCESS_FINE_LOCATION)
        )
        locationServicesEnabler = LocationServicesEnabler(this, severePermissionsEnabler)
        locationServicesEnabler.checkPermission(
            { allPermissionsGranted() },
            { permissionsDenied() }
        )
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        severePermissionsEnabler.acceptPermissionResult(requestCode, grantResults)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        locationServicesEnabler.onActivityResult(requestCode, resultCode)
    }

    private fun allPermissionsGranted() {
        startDiscoveringWifiNetworks()
    }

    private fun permissionsDenied() {
        Toast.makeText(
            this,
            getString(R.string.allow_location),
            Toast.LENGTH_LONG
        ).show()
        finish()
    }

    @Suppress("DEPRECATION")
    private fun startDiscoveringWifiNetworks() {
        showLoading()

        val wifi = applicationContext.getSystemService(Context.WIFI_SERVICE) as WifiManager
        wifiManager = wifi
        registerReceiver(
            object : BroadcastReceiver() {
                override fun onReceive(context: Context?, intent: Intent?) {
                    if (intent?.action == WifiManager.SCAN_RESULTS_AVAILABLE_ACTION) {
                        val scanResults = wifi.scanResults
                        connectMailboxViewModel.addAccessPoints(scanResults)
                        hideLoading()
                    }
                }
            },
            IntentFilter(WifiManager.SCAN_RESULTS_AVAILABLE_ACTION)
        )
        wifi.startScan();
    }

    private fun showLoading() {
        loadingBar.visibility = View.VISIBLE
    }

    private fun hideLoading() {
        loadingBar.visibility = View.INVISIBLE
    }
}