
int EnablePin = 8;
int duty;
int PWMPin = 11; //PWMA, Timer2
int PWMPin2 = 3; //PWMB

const byte CPin = 0; //analog input channel
int CRAW; //raw A/D value
float CVal; //adjusted Amps value



long time = 0;
int interval = 200;

void setup() {
  //Begin serial ports, 9600 for load cell and 115200 for LA commands
  Serial.begin(115200);
  //Serial1.begin(115200);

  
  //initalize the digital pin as an output
  pinMode(EnablePin, OUTPUT);
  pinMode(PWMPin, OUTPUT);
  pinMode(PWMPin2, OUTPUT);

  //Change Timer2 divisor to 8 gives 3.9kHz PWM freq)
  //setPWMFrequency(PWMPin, 8);
  
  
}

void loop(){
 //string format: string 'up' or 'down'

 if (Serial.available() > 0) {
  String command;
  command = Serial.readString();
  if (command.toInt() == 1) {
    Serial.println(1);
  }


  if (Serial.available() > 0) {
      float analog_val = analogRead(0);

      if (millis() > time + interval) {
        Serial.println(analog_val);
        time = millis();
      }

      
  }

 }
  
}

