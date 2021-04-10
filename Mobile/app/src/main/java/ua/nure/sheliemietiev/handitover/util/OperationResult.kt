package ua.nure.sheliemietiev.handitover.util

open class OperationResult<out T> protected constructor(
    private val value: T?
) {
    val isSuccess: Boolean get() = value != null

    val isFailure: Boolean get() = value == null

    fun getOrNull(): T? =
        when {
            isFailure -> null
            else -> value
        }

    fun getOrThrow(): T {
        if (this is OperationFailure) {
            throw this.exception;
        }
        if (value == null) {
            throw Exception("Value is null!")
        }
        return this.value
    }

    fun getErrorCode(): Int {
        if (this is OperationError) {
            return this.errorCode
        }
        throw IllegalStateException("The result does not have an error!")
    }

    companion object {
        fun <T> success(value: T): OperationResult<T> =
            OperationResult(value)

        fun <T> failure(error: Throwable?): OperationResult<T> =
            OperationFailure(error ?: Exception("Operation failure!"))

        fun <T> error(errorCode: Int = 0): OperationResult<T> =
            OperationError(errorCode)
    }

    private class OperationFailure<T>(
        internal val exception: Throwable
    ) : OperationResult<T>(null)

    private class OperationError<T>(
        internal val errorCode: Int
    ) : OperationResult<T>(null)
}