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


String command = "0";
boolean newData = false;
const byte numChars = 3;
char recievedChars[numChars];

double update_position()
{
	long newPos;
	newPos = mag_sensor.read();
	double pos = (newPos / 1024.0) * (2.0);

	return pos;
}

int read_loadcell()
{
	int analog_val = analogRead(0);
	return analog_val;
}

void recieve_commands()
{
	static boolean recvInProgress = false;
	static byte ndx = 0;
	char rc;
	char endMarker = '>';
	char startMaker = '<';
	while (Serial.available() > 0 && newData == false)
	{
		rc = Serial.read();
		if (recvInProgress == true)
		{
			if (rc != endMarker)
			{
				recievedChars[ndx] = rc;
				ndx++;
				if (ndx >= numChars)
				{
					ndx = numChars - 1;
				}
			}
			else
			{
				recievedChars[ndx] = '\0'; //termiante the string
				recvInProgress = false;
				ndx = 0;
				newData = true;
			}
		}
		else if (rc == startMaker)
		{
			recvInProgress = true;
		}

	}
}

void parseCommands()
{
	command = recievedChars;
}

void setup() {
	Serial.begin(115200);
	pinMode(chip_select, OUTPUT);

	digitalWrite(chip_select, LOW);

	delay(50);
	digitalWrite(chip_select, HIGH);


	//retract();

	//setTime(0);

}

// the loop function runs over and over again until power down or reset
void loop() {

	recieve_commands();
	if (newData == true)
	{
		parseCommands();
		newData = false;
	}

	if (command == "1")
	{
		retract();
		command = "0";
	}
	else if (command == "2")
	{
		double pos = update_position();
		// Will have to fix this at some point
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
			command = "0";
		}

		if (millis() > time + interval)
		{
			int analog_val = read_loadcell();

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
	else if (command == "0")
	{
		motor.Stop();
	}



	




  
}

void retract()
{
	motor.Fwd(255);
	delay(10000);

	motor.Stop();
	mag_sensor.write(0);
	Serial.print("ready");
}


