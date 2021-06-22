@file:Suppress("DEPRECATION")

package ua.nure.sheliemietiev.handitover.util

import android.annotation.SuppressLint
import android.net.wifi.WifiConfiguration
import android.net.wifi.WifiManager

const val mailboxPassword = "password";

class WifiConnector(private val wifiManager: WifiManager) {
    @SuppressLint("MissingPermission")
    fun connect(networkSSID: String, networkCapabilities: String) {
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

        wifiManager.addNetwork(wifiConfig)

        val network = wifiManager.configuredNetworks.find { network -> network.SSID == wifiConfig.SSID }


        wifiManager.disconnect()
        wifiManager.enableNetwork(network!!.networkId, true)
        wifiManager.reconnect()
    }


}
