package ua.nure.sheliemietiev.handitover.views

import android.Manifest
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.content.pm.PackageManager
import android.net.wifi.WifiManager
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.ListView
import android.widget.ProgressBar
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.viewModels.ConnectMailboxViewModel
import ua.nure.sheliemietiev.handitover.views.accessPointItems.AccessPointAdapter
import javax.inject.Inject

private const val ACCESS_FINE_LOCATION_ALLOWED_CODE = 1

class ConnectMailboxActivity : AppCompatActivity() {
    @Inject
    lateinit var viewModelFactory: ViewModelProvider.Factory

    private lateinit var connectMailboxViewModel: ConnectMailboxViewModel

    private lateinit var accessPointsListView: ListView

    private lateinit var loadingBar: ProgressBar

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
        connectMailboxViewModel.accessPoints.observe(this, Observer {
            accessPointAdapter.notifyDataSetChanged()
        })
    }

    private fun askForPermissions() {
        if (ContextCompat.checkSelfPermission(
                this,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            if (ActivityCompat.shouldShowRequestPermissionRationale(
                    this,
                    Manifest.permission.ACCESS_FINE_LOCATION
                )
            ) {
                ActivityCompat.requestPermissions(
                    this,
                    arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                    ACCESS_FINE_LOCATION_ALLOWED_CODE
                )
            } else {
                permissionsDenied()
            }
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            ACCESS_FINE_LOCATION_ALLOWED_CODE -> {
                if (grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    locationAccessGranted()
                } else {
                    permissionsDenied()
                }
            }
        }
    }

    private fun locationAccessGranted() {
        startDiscoveringWifiNetworks()
    }

    private fun permissionsDenied() {
        // TODO: replace with localized text
        Toast.makeText(
            this,
            "Allow location permission",
            Toast.LENGTH_LONG
        ).show()
        finish()
    }

    private fun startDiscoveringWifiNetworks() {
        showLoading()

        val wifiManager = applicationContext.getSystemService(Context.WIFI_SERVICE) as WifiManager
        registerReceiver(
            object : BroadcastReceiver() {
                override fun onReceive(context: Context?, intent: Intent?) {
                    if (intent?.action == WifiManager.SCAN_RESULTS_AVAILABLE_ACTION) {
                        val scanResults = wifiManager.scanResults
                        connectMailboxViewModel.addAccessPoints(scanResults)
                    }
                }
            },
            IntentFilter(WifiManager.SCAN_RESULTS_AVAILABLE_ACTION).apply {
                addAction(WifiManager.SCAN_RESULTS_AVAILABLE_ACTION)
            }
        )
        wifiManager.startScan();
    }

    private fun showLoading() {
        loadingBar.visibility = View.VISIBLE
    }

    private fun hideLoading() {
        loadingBar.visibility = View.INVISIBLE
    }
}