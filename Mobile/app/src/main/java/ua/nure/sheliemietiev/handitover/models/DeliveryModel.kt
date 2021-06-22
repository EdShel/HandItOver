package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.api.Api
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
                terminalTime = if (!json["terminalTime"].isJsonNull) iso8601ToDate(json["terminalTime"].asString) else null
            )
        )
    }

    suspend fun openMailboxWithDelivery(deliveryId: String): OperationResult<Boolean> {
        val response = api.post("delivery/$deliveryId/open")
        if (!response.isSuccessful) {
            return OperationResult.error()
        }

        return OperationResult.success(true)
    }
}

