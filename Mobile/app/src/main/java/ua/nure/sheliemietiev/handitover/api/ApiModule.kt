package ua.nure.sheliemietiev.handitover.api

import dagger.Module
import dagger.Provides
import ua.nure.sheliemietiev.handitover.BuildConfig
import ua.nure.sheliemietiev.handitover.util.SecureStorage

@Module
class ApiModule {
    @Provides
    fun provideAuthorizationInfo(storage : SecureStorage): AuthorizationInfo {
        val authInfo = AuthorizationInfo(storage)
        authInfo.loadFromStorage()
        return authInfo
    }

    @Provides
    fun provideRequestBuilder(authInfo: AuthorizationInfo) : ApiRequestBuilder{
        return ApiRequestBuilder(BuildConfig.API_URL, authInfo)
    }
}