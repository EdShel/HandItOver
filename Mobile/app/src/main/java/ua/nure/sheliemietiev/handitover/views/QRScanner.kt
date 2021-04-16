package ua.nure.sheliemietiev.handitover.views

import android.Manifest
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import me.dm7.barcodescanner.zxing.ZXingScannerView
import com.google.zxing.Result
import ua.nure.sheliemietiev.handitover.R
import ua.nure.sheliemietiev.handitover.util.SeverePermissionsEnabler

class QRScanner : AppCompatActivity(), ZXingScannerView.ResultHandler {
    private lateinit var scannerView: ZXingScannerView

    private lateinit var severePermissionsEnabler: SeverePermissionsEnabler

    private var allPermissionsGranted = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        scannerView = ZXingScannerView(this);
        setContentView(scannerView);

        severePermissionsEnabler =
            SeverePermissionsEnabler(this, listOf(Manifest.permission.CAMERA))
        severePermissionsEnabler.checkPermission({
            allPermissionsGranted = true
        }, {
            Toast.makeText(this, getString(R.string.allow_camera), Toast.LENGTH_LONG).show()
            finish()
        })
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        severePermissionsEnabler.acceptPermissionResult(requestCode, grantResults)
    }

    override fun onResume() {
        super.onResume()
        scannerView.setResultHandler(this)
        scannerView.startCamera()
    }

    override fun onPause() {
        super.onPause()
        scannerView.stopCamera()
    }

    override fun handleResult(result: Result) {
        val uri = Uri.parse(result.text)
        if (uri.isAbsolute) {
            val browseIntent = Intent(Intent.ACTION_VIEW, uri)
            startActivity(browseIntent)
        } else {
            scannerView.resumeCameraPreview(this)
        }
    }
}