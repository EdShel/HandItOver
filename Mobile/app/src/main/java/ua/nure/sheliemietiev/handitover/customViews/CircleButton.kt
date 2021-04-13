package ua.nure.sheliemietiev.handitover.customViews

import android.animation.ValueAnimator
import android.content.Context
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.Paint
import android.util.AttributeSet
import android.view.GestureDetector
import android.view.MotionEvent
import android.view.View
import androidx.annotation.StyleableRes
import ua.nure.sheliemietiev.handitover.R

class CircleButton @JvmOverloads constructor(
    context: Context, attrs: AttributeSet? = null, defStyleAttr: Int = 0
) : View(context, attrs, defStyleAttr) {
    var text: CharSequence

    var canClick: Boolean = true

    var innerCircleSize: Float = 0f

    private var longClickListener: ((CircleButton) -> Unit)? = null

    private val outerCirclePaint = Paint(Paint.ANTI_ALIAS_FLAG).apply {
        style = Paint.Style.FILL
        color = Color.rgb(53, 242, 75)
    }

    private val innerCirclePaint = Paint(Paint.ANTI_ALIAS_FLAG).apply {
        style = Paint.Style.FILL
        color = Color.rgb(242, 182, 53)
    }

    private val textPaint = Paint(Paint.ANTI_ALIAS_FLAG).apply {
        style = Paint.Style.FILL
        color = Color.rgb(0, 0, 0)
        textAlign = Paint.Align.CENTER
        textSize = 48f
    }

    init {
        context.obtainStyledAttributes(attrs, R.styleable.CircleButton).apply {
            text = getText(R.styleable.CircleButton_text)
            textPaint.textSize = getDimension(R.styleable.CircleButton_text_size, 48f)
            textPaint.color = getColor(R.styleable.CircleButton_text_color, Color.BLACK)
            outerCirclePaint.color = getColor(R.styleable.CircleButton_outer_color, Color.WHITE)
            innerCirclePaint.color = getColor(R.styleable.CircleButton_inner_color, Color.GRAY)
            recycle()
        }
    }

    private val gestureListener = object : GestureDetector.SimpleOnGestureListener() {
        override fun onDown(e: MotionEvent?): Boolean {
            return true
        }

        override fun onLongPress(e: MotionEvent?) {
            longClickListener?.invoke(this@CircleButton)
            val an = ValueAnimator.ofFloat(0f, 1f).apply {
                duration = 500
                addUpdateListener {
                    innerCircleSize = it.animatedValue as Float
                    invalidate()
                }
                start()
            }
        }
    }

    private val gestureDetector = GestureDetector(context, gestureListener)

    private val radius: Float get() = width.coerceAtMost(height) / 2f

    fun setLongClickListener(listener: (CircleButton) -> Unit) {
        longClickListener = listener
    }

    override fun onTouchEvent(event: MotionEvent?): Boolean {
        if (!canClick || event == null || !isPointInsideCircle(event.x, event.y)) {
            return false
        }
        return gestureDetector.onTouchEvent(event)
    }

    private fun isPointInsideCircle(x: Float, y: Float): Boolean {
        val centerX = width / 2f
        val centerY = height / 2f
        val xDif = x - centerX
        val yDif = y - centerY
        return xDif * xDif + yDif * yDif <= radius * radius
    }

    override fun onDraw(canvas: Canvas?) {
        if (canvas == null) {
            return
        }
        canvas.apply {
            val centerX = width / 2f
            val centerY = height / 2f
            val r = radius
            if (innerCircleSize < 1f) {
                drawCircle(centerX, centerY, r, outerCirclePaint)
            }
            if (innerCircleSize > 0f) {
                drawCircle(centerX, centerY, r * innerCircleSize, innerCirclePaint)
            }
            drawText(text, 0, text.length, centerX, centerY, textPaint)
        }
    }
}