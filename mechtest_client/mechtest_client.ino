//Input desired height
//control system designed to move LA to desired height, while streaming back load cell values




//Megamoto:

int EnablePin = 8;
int PWMPin = 11;  // Timer2
int PWMPin2 = 3;
const byte CPin = 0;  // analog input channel
int CRaw;      // raw A/D value
float CVal;    // adjusted Amps value


//load cell
float load_read = 0;
long time = 0;
int interval = 100; //ms


//lvdt

int lvdt_read = 0;
int la_pos = 17;
int la_rest = 7;
bool control_bool = false;
bool retract_bool = false;

void setup(){
  //Initialize digital pin as an output for motor control

  pinMode(EnablePin, OUTPUT);
  pinMode(PWMPin, OUTPUT);
  pinMode(PWMPin2, OUTPUT);
  setPwmFrequency(PWMPin, 8);  // change Timer2 divisor to 8 gives 3.9kHz PWM freq

  //Begin Serial communications
  Serial.begin(115200);
  
}

void loop() {

  get_lvdt_data();

  //move_to_distance();
  //retract();
  
}



void move_to_distance() {

  if (control_bool == false)
  {

    retract_bool = false;
    if (la_pos > lvdt_read)
    {
      digitalWrite(EnablePin, HIGH);
      analogWrite(PWMPin2, 0);
      analogWrite(PWMPin, 255);
      control_bool = false;
    
    }
    else if (la_pos <= lvdt_read)
    {
      control_bool = true;
    }

  }
  else
  {
    retract_bool = true;
  }


  

  
  
}

void retract() {

  if (retract_bool == true)
  {

    control_bool = true;

    if (la_rest <= lvdt_read)
    {
      
      digitalWrite(EnablePin, HIGH);
      analogWrite(PWMPin2, 255);
      analogWrite(PWMPin, 0);
      
    }
    else if (la_rest >= lvdt_read)
    {
      retract_bool = false;
    }


    
  }
  
}


void setPwmFrequency(int pin, int divisor) {
  byte mode;
  if(pin == 5 || pin == 6 || pin == 9 || pin == 10) { // Timer0 or Timer1
    switch(divisor) {
      case 1: mode = 0x01; break;
      case 8: mode = 0x02; break;
      case 64: mode = 0x03; break;
      case 256: mode = 0x04; break;
      case 1024: mode = 0x05; break;
      default: return;
    }
    if(pin == 5 || pin == 6) { 
      TCCR0B = TCCR0B & 0b11111000 | mode; // Timer0
    } else {
      TCCR1B = TCCR1B & 0b11111000 | mode; // Timer1
    }
  } else if(pin == 3 || pin == 11) {
    switch(divisor) {
      case 1: mode = 0x01; break;
      case 8: mode = 0x02; break;
      case 32: mode = 0x03; break;
      case 64: mode = 0x04; break;
      case 128: mode = 0x05; break;
      case 256: mode = 0x06; break;
      case 1024: mode = 0x7; break;
      default: return;
    }
    TCCR2B = TCCR2B & 0b11111000 | mode; // Timer2
  }
}



//should be a function that grabs states of both sensors and folds them into a tuple (update_tuple)
//stream tuple back to user with a different function or maybe implant in same one
void get_lvdt_data() {
  lvdt_read = analogRead(1);

  if (millis() > time + interval)
  {
    Serial.println(lvdt_read);
    time = millis();
  }

}

