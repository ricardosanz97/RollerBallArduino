#include "SoftwareSerial.h"
#include "SerialCommand.h"
// I2Cdev and MPU6050 must be installed as libraries, or else the .cpp/.h files
// for both classes must be in the include path of your project
#include "I2Cdev.h"
#include "MPU6050.h"

// Arduino Wire library is required if I2Cdev I2CDEV_ARDUINO_WIRE implementation
// is used in I2Cdev.h
#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
    #include "Wire.h"
#endif

// class default I2C address is 0x68
// specific I2C addresses may be passed as a parameter here
// AD0 low = 0x68 (default for InvenSense evaluation board)
// AD0 high = 0x69
MPU6050 accelgyro;
//MPU6050 accelgyro(0x69); // <-- use for AD0 high

SerialCommand sCmd;

//Ratios de conversion
#define A_R 16384.0
#define G_R 131.0

int16_t ax, ay, az;
int16_t gx, gy, gz;

float Acc[2];
float Gy[2];
float Angle[2];

#define OUTPUT_READABLE_ACCELGYRO
#define LED_PIN 13
void setup() {

    #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
        Wire.begin();
    #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
        Fastwire::setup(400, true);
    #endif

    // initialize serial communication
    Serial.begin(9600);

    // initialize device
    Serial.println("Initializing I2C devices...");
    accelgyro.initialize();

    // verify connection
    Serial.println("Testing device connections...");
    Serial.println(accelgyro.testConnection() ? "MPU6050 connection successful" : "MPU6050 connection failed");

    pinMode(LED_PIN, OUTPUT);
}

void loop() {

  accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz); //obtenemos los valores por referencia del acelerometro y del giroscopio

//aplicamos una conversión para que lo que nos devuelva sean angulos naturales.
  Acc[1] = atan(-1*(ax/A_R)/sqrt(pow((ay/A_R),2) + pow((az/A_R),2)))*RAD_TO_DEG;
  Acc[0] = atan((ay/A_R)/sqrt(pow((ax/A_R),2) + pow((az/A_R),2)))*RAD_TO_DEG;

  Gy[0] = gx/G_R;
  Gy[1] = gy/G_R;

  Angle[0] = 0.1 *(Angle[0]+Gy[0]*0.010) + 0.9*Acc[0];
  Angle[1] = 0.1 *(Angle[1]+Gy[1]*0.010) + 0.9*Acc[1];

  //Serial.print("GY: "); Serial.print(Gy[1]);

//lo enviamos por el serial port con el formato separado por "_" para en Unity poder hacer un Split por la "_".
  Serial.print("Angle_X_:_"); Serial.print(Angle[0]); Serial.println("\n");
  Serial.print("Angle_Y_:_"); Serial.print(Angle[1]); Serial.println("\n");


  delay(100); //metemos un delay de 0.1 segundos entre cada envio.
  // display tab-separated accel/gyro x/y/z values
  /*
  Serial.print("a/g:\t");
  Serial.print("ax = ");
  Serial.print(ax); Serial.print("\t");
  Serial.print("ay = ");
  Serial.print(ay); Serial.print("\t");
  Serial.print("az = ");
  Serial.print(az); Serial.print("\t");
  Serial.print("gx = ");
  Serial.print(gx); Serial.print("\t");
  Serial.print("gy = ");
  Serial.print(gy); Serial.print("\t");
  Serial.print("gz = ");
  Serial.println(gz);
  */
}

//void pingHandler (){

  //delay(1000);
  //Serial.println("PONG");
//}
