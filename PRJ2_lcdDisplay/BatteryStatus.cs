using System;
using System.Collections.Generic;
using System.Text;
using LogicLayer;
using RaspberryPiCore.LCD;

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
            }

            if (batteryRef.ShowBatteryStatus() >= 20 && batteryRef.ShowBatteryStatus() < 50)
            {
                lcd.lcdSetBackLight(0, 0, 250);
            }


            if (batteryRef.ShowBatteryStatus() >= 50)
            {
                lcd.lcdSetBackLight(0, 250, 0);
            }
        }
        /*
            while (batteryRef.ShowBatteryStatus() < 20) // Så længe batteristatus er lav, bliver systemet i denn løkke
            {
                lcd.lcdGotoXY(0, 0);
                lcd.lcdPrint("Enhed deaktiveret   Batteristatus lav   Tilslut oplader");
            }

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
