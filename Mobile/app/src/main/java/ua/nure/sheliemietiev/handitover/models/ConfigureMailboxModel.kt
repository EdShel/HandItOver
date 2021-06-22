package ua.nure.sheliemietiev.handitover.models

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.coroutineScope
import kotlinx.coroutines.supervisorScope
import kotlinx.coroutines.withContext
import okhttp3.OkHttpClient
import okhttp3.Request
import ru.gildor.coroutines.okhttp.await
import ua.nure.sheliemietiev.handitover.api.Api
import ua.nure.sheliemietiev.handitover.api.ApiResponse
import ua.nure.sheliemietiev.handitover.util.OperationResult
import java.util.regex.Pattern
import javax.inject.Inject

const val mailboxIp = "http://192.168.4.1"

data class MailboxNetwork(val ssid: String)

class ConfigureMailboxModel @Inject constructor(
    private val api: Api
) {
    private val client: OkHttpClient = OkHttpClient()

    suspend fun getAvailableNetworks(): OperationResult<Iterable<MailboxNetwork>> {
        return withContext(Dispatchers.IO) {
            val networksPageRequest = Request.Builder()
                .get()
                .url("$mailboxIp/wifi?")
                .build()

            val response = ApiResponse(client.newCall(networksPageRequest).await())
            if (!response.isSuccessful) {
                return@withContext OperationResult.error<Iterable<MailboxNetwork>>()
            }

            val html = response.body
            val wifiNamePattern =
                Pattern.compile("<a href='#p' onclick='c\\(this\\)'>([^<]+?)</a>")
            val matcher = wifiNamePattern.matcher(html)
            val networksList = ArrayList<MailboxNetwork>()
            while (matcher.find()) {
                val networkName = matcher.group(1)!!
                networksList.add(MailboxNetwork(networkName))
            }
            return@withContext OperationResult.success(networksList)
        }
    }

    suspend fun chooseNetwork(ssid: String, password: String, accessToken: String): Boolean {
        return withContext(Dispatchers.IO) {
            val setWifiRequest = Request.Builder()
                .get()
                .url("$mailboxIp/wifisave?s=$ssid&p=$password&token=$accessToken")
                .build()

            val response = ApiResponse(client.newCall(setWifiRequest).await())
            return@withContext response.isSuccessful
        }
    }

    suspend fun getSmartMailboxAccessToken(macAddress: String, size: Int, address: String): String {
        val response = api.post(
            "mailbox/authorize", null, mapOf<String, Any>(
                "physicalId" to macAddress,
                "size" to size,
                "address" to address
            )
        )

        val json = response.asJsonMap()
        return json["authToken"].asString
    }
}