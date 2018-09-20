#include "SoftwareSerial.h"
#include "SerialCommand.h"
SerialCommand sCmd;

void setup() {
  Serial.begin(9600); //esto define el baud rate
  while (!Serial);

  sCmd.addCommand("PING", pingHandler); //definimos comando "PING",
  //cuando recibimos este comando de unity, devolvemos un "PONG"
  

}

void loop() {
  
  if (Serial.available() > 0){
    sCmd.readSerial(); //continuosly reading from serial port
  }
}

void pingHandler (){

  delay(1000);
  Serial.println("PONG");
}
