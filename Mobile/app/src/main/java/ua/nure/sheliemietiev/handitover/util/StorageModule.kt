package ua.nure.sheliemietiev.handitover.util

import android.content.Context
import android.content.SharedPreferences
import dagger.Module
import dagger.Provides

@Module
class StorageModule(private val context: Context) {

    @Provides
    fun provideContext(): Context {
        return context
    }

    @Provides
    fun provideSharedPreferences(context: Context): SharedPreferences {
        return context.getSharedPreferences(
            "HandItOver",
            Context.MODE_PRIVATE
        )
    }
}