package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.api.Api
import ua.nure.sheliemietiev.handitover.util.OperationResult
import javax.inject.Inject

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