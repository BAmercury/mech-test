//#include <MegaMotoHB.h>

float analog_val = 0;
float analog_int = 0;
float loadA = 0;
//number of samples to take for average
const byte avgNum=100;
const float constant=-113.27;
float offset = 0;
int grams = 0;
float g = 0;
const float slope = 8.8319;

long time = 0;
int interval = 200; //in miliseconds

int motor_enablepin = 8;
int motor_pwm_pin = 11;
int motor_pwm_pin2 = 5;

boolean set_speed = true;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  analog_int = analogRead(0);
  delay(10);
  analog_int = analogRead(0);

  Serial.println("Zeroing Out");
  delay(10);

}

void loop() {
  // put your main code here, to run repeatedly:


  //digitalWrite(10, HIGH);
  //delay(10);
  //digitalWrite(10, LOW);
  //delay(20);

 //if (set_speed == true)
 //{
   //   motor.Fwd(150);
 
 //set_speed = false;
//}
  
  //motor.Fwd(150);

  //Serial.println(analog_val);
  //calcGrams();

  //Serial.println(analog_val, DEC);

  if (millis() > time + interval)
  {

    int analog_val = analogRead(0);
    //Serial.print("load: ");
    //analog_val = map(analog_val, analog_int, 2000, 0, 1000);
    Serial.println(analog_val);
    time = millis();
  }

}


float analog_to_load(float analog_value)
{
  float load = mapfloat(analog_value, analog_int, loadA);
  return load;
}


float mapfloat(float x, float in_min, float out_min)
{
  return (x - in_min) * (out_min) / (in_min) + out_min;
}

void calcGrams()
{
  analog_int = 0;
  // take avgNum samples
  for (int i=0;i<avgNum;i++)
  {
    int avalue=analogRead(0);
    analog_int =analog_int + avalue;
    delay(5);
  }
  analog_int = analog_int / avgNum;
  // calculate grams and round upward if necessary
  g=slope*(float)analog_int +constant+offset;
  grams=(int)g;
  float remainder=g-(float)grams;
  if (remainder > 0.5) grams=grams+1;
}





