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
}