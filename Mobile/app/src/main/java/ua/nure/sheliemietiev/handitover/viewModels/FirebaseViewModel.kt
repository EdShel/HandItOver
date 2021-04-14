package ua.nure.sheliemietiev.handitover.viewModels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import ua.nure.sheliemietiev.handitover.models.FirebaseModel
import ua.nure.sheliemietiev.handitover.util.SecureStorage
import javax.inject.Inject

class FirebaseViewModel @Inject constructor(
    private val firebaseModel: FirebaseModel,
    private val storage: SecureStorage
) : ViewModel() {
    fun onFirebaseTokenReceived(tokenValue: String) {
        viewModelScope.launch {
            val previousToken = storage.getStringOrNull("firebaseToken")
            if (previousToken != null && previousToken == tokenValue) {
                return@launch
            }
            val isSuccessfulSend = firebaseModel.assertServerKnowsClientToken(tokenValue)
            if (isSuccessfulSend) {
                storage.saveString("firebaseToken", tokenValue)
            }
        }
    }
}