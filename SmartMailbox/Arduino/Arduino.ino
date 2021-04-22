#include <SoftwareSerial.h>
#include "HX711.h"

SoftwareSerial mySerial(2, 3);

// HX711 circuit wiring
const int LOADCELL_DOUT_PIN = A1;
const int LOADCELL_SCK_PIN = A0;

float Calibration_Factor_Of_Load_cell = -41.7;
float U;
float O;

HX711 scale;

void setup()
{
  Serial.begin(9600);
  mySerial.begin(9600);
  // scale.begin(LOADCELL_DOUT_PIN, LOADCELL_SCK_PIN);
  // scale.tare();
}

void loop()
{
  mySerial.print("ADDRESS\n");
  Serial.print("ADDRESS\n");
  while (mySerial.available() == 0)
  {
    Serial.println("Nothing...");
    delay(1000);
  }
  String readResult = mySerial.readStringUntil(' ');
  Serial.print(String("Read ") + readResult + "\n");
  if (readResult.equals("OK"))
  {
    String address = mySerial.readStringUntil('\n');
    Serial.println(address);
  }
  else
  {
    serialFlush();
  }
  delay(10000);

  // scale.set_scale(Calibration_Factor_Of_Load_cell);

  // U = scale.get_units();
  // if (U < 0)
  // {
  //   U = 0.00;
  // }
  // O = U * 0.035274;
  // Serial.print(O);
  // Serial.print(" grams");
  // Serial.print(" Calibration_Factor_Of_Load_cell: ");
  // Serial.print(Calibration_Factor_Of_Load_cell);
  // Serial.println();

  // delay(1000);
}

void serialFlush()
{
  while (mySerial.available() > 0)
  {
    char t = mySerial.read();
  }
}