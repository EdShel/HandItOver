package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.api.Api
import ua.nure.sheliemietiev.handitover.api.StatusCode
import ua.nure.sheliemietiev.handitover.util.OperationResult
import ua.nure.sheliemietiev.handitover.util.iso8601ToDate
import java.util.*
import javax.inject.Inject

class ActiveDelivery(
    val id: String,
    val weight: Float,
    val mailboxId: String,
    val arrived: Date,
    val terminalTime: Date
)

class DeliveriesModel @Inject constructor(private val api: Api) {
    suspend fun getActiveDeliveries(): OperationResult<Iterable<ActiveDelivery>> {
        val response = api.get("delivery/active");
        if (response.statusCode != StatusCode.OK.code) {
            return OperationResult.error()
        }
        val json = response.asJsonArray()
        return OperationResult.success(json.map {
            val delivery = it.asJsonObject
            ActiveDelivery(
                id = delivery["id"].asString,
                weight = delivery["weight"].asFloat,
                mailboxId = delivery["mailboxId"].asString,
                arrived = iso8601ToDate(delivery["arrived"].asString),
                terminalTime = iso8601ToDate(delivery["terminalTime"].asString)
            )
        })
    }
}