package ua.nure.sheliemietiev.handitover.views

import android.content.Intent
import android.net.Uri
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.view.inputmethod.EditorInfo
import android.widget.Button
import android.widget.EditText
import android.widget.ProgressBar
import android.widget.Toast
import androidx.annotation.StringRes
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import kotlinx.android.synthetic.main.activity_login.*
import ua.nure.sheliemietiev.handitover.App
import ua.nure.sheliemietiev.handitover.BuildConfig
import ua.nure.sheliemietiev.handitover.MainActivity
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.api.AuthorizationInfo
import ua.nure.sheliemietiev.handitover.models.AuthorizationResult
import ua.nure.sheliemietiev.handitover.util.afterTextChanged
import ua.nure.sheliemietiev.handitover.viewModels.LoginViewModel
import javax.inject.Inject

class LoginActivity : AppCompatActivity() {

    @Inject
    lateinit var viewModelFactory: ViewModelProvider.Factory

    @Inject
    lateinit var authorizationInfo: AuthorizationInfo

    private lateinit var loginViewModel: LoginViewModel

    private lateinit var username: EditText

    private lateinit var password: EditText

    private lateinit var loginButton: Button

    private lateinit var loading: ProgressBar

    override fun onCreate(savedInstanceState: Bundle?) {
        (applicationContext as App).components.inject(this)

        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        loginViewModel = ViewModelProvider(this, viewModelFactory)
            .get(LoginViewModel::class.java)

        username = findViewById<EditText>(R.id.username)
        password = findViewById<EditText>(R.id.password)
        loginButton = findViewById<Button>(R.id.loginButton)
        loading = findViewById(R.id.loading)

        // TODO: remove it, it's just for test
        username.setText("eduard.sheliemietiev@nure.ua")
        password.setText("qwerty")
        loginButton.isEnabled = true

        observeValidationErrors()

        observeLoginResult(loginViewModel)

        validateFieldsChanged(username, loginViewModel, password)

        observeLoginSubmit()

        observeRegisterPressed()
    }

    private fun observeLoginSubmit() {
        password.setOnEditorActionListener { _, actionId, _ ->
            when (actionId) {
                EditorInfo.IME_ACTION_DONE -> {
                    loading.visibility = View.VISIBLE
                    loginViewModel.loginButtonPressed(
                        username.text.toString(),
                        password.text.toString()
                    )
                }
            }
            false
        }

        loginButton.setOnClickListener {
            loading.visibility = View.VISIBLE
            loginViewModel.loginButtonPressed(
                username.text.toString(),
                password.text.toString()
            )
        }
    }

    private fun observeValidationErrors() {
        loginViewModel.loginFormState.observe(this@LoginActivity, Observer {
            val loginState = it ?: return@Observer

            loginButton.isEnabled = loginState.isDataValid
            if (loginState.usernameError != null) {
                username.error = getString(loginState.usernameError)
            }
            if (loginState.passwordError != null) {
                password.error = getString(loginState.passwordError)
            }
        })
    }

    private fun validateFieldsChanged(
        username: EditText,
        loginViewModel: LoginViewModel,
        password: EditText
    ) {
        username.afterTextChanged {
            loginViewModel.loginDataChanged(
                username.text.toString(),
                password.text.toString()
            )
        }

        password.afterTextChanged {
            loginViewModel.loginDataChanged(
                username.text.toString(),
                password.text.toString()
            )
        }
    }

    private fun observeLoginResult(
        loginViewModel: LoginViewModel
    ) {
        loginViewModel.loginResult.observe(this@LoginActivity, Observer {
            val loginResult = it ?: return@Observer
            loading.visibility = View.GONE
            if (loginResult.isFailure) {
                showLoginFailed(loginResult.getErrorCode())
            }
            if (loginResult.isSuccess) {
                showWelcomeMessage(loginResult.getOrThrow())
                goToMainActivity()
            }
        })
    }

    private fun goToMainActivity() {
        val mainActivity = Intent(this, MainActivity::class.java)
        startActivity(mainActivity)
    }

    private fun showWelcomeMessage(model: AuthorizationResult) {
        val welcome = "Greetings,"
        val displayName = model.userName
        Toast.makeText(
            applicationContext,
            "$welcome $displayName",
            Toast.LENGTH_LONG
        ).show()
    }

    private fun showLoginFailed(@StringRes errorString: Int) {
        Toast.makeText(applicationContext, errorString, Toast.LENGTH_SHORT).show()
    }

    private fun observeRegisterPressed() {
        registerButton.setOnClickListener {
            val browseIntent = Intent(Intent.ACTION_VIEW, Uri.parse(BuildConfig.WEB_URL))
            startActivity(browseIntent)
        }
    }
}