package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.api.Api
import ua.nure.sheliemietiev.handitover.util.OperationResult
import javax.inject.Inject

class MailboxData(
    val macAddress: String,
    val size: Int,
    val address: String
)

class AuthorizedMailbox(
    val mailboxId: String,
    val accessToken: String,
    val refreshToken: String
)

class WifiStation(
    val SSID: String,
    val BSSID: String
)

class ChooseWifiModel @Inject constructor(
    private val api: Api
) {
    suspend fun getMailboxAuth(mailbox: MailboxData) : OperationResult<AuthorizedMailbox> {
        val response = api.post("/mailbox/authorize", null, mapOf(
            "physicalId" to mailbox.macAddress,
            "size" to mailbox.size,
            "address" to mailbox.address
        ))
        if (!response.isSuccessful){
            return OperationResult.error()
        }

        val json = response.asJsonMap()
        return OperationResult.success(AuthorizedMailbox(
            json["mailboxId"].asString,
            json["authToken"].asString,
            json["refreshToken"].asString
        ))
    }

    suspend fun getVisibleWifiStations() {

    }
}