package ua.nure.sheliemietiev.handitover

import android.content.Context
import dagger.Component
import ua.nure.sheliemietiev.handitover.api.ApiModule
import ua.nure.sheliemietiev.handitover.util.StorageModule
import ua.nure.sheliemietiev.handitover.viewModels.ViewModelModule
import ua.nure.sheliemietiev.handitover.views.DeliveriesActivity
import ua.nure.sheliemietiev.handitover.views.LoginActivity
import ua.nure.sheliemietiev.handitover.views.MainActivity
import javax.inject.Singleton

@Singleton
@Component(
    modules = [
        StorageModule::class,
        ApiModule::class,
        ViewModelModule::class
    ]
)
interface ApplicationComponent {
    fun context(): Context

    fun inject(mainActivity: MainActivity)

    fun inject(deliveriesActivity: DeliveriesActivity)

    fun inject(loginActivity: LoginActivity)
}