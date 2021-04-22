#include <WiFiManager.h>

#define DEBUG false
#define MAX_ACCESS_TOKEN_LEN 350

char *_accessToken;

void setup()
{
  WiFiManager wifiManager;
  wifiManager.setDebugOutput(DEBUG);
  WiFiManagerParameter accessTokenParameter("token", "", "", MAX_ACCESS_TOKEN_LEN);
  wifiManager.addParameter(&accessTokenParameter);

  const char *configureStationName = "SmartMailbox";
  const char *configureStationPass = "password";

  bool connected = wifiManager.startConfigPortal(configureStationName, configureStationPass);
  if (!connected)
  {
  }
  const char* tokenValue = accessTokenParameter.getValue(); 
  const uint tokenLength = strlen(tokenValue);
  _accessToken = new char[tokenLength + 1]();
  memcpy(_accessToken, tokenValue, tokenLength);

  Serial.begin(9600);
  Serial.println("Got access token");
  Serial.println(_accessToken);
}

void loop()
{
  digitalWrite(LED_BUILTIN, LOW);
  Serial.println("Loop Got access token");
  Serial.println(_accessToken);
  delay(1000);
  digitalWrite(LED_BUILTIN, HIGH);
  delay(2000);
}