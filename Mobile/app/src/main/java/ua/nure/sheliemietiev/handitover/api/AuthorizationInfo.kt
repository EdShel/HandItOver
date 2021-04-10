package ua.nure.sheliemietiev.handitover.api

import ua.nure.sheliemietiev.handitover.util.SecureStorage
import javax.inject.Singleton

@Singleton
class AuthorizationInfo(
    private val storage: SecureStorage
) {
    var fullName: String = ""
        private set

    var email: String = ""
        private set

    var token: String = ""
        private set

    var refreshToken: String = ""
        private set

    val isAuthorized: Boolean
        get() {
            return token.isNotBlank()
        }

    fun loadFromStorage() {
        fullName = storage.getStringOrNull("fullName") ?: ""
        email = storage.getStringOrNull("email") ?: ""
        token = storage.getStringOrNull("token") ?: ""
        refreshToken = storage.getStringOrNull("refreshToken") ?: ""
    }

    fun setUser(fullName : String, email : String, token : String, refreshToken : String){
        this.fullName = fullName
        this.email = email

        storage.saveString("fullName", fullName)
        storage.saveString("email", email)

        changeAuth(token, refreshToken)
    }

    fun changeAuth(newToken : String, newRefresh : String){
        token = newToken
        refreshToken = newRefresh

        storage.saveString("token", newToken)
        storage.saveString("refreshToken", newRefresh)
    }

    fun deauthorize(){
        changeAuth("", "")
    }
}