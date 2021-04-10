package ua.nure.sheliemietiev.handitover

import android.content.Context
import dagger.Component
import ua.nure.sheliemietiev.handitover.api.ApiModule
import ua.nure.sheliemietiev.handitover.util.StorageModule
import ua.nure.sheliemietiev.handitover.views.LoginActivity
import javax.inject.Singleton

@Singleton
@Component(
    modules = [
        StorageModule::class,
        ApiModule::class
    ]
)
interface ApplicationComponent {
    fun context(): Context

    fun inject(mainActivity: MainActivity)

    fun inject(loginActivity: LoginActivity)
}