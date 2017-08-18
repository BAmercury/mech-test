#define RED 3
#define GREEN 5
#define BLUE 6
#define LOW 0
#define HIGH 254
void setup() {
  pinMode(RED, OUTPUT);
  pinMode(GREEN, OUTPUT);
  pinMode(BLUE, OUTPUT);
  
  }
  
 void loop() { 
  analogWrite(BLUE, LOW);
  analogWrite(GREEN,LOW);
  analogWrite(RED, LOW);
  delay(1000);
  analogWrite(RED, LOW);
  analogWrite(BLUE,LOW);
  analogWrite(GREEN, HIGH);
  delay(1000);
  analogWrite(GREEN, LOW);
  analogWrite(RED, LOW);
  analogWrite(BLUE, HIGH);
  delay(1000);
  analogWrite(BLUE, LOW);
  analogWrite(GREEN,LOW);
  analogWrite(RED, LOW);
  
  fade();
  fade();
  
  seizuretime();
  seizuretime();
  seizuretime();
  
 }
  
  
  void fade(){
  for(int i = LOW; i < HIGH; i++){
    analogWrite(RED, i);
    analogWrite(BLUE, i);
    analogWrite(GREEN, i);
    delay(10);
  }
  for(int i = HIGH; i > LOW; i--){
    analogWrite(RED, i);
    analogWrite(BLUE, i);
    analogWrite(GREEN, i);
    delay(10);
  }
}


void seizuretime(){
int x = 0;
while(x < 20){
  analogWrite(RED, HIGH);
  analogWrite(GREEN, HIGH);
  analogWrite(BLUE, HIGH);
  delay(10);
  analogWrite(RED, LOW);
  analogWrite(GREEN, LOW);
  analogWrite(BLUE, LOW);
  delay(100);
  x++;
  }
}
