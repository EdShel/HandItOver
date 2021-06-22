package ua.nure.sheliemietiev.handitover.viewModels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import ua.nure.sheliemietiev.handitover.models.DeliveryModel
import ua.nure.sheliemietiev.handitover.models.MailboxModel
import java.lang.IllegalStateException
import java.util.*
import javax.inject.Inject

class ActiveDeliveryFullInfo(
    val id: String,
    val weight: Float,
    val mailboxId: String,
    val arrived: Date,
    val terminalTime: Date?,
    val mailboxAddress: String,
    val mailboxSize: Int
)

class DeliveryViewModel @Inject constructor(
    private val deliveryModel: DeliveryModel,
    private val mailboxModel: MailboxModel
) : ViewModel() {
    private val _delivery: MutableLiveData<ActiveDeliveryFullInfo>
    val delivery: LiveData<ActiveDeliveryFullInfo> get() = _delivery

    private val _openResult = MutableLiveData<Boolean>()
    val openResult: LiveData<Boolean> get() = _openResult

    var deliveryId: String? = null

    init {
        _delivery = object : MutableLiveData<ActiveDeliveryFullInfo>() {
            override fun onActive() {
                val deliveryIdToFind = deliveryId
                    ?: throw IllegalStateException("Pass in deliveryId before observing the delivery.")
                viewModelScope.launch {
                    val deliveryResult = deliveryModel.getDelivery(deliveryIdToFind)
                    if (!deliveryResult.isSuccess) {
                        return@launch
                    }
                    val delivery = deliveryResult.getOrThrow()
                    val mailboxResult = mailboxModel.getMailbox(delivery.mailboxId)
                    if (!mailboxResult.isSuccess) {
                        return@launch
                    }
                    val mailbox = mailboxResult.getOrThrow()

                    value = ActiveDeliveryFullInfo(
                        id = delivery.id,
                        weight = delivery.weight,
                        mailboxId = mailbox.id,
                        arrived = delivery.arrived,
                        terminalTime = delivery.terminalTime,
                        mailboxAddress = mailbox.address,
                        mailboxSize = mailbox.size
                    )
                }
            }
        }
    }

    fun openMailboxButtonPressed() {
        val deliveryToOpen = deliveryId ?: return
        viewModelScope.launch {
            val openResult = deliveryModel.openMailboxWithDelivery(deliveryToOpen)
            if (openResult.isSuccess) {
                _openResult.value = true
            }
        }
    }
}