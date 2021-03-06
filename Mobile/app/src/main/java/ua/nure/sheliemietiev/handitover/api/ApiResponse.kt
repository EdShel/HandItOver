package ua.nure.sheliemietiev.handitover.api

import com.google.gson.Gson
import com.google.gson.JsonArray
import com.google.gson.JsonElement
import com.google.gson.JsonObject
import okhttp3.Response

class ApiResponse() {

    var statusCode : Int = 0
        private set

    var body : String = ""
        private set

    val isSuccessful get() = (statusCode / 100) == 2

    constructor(response : Response) : this() {
        this.statusCode = response.code()
        this.body = response.body()?.string() ?: ""
    }

    fun asJsonMap() : JsonObject {
        return Gson()
            .fromJson(this.body, JsonElement::class.java)
            .asJsonObject
    }

    fun asJsonArray() : JsonArray {
        return Gson()
            .fromJson(this.body, JsonElement::class.java)
            .asJsonArray
    }
}