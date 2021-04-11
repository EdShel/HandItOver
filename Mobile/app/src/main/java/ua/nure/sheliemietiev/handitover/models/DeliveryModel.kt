package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.api.Api
import ua.nure.sheliemietiev.handitover.api.StatusCode
import ua.nure.sheliemietiev.handitover.util.OperationResult
import ua.nure.sheliemietiev.handitover.util.iso8601ToDate
import javax.inject.Inject

class DeliveryModel @Inject constructor(
    private val api: Api
) {
    suspend fun getDelivery(deliveryId: String): OperationResult<ActiveDelivery> {
        val response = api.get("delivery/$deliveryId")
        if (!response.isSuccessful) {
            return OperationResult.error()
        }

        val json = response.asJsonMap()
        return OperationResult.success(
            ActiveDelivery(
                id = json["id"].asString,
                weight = json["weight"].asFloat,
                mailboxId = json["mailboxId"].asString,
                arrived = iso8601ToDate(json["arrived"].asString),
                terminalTime = iso8601ToDate(json["terminalTime"].asString)
            )
        )
    }
}

class Mailbox(
    val id: String,
    val ownerId: String,
    val size: Int,
    val groupId: String,
    val address: String
)

class MailboxModel @Inject constructor(
    private val api: Api
) {
    suspend fun getMailbox(mailboxId: String): OperationResult<Mailbox> {
        val response = api.get("mailbox/$mailboxId")
        if (!response.isSuccessful){
            return OperationResult.error()
        }

        val json = response.asJsonMap()
        return OperationResult.success(
            Mailbox(
                id = json["id"].asString,
                ownerId = json["ownerId"].asString,
                size = json["size"].asInt,
                groupId = json["groupId"].asString,
                address = json["address"].asString
            )
        )
    }
}