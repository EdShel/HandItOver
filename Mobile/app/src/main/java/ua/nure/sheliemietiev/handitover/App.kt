package ua.nure.sheliemietiev.handitover

import android.app.Application
import ua.nure.sheliemietiev.handitover.util.StorageModule

class App : Application() {

    val components: ApplicationComponent = DaggerApplicationComponent.builder()
        .storageModule(StorageModule(this))
        .build();

    override fun onCreate() {
        super.onCreate()
    }
}