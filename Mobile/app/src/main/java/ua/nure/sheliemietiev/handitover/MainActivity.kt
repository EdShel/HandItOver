package ua.nure.sheliemietiev.handitover

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.Toast
import com.google.android.gms.tasks.OnCompleteListener
import com.google.firebase.messaging.FirebaseMessaging

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val button = findViewById<Button>(R.id.firebaseButton)
        button.setOnClickListener {
            FirebaseMessaging.getInstance().token.addOnCompleteListener(OnCompleteListener { task ->
                if (!task.isSuccessful) {
                    Toast.makeText(this, "Shit", Toast.LENGTH_SHORT).show()
                    return@OnCompleteListener
                }

                val token = task.result

                Toast.makeText(this, token, Toast.LENGTH_SHORT).show()
                Log.w("LOL", token ?: "ooops")

            })
        }
    }
}