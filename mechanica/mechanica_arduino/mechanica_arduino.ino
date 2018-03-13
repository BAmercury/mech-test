/*
 Name:		mechanica_arduino.ino
 Created:	3/1/2018 1:13:50 PM
 Author:	Merc
*/

// the setup function runs once when you press reset or power the board
#include <MegaMotoHB.h>
#include <Encoder.h>


// Setup Pins
int chip_select = 4;
int pinA = 2;
int pinB = 3;
int loadcell_pin = 0;
int motor_enablepin = 8;
int motor_pwm_pin = 11;
int motor_pwm_pin2 = 5;
MegaMotoHB motor(motor_pwm_pin, motor_pwm_pin2, motor_enablepin);


//Magnetic Sensor
Encoder mag_sensor(3, 2);


// Load cell
float analog_val = 0;
float analog_int = 0;
float loadA = 0;



double desired_position = 70.00;

bool control_bool = false;
bool retract_bool = false;

long time = 0;
int interval = 100; //ms

void setup() {
	Serial.begin(115200);
	pinMode(chip_select, OUTPUT);

	digitalWrite(chip_select, LOW);

	delay(50);
	digitalWrite(chip_select, HIGH);


	retract();

	//setTime(0);

}

// the loop function runs over and over again until power down or reset
void loop() {
	//magnetic sensor
	long newPos;
	newPos = mag_sensor.read();


	double pos = (newPos / 1024.0) * 2.0;
	long pos_time = millis();


	if (control_bool == false)
	{
		if (desired_position > pos)
		{
			motor.Rev(150);
		}
		else if (desired_position <= pos)
		{
			control_bool = true;
		}
	}
	else if (control_bool == true)
	{
		motor.Stop();
	}

	if (millis() > time + interval)
	{
		int analog_val = analogRead(0);

		//Serial.print("Position (mm): ");
		//Serial.println(pos);
		time = millis();

		Serial.print(pos);
		Serial.print(",");
		Serial.print(pos_time);
		Serial.print(",");
		Serial.print(analog_val);
		Serial.print(",");
		Serial.print(time);
		Serial.println();



	}




  
}

void retract()
{
	motor.Fwd(255);
	delay(10000);

	motor.Stop();
	mag_sensor.write(0);
}


