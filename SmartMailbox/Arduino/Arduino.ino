#include <SoftwareSerial.h>
#include "HX711.h"
#include <LiquidCrystal_I2C.h>

#define LCD_W 20
#define LCD_H 4
#define LOADCELL_DOUT_PIN A1;
#define LOADCELL_SCK_PIN A0;

float Calibration_Factor_Of_Load_cell = -41.7;
float U;
float O;

HX711 scale;
LiquidCrystal_I2C lcd(0x27, LCD_W, LCD_H);
SoftwareSerial esp8266(2, 3);

void setup()
{
  Serial.begin(9600);
  esp8266.begin(9600);
  lcd.init(); // initialize the lcd
  lcd.backlight();
  lcd.clear();
  printLcdOnLines("Loading...", 0, 0);

  printAddress();
  // scale.begin(LOADCELL_DOUT_PIN, LOADCELL_SCK_PIN);
  // scale.tare();
}

void loop()
{

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

void printAddress()
{
  while (true)
  {
    esp8266Flush();
    esp8266.print("ADDRESS\n");
    while (esp8266.available() == 0)
    {
      delay(1000);
    }
    String readResult = esp8266.readStringUntil(' ');
    Serial.print(String("Read ") + readResult + "\n");
    if (readResult.equals("OK"))
    {
      String address = esp8266.readStringUntil('\n');
      Serial.println(address);
      printLcdOnLines(address, 0, 1);
      return;
    }
    else
    {
      esp8266Flush();
      delay(5000);
    }
  }
}

void esp8266Flush()
{
  while (esp8266.available() > 0)
  {
    char t = esp8266.read();
  }
}

void printLcdOnLines(String text, int lineFrom, int lineToInclusive)
{
  for (int i = lineFrom; i <= lineToInclusive; i++)
  {
    clearLcdLine(i);
  }
  int textLength = text.length();
  if (textLength < LCD_W)
  {
    int leftPadding = (LCD_W - textLength) / 2;
    lcd.setCursor(leftPadding, lineFrom);
    lcd.print(text);
    return;
  }

  for (int i = lineFrom, printFrom = 0; i <= lineToInclusive && printFrom < textLength; i++, printFrom += LCD_W)
  {
    int leftToPrint = textLength - printFrom;
    int printOnThisRow = min(LCD_W, leftToPrint);
    lcd.setCursor(0, i);
    for (int j = 0; j < printOnThisRow; j++)
    {
      lcd.print(text[printFrom + j]);
    }
  }
}

void clearLcdLine(const int rowNum)
{
  lcd.setCursor(0, rowNum);
  for (int i = 0; i < LCD_W; i++)
  {
    lcd.print(' ');
  }
}