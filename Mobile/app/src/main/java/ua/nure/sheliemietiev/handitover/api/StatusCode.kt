package ua.nure.sheliemietiev.handitover.api

enum class StatusCode(val code : Int) {
    OK(200),
    BAD_REQUEST(400),
    UNAUTHORIZED(401),
    FORBIDDEN(403),
    NOT_FOUND(404),
    BAD_GATEWAY(503)
}