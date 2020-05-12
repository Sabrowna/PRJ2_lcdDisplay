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
            return BatteryStatus;
            
        }

        public bool Charging() // Besked fra præsentationslaget
        {
            bool ChargeBattery = localDBRef.ChargeBattery;

            return ChargeBattery;

        }
        
    }

}

            

    

