package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.api.Api
import javax.inject.Inject

class FirebaseModel @Inject constructor(
    private val api: Api
) {
    suspend fun assertServerKnowsClientToken(token: String): Boolean {
        val result = api.post(
            "firebase/token", null, mapOf(
                "firebaseClientToken" to token
            )
        )
        return result.isSuccessful
    }
}