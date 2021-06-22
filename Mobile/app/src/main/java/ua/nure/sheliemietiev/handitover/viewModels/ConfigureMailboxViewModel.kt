package ua.nure.sheliemietiev.handitover.viewModels

import android.net.wifi.WifiManager
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import ua.nure.sheliemietiev.handitover.models.ConfigureMailboxModel
import ua.nure.sheliemietiev.handitover.models.MailboxNetwork
import javax.inject.Inject

class ConfigureMailboxViewModel @Inject constructor(
    private val model: ConfigureMailboxModel
) : ViewModel() {
    private val _networksList = MutableLiveData<Iterable<MailboxNetwork>>()
    val networksList: LiveData<Iterable<MailboxNetwork>> get() = _networksList

    private val _mailboxAccessToken = MutableLiveData<String?>()
    val mailboxAccessToken: LiveData<String?> get() = _mailboxAccessToken

    private val _mailboxConfigured = MutableLiveData<Boolean>()
    val mailboxConfigured: LiveData<Boolean> get() = _mailboxConfigured

    fun loadAccessToken(macAddress: String) {
        viewModelScope.launch {
            val token = model.getSmartMailboxAccessToken(macAddress, 1, "Unspecified")
            _mailboxAccessToken.value = token
        }
    }

    fun loadNetworks() {
        viewModelScope.launch {
            val response = model.getAvailableNetworks()
            if (!response.isSuccess) {
                _networksList.value = listOf()
            }

            _networksList.value = response.getOrThrow()
        }
    }

    fun setConfiguration(networkSsid: String, password: String) {
        viewModelScope.launch {
            val accessToken =
                _mailboxAccessToken.value ?: throw Exception("Load access token first.")
            val result = model.chooseNetwork(networkSsid, password, accessToken)
            _mailboxConfigured.value = true
        }
    }

}
