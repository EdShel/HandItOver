package ua.nure.sheliemietiev.handitover

import android.app.NotificationManager
import androidx.core.app.NotificationCompat
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage
import java.lang.IllegalStateException


class AppFirebaseMessagingService : FirebaseMessagingService() {

    override fun onNewToken(p0: String) {
    }

    override fun onMessageReceived(message: RemoteMessage) {
        val messageType = message.data["messageKey"] ?: throw IllegalStateException();
        val messageTitle = when(messageType){
            "deliveryArrived" -> R.string.notify_delivery_arrived_title
            "deliveryStolen" -> R.string.notify_delivery_stolen_title
            "deliveryExpired" -> R.string.notify_delivery_expired_title
            else -> R.string.unknown
        }
        val messageBody = when(messageType){
            "deliveryArrived" -> R.string.notify_delivery_arrived_body
            "deliveryStolen" -> R.string.notify_delivery_stolen_body
            "deliveryExpired" -> R.string.notify_delivery_expired_body
            else -> R.string.unknown
        }

        val builder = NotificationCompat.Builder(this, "HandItOverNotifications")
            .setSmallIcon(android.R.drawable.ic_dialog_alert)
            .setContentTitle(getString(messageTitle))
            .setContentText(getString(messageBody))

        val notification = builder.build()

        val notificationManager = getSystemService(NOTIFICATION_SERVICE) as NotificationManager
        notificationManager.notify(1, notification)
    }
}