package ua.nure.sheliemietiev.handitover

import android.util.Log
import com.google.firebase.messaging.FirebaseMessagingService

class AppFirebaseMessagingService : FirebaseMessagingService() {
    override fun onNewToken(token: String) {
        Log.w("LOL", token)
    }
}