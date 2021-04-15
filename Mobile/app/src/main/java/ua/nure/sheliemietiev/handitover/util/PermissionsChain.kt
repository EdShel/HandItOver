package ua.nure.sheliemietiev.handitover.util

import android.Manifest
import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.location.LocationManager
import android.provider.Settings
import androidx.appcompat.app.AlertDialog
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat

interface PermissionChecker {
    fun checkPermission(onAllow: () -> Unit, onDeny: () -> Unit)
}

abstract class PermissionsChain(var next: PermissionChecker? = null) : PermissionChecker {
    private var onAllow: (() -> Unit)? = null
    private var onDeny: (() -> Unit)? = null

    final override fun checkPermission(onAllow: () -> Unit, onDeny: () -> Unit) {
        this.onAllow = onAllow
        this.onDeny = onDeny

        checkPermissions()
    }

    protected abstract fun checkPermissions()

    protected fun allowed() {
        val onAllowCallback = onAllow
        val onDenyCallback = onDeny
        if (onAllowCallback != null && onDenyCallback != null) {
            val nextChecker = next
            if (nextChecker == null) {
                onAllow?.invoke()
                return
            }
            nextChecker.checkPermission(onAllowCallback, onDenyCallback)
        }
    }

    protected fun denied() {
        onDeny?.invoke()
    }
}

private const val PERMISSION_LISTENER_CODE = 1

private const val LOCATION_SERVICE_ENABLE_CODE = 2

class LocationServicesEnabler(
    private val activity: Activity,
    next: PermissionChecker? = null
) :
    PermissionsChain(next) {

    override fun checkPermissions() {
        if (isLocationServicesEnabled()) {
            allowed()
            return
        }

        askToEnableLocationServices()
    }

    private fun isLocationServicesEnabled(): Boolean {
        val locationManager = activity.getSystemService(Context.LOCATION_SERVICE) as LocationManager
        return locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)
    }

    private fun askToEnableLocationServices() {
        // TODO: localize messages
        val builder = AlertDialog.Builder(activity).apply {
            setMessage("To find mailbox you need to enable location services")
            setCancelable(false)
            setPositiveButton("Ok") { _, _ ->
                activity.startActivityForResult(
                    Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),
                    LOCATION_SERVICE_ENABLE_CODE
                )
            }
            setNegativeButton("Cancel") { _, _ -> denied() }
        }
        builder.create().show()
    }

    fun onActivityResult(requestCode: Int, resultCode: Int) {
        if (requestCode != LOCATION_SERVICE_ENABLE_CODE) {
            return
        }
        if (isLocationServicesEnabled()) {
            return allowed()
        } else {
            denied()
        }
    }
}

class SeverePermissionsEnabler(
    private val activity: Activity,
    private val permissions: Iterable<String>,
    next: PermissionChecker? = null
) : PermissionsChain(next) {
    override fun checkPermissions() {
        if (areAllPermissionsAllowed()) return

        val allPermissionsCanBeAsked = permissions.all { permission ->
            ActivityCompat.shouldShowRequestPermissionRationale(
                activity,
                permission
            )
        }
        if (allPermissionsCanBeAsked) {
            ActivityCompat.requestPermissions(
                activity,
                arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                PERMISSION_LISTENER_CODE
            )
        } else {
            denied()
        }
    }

    private fun areAllPermissionsAllowed(): Boolean {
        val deniedPermissions = permissions.filterNot { permission ->
            ContextCompat.checkSelfPermission(
                activity,
                permission
            ) == PackageManager.PERMISSION_GRANTED
        }

        if (!deniedPermissions.any()) {
            allowed()
            return true
        }
        return false
    }

    fun acceptPermissionResult(
        requestCode: Int,
        grantResults: IntArray
    ) {
        if (requestCode == PERMISSION_LISTENER_CODE) {
            if (grantResults.all { r -> r == PackageManager.PERMISSION_GRANTED }) {
                allowed()
            } else {
                denied()
            }
        }
    }
}