package ua.nure.sheliemietiev.handitover.viewModels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.models.AuthorizationResult
import ua.nure.sheliemietiev.handitover.models.LoginFormState
import ua.nure.sheliemietiev.handitover.models.LoginModel
import ua.nure.sheliemietiev.handitover.util.OperationResult
import java.util.regex.Pattern
import javax.inject.Inject

const val EMAIL_PATTERN =
    "^(([^<>()\\[\\]\\\\.,;:\\s@\"\"]+(\\.[^<>()\\[\\]\\\\.,;:\\s@\"\"]+)*)|(\"\".+\"\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))\$"

const val PASSWORD_PATTERN = "^.{6,20}\$"

class LoginViewModel @Inject constructor(
    private val loginModel: LoginModel
) : ViewModel() {

    private val _loginFormState = MutableLiveData<LoginFormState>()
    val loginFormState: LiveData<LoginFormState> get() = _loginFormState

    private val _loginResult = MutableLiveData<OperationResult<AuthorizationResult>>()
    val loginResult: LiveData<OperationResult<AuthorizationResult>> get() = _loginResult

    private val emailPattern = Pattern.compile(EMAIL_PATTERN)

    private val passwordPattern = Pattern.compile(PASSWORD_PATTERN)

    init {
        if (loginModel.authInfo.isAuthorized) {
            _loginResult.value = OperationResult.success(
                AuthorizationResult(
                    loginModel.authInfo.fullName
                )
            )
        }
    }

    fun loginButtonPressed(email: String, password: String) {
        viewModelScope.launch {
            val result = loginModel.signIn(email, password)
            _loginResult.value = result
        }
    }

    fun loginDataChanged(email: String, password: String) {
        if (!isEmailValid(email)) {
            _loginFormState.value = LoginFormState(usernameError = R.string.error_invalid_email)
        } else if (!isPasswordValid(password)) {
            _loginFormState.value = LoginFormState(passwordError = R.string.error_invalid_password)
        } else {
            _loginFormState.value = LoginFormState(isDataValid = true)
        }
    }

    private fun isEmailValid(username: String): Boolean {
        return emailPattern.matcher(username).matches()
    }

    private fun isPasswordValid(password: String): Boolean {
        return passwordPattern.matcher(password).matches()
    }
}