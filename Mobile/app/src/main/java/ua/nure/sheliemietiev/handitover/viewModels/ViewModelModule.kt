package ua.nure.sheliemietiev.handitover.viewModels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import dagger.Binds
import dagger.Module
import dagger.multibindings.IntoMap
import ua.nure.sheliemietiev.handitover.util.ViewModelFactory
import ua.nure.sheliemietiev.handitover.util.ViewModelKey

@Module
abstract class ViewModelModule {
    @Binds
    internal abstract fun bindViewModelFactory(factory: ViewModelFactory): ViewModelProvider.Factory

    @Binds
    @IntoMap
    @ViewModelKey(LoginViewModel::class)
    internal abstract fun provideLoginViewModel(viewModel: LoginViewModel): ViewModel

    @Binds
    @IntoMap
    @ViewModelKey(DeliveriesViewModel::class)
    internal abstract fun provideDeliveriesViewModel(viewModel: DeliveriesViewModel): ViewModel

    @Binds
    @IntoMap
    @ViewModelKey(DeliveryViewModel::class)
    internal abstract fun provideDeliveryViewModel(viewModel: DeliveryViewModel): ViewModel

    @Binds
    @IntoMap
    @ViewModelKey(FirebaseViewModel::class)
    internal abstract fun provideFirebaseViewModel(viewModel: FirebaseViewModel): ViewModel

    @Binds
    @IntoMap
    @ViewModelKey(ConnectMailboxViewModel::class)
    internal abstract fun provideConnectMailboxViewModel(viewModel: ConnectMailboxViewModel): ViewModel

    @Binds
    @IntoMap
    @ViewModelKey(ConfigureMailboxViewModel::class)
    internal abstract fun provideConfigureMailboxViewModel(viewModel: ConfigureMailboxViewModel): ViewModel
}