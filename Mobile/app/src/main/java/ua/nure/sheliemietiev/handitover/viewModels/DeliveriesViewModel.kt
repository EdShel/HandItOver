package ua.nure.sheliemietiev.handitover.viewModels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import ua.nure.sheliemietiev.handitover.models.ActiveDelivery
import ua.nure.sheliemietiev.handitover.models.DeliveriesModel
import javax.inject.Inject

class DeliveriesViewModel @Inject constructor(
    private val deliveriesModel: DeliveriesModel
) : ViewModel() {
    private val _currentDeliveries: MutableLiveData<Iterable<ActiveDelivery>>
    val currentDeliveries: LiveData<Iterable<ActiveDelivery>> get() = _currentDeliveries

    init {
        _currentDeliveries = object : MutableLiveData<Iterable<ActiveDelivery>>() {
            override fun onActive() {
                viewModelScope.launch {
                    val deliveriesResult = deliveriesModel.getActiveDeliveries()
                    value = if (deliveriesResult.isSuccess) {
                        deliveriesResult.getOrThrow()
                    } else {
                        emptyList()
                    }
                }
            }
        }
    }
}