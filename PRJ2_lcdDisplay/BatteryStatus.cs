using System;
using System.Collections.Generic;
using System.Text;
using LogicLayer;
using RaspberryPiCore.LCD;
using System.Threading;

namespace PresentationLayer
{
    public class BatteryStatus
    {
        public SerLCD lcd = new SerLCD();
        private Battery batteryRef = new Battery();


        public void ShowBatteryStatus() // Ved test kommenteres statements med ShowBatteryStatusTEST ind! 
        {
            double værdi = batteryRef.ShowBatteryStatus();
            //double værdi = (batteryRef.ShowBatteryStatusTEST());

            if (værdi < 20)                // Hvis batteristatus er lav jf. UC, udskrives nedenstående
            {

                lcd.lcdSetBackLight(200, 0, 0);


                for (int i = 0; i < 5; i++)
                {
                    lcd.lcdGotoXY(0, 0);
                    lcd.lcdPrint("Enhed deaktiveres   Batteristatus lav   Tilslut oplader");
                    Thread.Sleep(1000);
                    lcd.lcdClear();
                    Thread.Sleep(500);
                }

                lcd.lcdPrint("Slukker");
                Thread.Sleep(2000);
                lcd.lcdSetBackLight(0, 0, 0);
                lcd.lcdNoDisplay();


                //Environment.Exit(0);

            }

            if (værdi >= 20 && værdi < 50)
            {
                lcd.lcdSetBackLight(0, 250, 250);
            }


            if (værdi >= 50)
            {
                lcd.lcdSetBackLight(0, 200, 0);
            }
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint($"Batteristatus: {Convert.ToInt32(batteryRef.ShowBatteryStatus())} % ");
            Thread.Sleep(3000); // Venter i 3 sek. så det er muligt at se status på batteri både på display LED
        }

        public void ChargeBattery()
        {
            
            if (batteryRef.Charging() == true) // Så længe oplader er tilslutte (bool == true), køres løkken her. // som indikation på at opladning er i gang. 
            {
                for (int i = 0; i < 1; i++)
                {
                    lcd.lcdClear();
                    lcd.lcdGotoXY(0, 0);
                    lcd.lcdPrint($"Batteristatus: {batteryRef.ShowBatteryStatus()} %");
                    Thread.Sleep(3000);
                    lcd.lcdClear();
                    lcd.lcdGotoXY(0, 0);
                    lcd.lcdPrint("Batteriet oplades");
                    lcd.lcdGotoXY(0, 1);
                    lcd.lcdPrint("Systemet slukkes");
                    lcd.lcdGotoXY(0, 2);
                    lcd.lcdPrint("om 3 sekunder");
                    Thread.Sleep(3000);
                    lcd.lcdClear();
                    lcd.lcdNoDisplay();
                }
                Environment.Exit(0);
            }
        }
    }
}
