#include <WiFiManager.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>

#define DEBUG false
#define MAX_ACCESS_TOKEN_LEN 350

#define CHECK_STATUS_COMMAND "STATUS"
#define GET_ADDRESS_COMMAND "ADDRESS"
#define DELIVERY_ARRIVED_COMMAND "ARRIVED"
#define DELIVERY_STOLEN_COMMAND "STOLEN"

#define OK_RESULT "OK "
#define ERROR_RESULT "ERROR\n"

#define MAILBOX_STATUS_URL "http://192.168.1.16:5003/delivery/mailboxStatus"
#define MAILBOX_INFO_URL "http://192.168.1.16:5003/mailbox"
#define DELIVERY_ARRIVED_URL "http://192.168.1.16:5003/delivery/arrived"
#define DELIVERY_STOLEN_URL "http://192.168.1.16:5003/delivery/stolen"

char *_accessToken;

void setup()
{
  WiFiManager wifiManager;
  wifiManager.setDebugOutput(DEBUG);
  WiFiManagerParameter accessTokenParameter("token", "", "", MAX_ACCESS_TOKEN_LEN);
  wifiManager.addParameter(&accessTokenParameter);

  const char *configureStationName = "SmartMailbox";
  const char *configureStationPass = "password";

  // Uncomment to force reconfiguration
  // bool connected = wifiManager.startConfigPortal(configureStationName, configureStationPass);
  if (!wifiManager.autoConnect(configureStationName, configureStationPass))
  {
  }
  // const char *tokenValue = accessTokenParameter.getValue();
  // const uint tokenLength = strlen(tokenValue);
  // _accessToken = new char[tokenLength + 1]();
  // memcpy(_accessToken, tokenValue, tokenLength);
  _accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjJjZjA1N2NiLWMyMjAtNDFiOS1hMTAzLWJlM2Y5ZDY0MjEzYSIsIm1haWxib3hJZCI6Ijk4YWMzMTczLTM2OGYtNGMwZC1hY2UyLWUxYzUyNzUyMzVlYiIsInJvbGUiOiJtYWlsYm94IiwibmJmIjoxNjE2MDUzNjQxLCJleHAiOjE2MTk2NTM2NDEsImlzcyI6IkhhbmRJdE92ZXJBcGkifQ.2teOFt9Cy52ZBxeBib1miXxU-SySdIYzc3lXxt7y6BU";

  Serial.begin(9600);
  Serial.println("Got access token");
  Serial.println(_accessToken);
}

void loop()
{
  if (Serial.available() > 0)
  {
    String commandName = Serial.readStringUntil('\n');
    if (commandName.equals(CHECK_STATUS_COMMAND))
    {
      outputWhetherIsOpenAndRenter();
    }
    else if (commandName.equals(GET_ADDRESS_COMMAND))
    {
      outputMailboxAddress();
    }
    else if (commandName.equals(DELIVERY_ARRIVED_COMMAND))
    {
      float deliveryMass = Serial.readStringUntil('\n').toFloat();
      sendDeliveryArrived(deliveryMass);
    }
    else if (commandName.equals(DELIVERY_STOLEN_COMMAND))
    {
      sendDeliveryStolen();
    }
    else
    {
      Serial.print("Unknown command\n");
    }
  }

  delay(10000);
}

void outputWhetherIsOpenAndRenter()
{
  HTTPClient httpClient;
  if (httpClient.begin(MAILBOX_STATUS_URL))
  {
    httpClient.addHeader("Authorization", String("Bearer ") + _accessToken);
    int statusCode = httpClient.GET();
    if (statusCode == HTTP_CODE_OK)
    {
      String payload = httpClient.getString();
      StaticJsonDocument<192> doc;
      DeserializationError error = deserializeJson(doc, payload);
      if (!error)
      {
        const char *mailboxId = doc["mailboxId"];
        bool isOpen = doc["isOpen"];
        const char *renter = doc["renter"];

        Serial.print(OK_RESULT);
        Serial.print(isOpen ? "OPEN" : "CLOSED");
        Serial.print(' ');
        Serial.print(renter ? renter : "NONE");
        Serial.print('\n');

        httpClient.end();
        return;
      }
    }
    httpClient.end();
  }
  Serial.print(ERROR_RESULT);
}

void outputMailboxAddress()
{
  HTTPClient httpClient;
  if (httpClient.begin(MAILBOX_INFO_URL))
  {
    httpClient.addHeader("Authorization", String("Bearer ") + _accessToken);
    int statusCode = httpClient.GET();
    if (statusCode == HTTP_CODE_OK)
    {
      String payload = httpClient.getString();
      StaticJsonDocument<384> doc;
      DeserializationError error = deserializeJson(doc, payload);
      if (!error)
      {
        const char *address = doc["address"];

        Serial.print(OK_RESULT);
        Serial.print(address);
        Serial.print('\n');

        httpClient.end();
        return;
      }
    }
    httpClient.end();
  }
  Serial.print(ERROR_RESULT);
}

void sendDeliveryArrived(float deliveryMass)
{
  HTTPClient httpClient;
  if (httpClient.begin(DELIVERY_ARRIVED_URL))
  {
    httpClient.addHeader("Authorization", String("Bearer ") + _accessToken);
    httpClient.addHeader("Content-Type", "application/json");

    StaticJsonDocument<16> doc;
    doc["weight"] = deliveryMass;
    String output;
    serializeJson(doc, output);
    Serial.print(output);
    Serial.print('\n');

    int statusCode = httpClient.POST(output);
    if (statusCode == HTTP_CODE_OK)
    {
      Serial.print(OK_RESULT);
      Serial.print('\n');
      httpClient.end();
      return;
    }
    httpClient.end();
  }
  Serial.print(ERROR_RESULT);
}

void sendDeliveryStolen()
{
  HTTPClient httpClient;
  if (httpClient.begin(DELIVERY_STOLEN_URL))
  {
    httpClient.addHeader("Authorization", String("Bearer ") + _accessToken);
    httpClient.addHeader("Content-Type", "application/json");

    int statusCode = httpClient.POST("{}");
    if (statusCode == HTTP_CODE_OK)
    {
      Serial.print(OK_RESULT);
      Serial.print('\n');
      httpClient.end();
      return;
    }
    httpClient.end();
  }
  Serial.print(ERROR_RESULT);
}