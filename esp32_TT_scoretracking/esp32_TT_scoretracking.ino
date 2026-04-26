#include <LiquidCrystal.h>

LiquidCrystal lcd(19, 23, 18, 17, 16, 15);

String centerText(String text) {
  int len = text.length();
  if (len >= 16) return text;
  int totalPadding = 16 - len;
  int leftPadding = totalPadding / 2;
  String centered = "";
  for (int i = 0; i < leftPadding; i++) centered += " ";
  centered += text;
  return centered;
}

void setup() {
  Serial.begin(115200);
  lcd.begin(16, 2);
  lcd.setCursor(0, 0);
  lcd.print(centerText("Waiting..."));
}

vovoid loop() {
  if (Serial.available()) {
    String data = Serial.readStringUntil('\n');
    int separator = data.indexOf('|');
    String line1 = data.substring(0, separator);
    String line2 = data.substring(separator + 1);
    line1.trim();
    line2.trim();
    lcd.clear();
    lcd.setCursor(0, 0); lcd.print(centerText(line1));
    lcd.setCursor(0, 1); lcd.print(centerText(line2));
  }
}