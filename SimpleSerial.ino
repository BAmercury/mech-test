//Roboclaw simple serial example.  Set mode to 6.  Option to 4(38400 bps)
#include <SoftwareSerial.h>

//See limitations of Arduino SoftwareSerial
int analog_int = 0;
SoftwareSerial mySerial(10,11); //RX TX
int interval = 200;
long time = 0;
boolean set_speed = true;
void setup() {
  mySerial.begin(38400);
  Serial.begin(115200);
  analog_int = analogRead(0);
  delay(10);
  analog_int = analogRead(0);
  pinMode(8, OUTPUT);
    delay(2000);

}

void loop() {

  if (set_speed == true)
  {
    mySerial.write(127);
    set_speed = false;
  }

  if (millis() > 10000)
  {
    mySerial.write(64);
  }
  
  if (millis() > time + interval)
  {

    int analog_val = analogRead(0);
    Serial.print("load: ");
    //analog_val = map(analog_val, analog_int, 2000, 0, 1000);
    Serial.println(analog_val);
    time = millis();
  }

}
