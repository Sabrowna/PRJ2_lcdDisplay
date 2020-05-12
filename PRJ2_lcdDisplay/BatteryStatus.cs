﻿using System;
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



        public void ChangeBackground()
        {
            if (batteryRef.ShowBatteryStatus() < 20) // Hvis batteristatus er lav jf. UC, udskrives nedenstående
            {
                lcd.lcdSetBackLight(250, 0, 0);

                lcd.lcdGotoXY(0, 0);
                for (int i = 0; i < 5; i++)
                {
                    lcd.lcdPrint("Enhed deaktiveret   Batteristatus lav   Tilslut oplader");
                    Thread.Sleep(1000);
                    lcd.lcdClear();
                    Thread.Sleep(500);
                }
                lcd.lcdNoDisplay();
                //Environment.Exit(lcd.lcdPrint(); //Kommentar exit code
            }

            if (batteryRef.ShowBatteryStatus() >= 20 && batteryRef.ShowBatteryStatus() < 50)
            {
                lcd.lcdSetBackLight(0, 0, 250);
            }


            if (batteryRef.ShowBatteryStatus() >= 50)
            {
                lcd.lcdSetBackLight(0, 250, 0);
            }

            lcd.lcdPrint($"Batteristatus: {batteryRef.ShowBatteryStatus()} %");
            Thread.Sleep(3000); // Venter i 3 sek. så det er muligt at se status på batteri både på display LED
        }

        public void ChargeBattery()
        {
            if (batteryRef.Charging() == true) // Så længe oplader er tilslutte (bool == true), køres løkken her. // som indikation på at opladning er i gang. 
            {
                lcd.lcdClear();
                lcd.lcdGotoXY(0, 1);
                lcd.lcdPrint($"Batteristatus: {batteryRef.ShowBatteryStatus()} %");
                Thread.Sleep(3000);
                lcd.lcdClear();
                lcd.lcdGotoXY(0, 1);
                lcd.lcdPrint("Batteriet oplades");
                Thread.Sleep(3000);
                Environment.Exit(0);
            }
            
        }
    /*


    }

        if (batteryRef.Charging(isCharging) == true) // Hvis opladning er i gang, køres og udskrives som nedenfor
        {
            while (batteryRef.Charging(isCharging) == true) // Så længe opladning er i gang, bliver systemet i denne løkke
            {
                lcd.lcdGotoXY(0, 1);
                lcd.lcdPrint("Opladning i gang    Måling ikke mulig");
            }
            */
}
}
