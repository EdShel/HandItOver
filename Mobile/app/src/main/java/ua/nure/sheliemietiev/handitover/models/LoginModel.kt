package ua.nure.sheliemietiev.handitover.models

import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.api.Api
import ua.nure.sheliemietiev.handitover.api.ApiResponse
import ua.nure.sheliemietiev.handitover.api.AuthorizationInfo
import ua.nure.sheliemietiev.handitover.api.StatusCode
import ua.nure.sheliemietiev.handitover.util.OperationResult
import javax.inject.Inject

class LoginModel @Inject constructor(
    var api: Api,
    var authInfo: AuthorizationInfo
) {
    suspend fun signIn(email: String, password: String): OperationResult<AuthorizationResult> {
        val response = api.post(
            "auth/login", null, mapOf<String, Any>(
                "email" to email,
                "password" to password
            )
        )

        return when (response.statusCode) {
            StatusCode.OK.code ->
                createSuccessfulAuthorizationResult(response)
            StatusCode.BAD_GATEWAY.code ->
                OperationResult.error(R.string.error_server_not_accessible)
            else ->
                OperationResult.error(R.string.error_invalid_email_or_password)
        }
    }

    private fun createSuccessfulAuthorizationResult(
        response: ApiResponse
    ): OperationResult<AuthorizationResult> {
        val json = response.asJsonMap()
        val email = json["email"].asString
        val fullName = json["fullName"].asString
        val token = json["token"].asString
        val refreshToken = json["refreshToken"].asString
        authInfo.setUser(
            fullName,
            email,
            token,
            refreshToken
        )
        return OperationResult.success(
            AuthorizationResult(
                fullName
            )
        )
    }
}