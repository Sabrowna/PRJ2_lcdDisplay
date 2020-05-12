using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using DataLayer;


namespace LogicLayer
{
    public class Battery
    {

        
        private LocalDB localDBRef = new LocalDB();
        private double BatteryStatus {get; set;}

        public Battery()
        {
                       

        }

        public double ShowBatteryStatus()
        {
            // Sæt BatteryStatus 
            BatteryStatus = localDBRef.GetBatteryStatus();
            if (BatteryStatus >= 0 && BatteryStatus <= 20)
            {
                
            }

            if (BatteryStatus > 20 && BatteryStatus <= 50)
            {
                
            }

            if (BatteryStatus > 50)
            {
                
            }

            return BatteryStatus;
            
        }

        public bool Charging(bool isCharging) // Besked fra præsentationslaget
        {
           while(isCharging == true) // Så længe oplader er tilslutte (bool == true), køres løkken her.
            // on/off kombineret med Thread.Sleep på red1, skulle gerne få den til at blinke
            // som indikation på at opladning er i gang. 
           {
                red1.on();
                ShowBatteryStatus(); // 
                Thread.Sleep(500);
                red1.off();
            

           }

            return isCharging;

        }
        
    }

}

            

    

