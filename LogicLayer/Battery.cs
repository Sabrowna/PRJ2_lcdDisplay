﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using DataLayer2;
using DTO;



namespace LogicLayer
{
    public class Battery
    {
        IData localDataRef;

        
        private double BatteryStatus {get; set;}

        public Battery()
        {
               localDataRef = new LocalDataFile();

        }

        public double ShowBatteryStatus()
        {
            // Sæt BatteryStatus 
            BatteryStatus = localDataRef.GetBatteryStatus();
            return BatteryStatus;
            
        }

        public bool Charging() // Besked fra præsentationslaget
        {
            bool chargingBattery = localDataRef.ChargingBattery();

            return chargingBattery;

        }
        
    }

}

            

    

