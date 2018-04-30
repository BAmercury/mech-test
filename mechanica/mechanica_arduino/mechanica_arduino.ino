/*

 Name:		mechanica_arduino.ino
 Created:	3/1/2018 1:13:50 PM
 Author:	Brian Amin
*/

// the setup function runs once when you press reset or power the board
#include <Encoder.h>
#include <SoftwareSerial.h>

// Color Code:
// POWR = RED
// CSN = BLUE
// CLK = GREEN
// DO = YELLOW
// PWM = ORANGE
// B = BROWN
// A = PURPLE
// GND = BLACk


// Setup Pins
int chip_select = 4;
int pinA = 2;
int pinB = 3;
int loadcell_pin = 0;

//Magnetic Sensor
Encoder mag_sensor(2, 3);

struct CommandMessage
{
	long ControlBool;
	long DisplacementRate, Displacement;
	long Reset;
};
CommandMessage command_message;

struct FeedbackMessage
{
	double Distance;
	long DistTime, LoadTime, Load;

};


const unsigned int BUFFER_SIZE = 256;
char message_buffer[BUFFER_SIZE];


long time = 0;
int interval = 200; //ms

bool got_commands = false;

// Set up Roboclaw serial object
SoftwareSerial roboclaw(10, 11); //RX TX

void handle_message()
{
	char* last_token = strtok(message_buffer, "<");
	char* next_token;

	{
		command_message.ControlBool = strtol(last_token, &next_token, 10);
		last_token = next_token + 1;
		command_message.DisplacementRate = strtol(last_token, &next_token, 10);
		last_token = next_token + 1;
		command_message.Displacement = strtol(last_token, &next_token, 10);
		last_token = next_token + 1;
		command_message.Reset = strtol(last_token, &next_token, 10);
	}


}


void retract()
{
	roboclaw.write(1);
	delay(10000);

	roboclaw.write(64);
	mag_sensor.write(0);
	digitalWrite(13, HIGH);
	delay(1000);
	digitalWrite(13, LOW);
	delay(1000);
	got_commands = false;
	Serial.print("go");
	Serial.println();
}

void update_motors()
{
	if (command_message.Reset == 1)
	{
		retract();
	}

	uint8_t speed = map(command_message.DisplacementRate, 0, 5, 64, 127);

	if (command_message.ControlBool == 1)
	{
		FeedbackMessage feedback;
		feedback.Distance = update_position();
		feedback.DistTime = millis();
		if (command_message.Displacement > feedback.Distance)
		{
			roboclaw.write(speed);
			got_commands = true;
			feedback.Load = read_loadcell();
			feedback.LoadTime = millis();
			if (millis() > time + interval)
			{
				time = millis();
				Serial.print(feedback.Distance);
				Serial.print(",");
				Serial.print(feedback.DistTime);
				Serial.print(",");
				Serial.print(feedback.Load);
				Serial.print(",");
				Serial.print(feedback.LoadTime);
				Serial.println();



			}
		}
		else if (command_message.Displacement <= feedback.Distance)
		{
			roboclaw.write(64);
			got_commands = false;
			Serial.print("done");
			Serial.println();
		}


	}
}


double update_position()
{
	long newPos;
	newPos = mag_sensor.read();
	double pos = (newPos / 1024.0) * (2.0);

	return pos;
}

long read_loadcell()
{
	long analog_val = analogRead(0);

	//analog_val = map(analog_val, analog_int, 2000, 0, 1000);

	return analog_val;
}




void setup() {
	roboclaw.begin(38400);
	Serial.begin(115200);
	pinMode(chip_select, OUTPUT);
	pinMode(13, OUTPUT);
	digitalWrite(13, LOW);
	digitalWrite(chip_select, LOW);
	delay(50);
	digitalWrite(chip_select, HIGH);

	Serial.println("ready");

}

// the loop function runs over and over again until power down or reset
void loop() {

	if (got_commands == false)
	{
		if (Serial.available() > 0)
		{
			unsigned int message_length = Serial.readBytesUntil('>', message_buffer, 256);
			if (message_length > 0)
			{
				message_buffer[message_length] = '\0';
				handle_message();
			}
		}


	}

	update_motors();

}





