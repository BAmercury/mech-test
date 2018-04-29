/*

 Name:		mechanica_arduino.ino
 Created:	3/1/2018 1:13:50 PM
 Author:	Brian Amin
*/

// the setup function runs once when you press reset or power the board
#include <Encoder.h>
#include <SoftwareSerial.h>


// Setup Pins
int chip_select = 4;
int pinA = 2;
int pinB = 3;
int loadcell_pin = 0;

//Magnetic Sensor
Encoder mag_sensor(2, 3);


// Load cell
long analog_int = 0;



struct CommandMessage
{
	long DisplacementRate, Displacement;
	long Reset;
};
CommandMessage command_message;

struct FeedbackMessage
{
	long Load, Distance;

};
FeedbackMessage feedback_message;

const unsigned int BUFFER_SIZE = 256;
char message_buffer[BUFFER_SIZE];
char feedback_buffer[BUFFER_SIZE];


bool control_bool = true;
bool retract_bool = false;

long time = 0;
int interval = 500; //ms
String incoming_string = "";
double desired_position = 00.00;
String command = "0";
boolean newData = false;
boolean newData_desired = false;
const byte numChars = 32;
const byte numChars_desired = 32;
char recievedChars[numChars];
char recievedDesiredChars[numChars_desired];

// Set up Roboclaw serial object
SoftwareSerial roboclaw(10, 11); //RX TX


double update_position()
{
	long newPos;
	newPos = mag_sensor.read();
	double pos = (newPos / 1024.0) * (2.0);

	return pos;
}

float read_loadcell()
{
	float analog_val = analogRead(0);

	//analog_val = map(analog_val, analog_int, 2000, 0, 1000);

	return analog_val;
}


void zero_loadcell()
{
  analog_int = analogRead(0);
  delay(10);
  analog_int = analogRead(0);

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
	roboclaw.begin(38400);
	Serial.begin(115200);
	pinMode(chip_select, OUTPUT);

	digitalWrite(chip_select, LOW);

	delay(50);
	digitalWrite(chip_select, HIGH);


	//retract();
	Serial.println("ready");
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

		long val = analogRead(0);
		long load_time = millis();


		if (control_bool == false)
		{
			if (desired_position > pos)
			{
				roboclaw.write(127);
			}
			else if (desired_position <= pos)
			{
				control_bool = true;
				newData_desired = false;
				desired_position = 00.00;
				roboclaw.write(1);
				command = "0";
			}


			if (millis() > time + interval)
			{

        //val = map(val, analog_int, 2000, 0, 1000);

				time = millis();

				Serial.print(pos);
				Serial.print(",");
				Serial.print(pos_time);
				Serial.print(",");
				Serial.print(val);
				Serial.print(",");
				Serial.print(load_time);
				Serial.println();



			}
		}
		else if (control_bool == true)
		{

			Serial.println("give");
			//while (!Serial.available()) ;
			while (Serial.available() == 0)
			{
				desired_position = Serial.parseFloat();
				control_bool = false;
				newData_desired = true;

			}

		}

		


	}
	else if (command == "0")
	{
		roboclaw.write(1);
	}



	




  
}


void retract()
{
	roboclaw.write(64);
	delay(10000);

	roboclaw.write(1);
	mag_sensor.write(0);
    zero_loadcell();
	Serial.println("begin");
}


