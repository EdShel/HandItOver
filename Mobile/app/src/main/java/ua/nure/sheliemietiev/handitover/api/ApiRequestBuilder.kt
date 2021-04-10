package ua.nure.sheliemietiev.handitover.api

import com.google.gson.Gson
import okhttp3.HttpUrl
import okhttp3.MediaType
import okhttp3.Request
import okhttp3.RequestBody
import javax.inject.Inject

class ApiRequestBuilder @Inject constructor(
    private val apiUrl: String,
    private val authorization: AuthorizationInfo
) {
    fun buildPostRequest(
        relativeUrl: String,
        queryParams: Map<String, String>?,
        body: Any?
    ): Request {
        val url = buildUrl(relativeUrl, queryParams)

        val bodyJson = Gson().toJson(body)
        val requestBuilder = Request.Builder()
            .url(url)
            .post(
                RequestBody.create(
                    MediaType.parse(
                        "application/json"
                    ), bodyJson
                )
            )

        if (authorization.isAuthorized) {
            requestBuilder.addHeader("Authorization", "Bearer ${authorization.token}")
        }

        return requestBuilder.build()
    }

    fun buildGetRequest(
        relativeUrl: String,
        queryParams: Map<String, String>?
    ): Request {
        val url = buildUrl(relativeUrl, queryParams)

        val requestBuilder = Request.Builder().url(url).get()

        if (authorization.isAuthorized) {
            requestBuilder.addHeader("Authorization", "Bearer ${authorization.token}")
        }

        return requestBuilder.build()
    }

    fun buildRefreshRequest(): Request {
        return buildPostRequest(
            "auth/refresh", null, mapOf(
                "refreshToken" to authorization.refreshToken
            )
        )
    }

    private fun buildUrl(
        relativeUrl: String,
        queryParams: Map<String, String>?
    ): HttpUrl {
        val urlBuilder = HttpUrl.parse("$apiUrl/$relativeUrl")!!.newBuilder()
        queryParams?.forEach { (name, value) -> urlBuilder.addQueryParameter(name, value) }
        return urlBuilder.build()
    }
}