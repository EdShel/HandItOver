@file:Suppress("DEPRECATION")

package ua.nure.sheliemietiev.handitover.util

import android.annotation.SuppressLint
import android.net.wifi.WifiConfiguration
import android.net.wifi.WifiManager
import java.lang.Exception

const val mailboxPassword = "";

class MailboxInitializer(private val wifiManager: WifiManager) {
    @SuppressLint("MissingPermission")
    fun connect(networkCapabilities: String, networkSSID: String) {
        val wifiConfig = WifiConfiguration()
        wifiConfig.SSID = "\"${networkSSID}\""

        when {
            networkCapabilities.toUpperCase().contains("WEP") -> {
                wifiConfig.apply {
                    wepKeys[0] = "\"${mailboxPassword}\""
                    wepTxKeyIndex = 0
                    allowedKeyManagement.set(WifiConfiguration.KeyMgmt.NONE)
                    allowedGroupCiphers.set(WifiConfiguration.GroupCipher.WEP40)
                }
            }
            networkCapabilities.toUpperCase().contains("WPA") -> {
                wifiConfig.preSharedKey = "\"${mailboxPassword}\""
            }
            else -> {
                wifiConfig.allowedKeyManagement.set(WifiConfiguration.KeyMgmt.NONE)
            }
        }

        val networkId = wifiManager.addNetwork(wifiConfig)

        wifiManager.disconnect()
        wifiManager.enableNetwork(networkId, true)
        wifiManager.reconnect()
    }


}
