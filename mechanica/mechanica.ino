#include <Encoder.h>
#include <PID.h>
// Change these pin numbers to the pins connected to your encoder.
//   Best Performance: both pins have interrupt capability
//   Good Performance: only the first pin has interrupt capability
//   Low Performance:  neither pin has interrupt capability
//   avoid using pins with LEDs attached

//Setup Pins
int chip_select = 4;
int pinA = 2;
int pinB = 3;
int loadcell_pin = 0;
int motor_enablepin = 8;
int motor_pwm_pin = 11;
int motor_pwm_pin2 = 5;


//Magnetic Sensor
Encoder der(3, 2);


// Load cell
float analog_val = 0;
float analog_int = 0;
float loadA = 0;

//Stream output
float[] output;

float desired_position = 0;

void setup() {
	Serial.begin(115200);
	pinMode(chip_select, OUTPUT);
	pinMode(motor_enablepin, OUTPUT);
	pinMode(motor_pwm_pin, OUTPUT);
	pinMode(motor_pwm_pin2, OUTPUT);

	digitalWrite(chip_select, LOW);

	delay(50);
	Serial.print("Device Ready");
	digitalWrite(chip_select, HIGH);
}

long oldPos = -999;

void loop() {
	long newPos;
	newPos = der.read();

	if (newPos != oldPos) {
		Serial.print("New Pos (mm) = ");
		double pos = (newPos / 1024.0) * 2.0;
		Serial.println(pos);
		oldPos = newPos;
	}
}
