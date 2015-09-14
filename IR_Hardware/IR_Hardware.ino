/*
 Name:		IR_Hardware.ino
 Created:	8/23/2015 11:52:55 AM
 Author:	Stanislav
*/


const float kT = 0.615234375;
String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
boolean isStart = false;
float t1 = 0;
float t2 = 0;
float t3 = 0;
float t4 = 0;
int ledPin = 13;
int cycle = 0;
int pwrUp = 7;
int pwrLow = 8;
float tUp = 0;
float tLow = 0;
void setup() {
	pinMode(7, OUTPUT);
	pinMode(8, OUTPUT);
	pinMode(13, OUTPUT);
	digitalWrite(ledPin, HIGH);
	delay(100);
	digitalWrite(ledPin, LOW);
	// initialize serial: 
	Serial.begin(9600);
	// reserve 256 bytes for the inputString: 
	inputString.reserve(256);
}
void loop() {
	cycle = (cycle + 1) % 1024;
	if (cycle == 0)
	{
		digitalWrite(A0, LOW);
		delay(10);
		int sensorValue1 = analogRead(A0);
		delay(10);
		digitalWrite(A0, LOW);
		delay(10);
		int sensorValue2 = analogRead(A1);
		delay(10);
		digitalWrite(A0, LOW);
		delay(10);
		int sensorValue3 = analogRead(A2);
		delay(10);
		digitalWrite(A0, LOW);
		delay(10);
		int sensorValue4 = analogRead(A3);
		delay(10);
		t1 = (float)sensorValue1*kT;
		t2 = (float)sensorValue2*kT;
		t3 = (float)sensorValue3*kT;
		t4 = (float)sensorValue4*kT;
		if (isStart){
			if (t1 < tUp){
				digitalWrite(pwrUp, HIGH);
				digitalWrite(ledPin, HIGH);
			}
			else {
				digitalWrite(pwrUp, LOW);
				digitalWrite(ledPin, LOW);
			}
			if (t2 < tLow){
				digitalWrite(pwrLow, HIGH);
			}
			else {
				digitalWrite(pwrLow, LOW);
			}
		}
	}
	// print the string when a newline arrives: 
	if (stringComplete) {
		if (inputString.equalsIgnoreCase("HelloDevice\n")) 
		{
			Serial.println("HelloHost");
			Serial.println("IR station");
			Serial.println("4");
			isStart = true;
		}
		else {
			if (inputString[0] == 'U')
			{
				char buf[inputString.length() - 1];
				String data = inputString.substring(1,
					inputString.length() - 1);
				data.toCharArray(buf, data.length());
				tUp = (float)atof(buf);
				//Serial.println(tUp); 
			}
			if (inputString[0] == 'L')
			{
				char buf[inputString.length() - 1];
				String data = inputString.substring(1,
					inputString.length() - 1);
				data.toCharArray(buf, data.length());
				tLow = (float)atof(buf);
			}
			if (inputString == "GetTemp\n")
			{
				String data;
				char TempString[10];
				dtostrf(t1, 4, 2, TempString);
				data = String(TempString) + ";";
				dtostrf(t2, 4, 2, TempString);
				data = data + String(TempString) + ";";
				dtostrf(t3, 4, 2, TempString);
				data = data + String(TempString) + ";";
				dtostrf(t4, 4, 2, TempString);
				data = data + String(TempString);
				Serial.println(data);
			}
		}
		// clear the string: 
		inputString = "";
		stringComplete = false;
	}
}
void serialEvent() {
	while (Serial.available()) {
		// get the new byte: 
		char inChar = (char)Serial.read();
		// add it to the inputString: 
		inputString += inChar;
		// if the incoming character is a newline, set 	a flag
		// so the main loop can do something about it: 
		if (inChar == '\n') {
			stringComplete = true;
		}
	}
}

