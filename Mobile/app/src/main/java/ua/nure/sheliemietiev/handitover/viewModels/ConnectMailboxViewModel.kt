package ua.nure.sheliemietiev.handitover.viewModels

import android.net.wifi.ScanResult
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import javax.inject.Inject

class ConnectMailboxViewModel @Inject constructor(
) : ViewModel() {
    private val accessPointsList: MutableList<ScanResult> = arrayListOf()

    private val _accessPoints =
        MutableLiveData<List<ScanResult>>().apply { value = accessPointsList }
    val accessPoints: LiveData<List<ScanResult>> get() = _accessPoints

    fun addAccessPoints(newAccessPoints: Iterable<ScanResult>) {
        val notAddedAccessPoints = newAccessPoints.filter { point ->
            accessPointsList.any { saved -> saved.SSID == point.SSID }
        }
        accessPointsList.addAll(notAddedAccessPoints)
        _accessPoints.value = _accessPoints.value
    }

}