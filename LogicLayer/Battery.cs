using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using RPI;

namespace LogicLayer
{
    public class Battery
    {
        private static RPi rpi;
        private static Led green1; // Svarer til den røde LED i vores UC
        private static Led green2; // Svarer til den gule LED i vores UC
        private static Led green3; // Svarer til den grønne LED i vores UC
        private static Led red1; // Svarer til den blå LED i vores UC
        

        public Battery()
        {
            rpi = new RPi();
            green1 = new Led(rpi, Led.ID.LD1);
            green2 = new Led(rpi, Led.ID.LD2);
            green3 = new Led(rpi, Led.ID.LD3);
            red1 = new Led(rpi, Led.ID.LD6);
            
        }

        public double ShowBatteryStatus(double Batterystatus)
        {
            if (Batterystatus >= 0 && Batterystatus <= 20)
            {
                green1.on();
            }

            if (Batterystatus > 20 && Batterystatus <= 50)
            {
                green1.on();
                green2.on();
            }

            if (Batterystatus > 50)
            {
                green1.on();
                green2.on();
                green3.on();
            }

            return Batterystatus;
            
        }

        public bool Charging(bool isCharging) // Besked fra præsentationslaget
        {
           while(isCharging == true) // Så længe oplader er tilslutte (bool == true), køres løkken her.
            // on/off kombineret med Thread.Sleep på red1, skulle gerne få den til at blinke
            // som indikation på at opladning er i gang. 
           {
                red1.on();
                ShowBatteryStatus();
                Thread.Sleep(500);
                red1.off();
            

           }

            return isCharging;

        }
    }

}

            

    

