﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using DataLayer2;
using RaspberryPiCore.ADC;
using DTO2;



namespace LogicLayer
{
    /// <summary>
    /// Varetager registrering og behandling af batteridata.
    /// </summary>
    public class Battery
    {
        /// <summary>
        /// Reference til DataLayer.
        /// </summary>
        IData localDataRef;

        private double voltage;
        private double current;
        private double batteryLevel;
        private const int maxVoltage = 5;
        private DTO_BatteryLevel batteryLevelRecord;

        /// <summary>
        /// Property til get/set af værdier for batteristatus.
        /// </summary>
        private double BatteryStatus {get; set;}
        private ADC1015 adc;
        

        /// <summary>
        /// Constructor til klassen. Initialiserer referencer. 
        /// </summary>
        public Battery()
        {
            localDataRef = new LocalDataFile();
            batteryLevel = AssumeLevel();
            batteryLevelRecord = GetRecord(); //KOMMENTERES IND EFTER FØRSTE GANG 
            if (batteryLevel != 0)
            {
                localDataRef.NewRecord(batteryLevel, DateTime.Now);
            }
            else
            {
                batteryLevelRecord.Date = DateTime.Now;
            }

            adc = new ADC1015();
        }

        /// <summary>
        /// Returnerer true hvis opladning er i gang. Indeholder reference til DataLayer.
        /// </summary>
        /// <returns>Returnerer true eller false, afhængig af om oplader er tilstluttet eller ej.</returns>
        public bool Charging() // Besked fra præsentationslaget
        {
            bool chargingBattery = localDataRef.ChargingBattery();

            return chargingBattery;

        }
        
        //JACOBS BATTERISTATUS KLASSER
        /// <summary>
        /// Returnerer spændingen på batteriet.
        /// </summary>
        /// <returns>Returnerer spændingsværdi som double. </returns>
        public double GetVoltage()
        {
            double voltageInput = 3480;//adc.readADC_SingleEnded(1);
            voltage = 2 * maxVoltage * (voltageInput / (Math.Pow(2, 12) / 100)) / 100;
            return voltage;
        }

        /// <summary>
        /// Returnere strømtrækket i systemet. 
        /// </summary>
        /// <returns>Returnerer strømværdi som double. </returns>
        public double GetCurrent()
        {
            double currentInput = 1000;//adc.readADC_SingleEnded(2);
            voltage = (currentInput / Math.Pow(2, 12)) * maxVoltage;
            current = ((voltage * 100 / 0.1)/4400)*1000;//maxVoltage * 1000 * (currentInput / (Math.Pow(2, 12) / 100)) / 100 * 100 / 4400;
            return current;
        }
        /// <summary>
        /// Returnere status på batteriet, angivet i %
        /// </summary>
        /// <returns>Status på batteri (resterende kapacitet) som double. </returns>
        public double ShowBatteryStatus()
        {
            current = GetCurrent();
            batteryLevel = AssumeLevel();
            if (batteryLevel != 0)
            {
                localDataRef.NewRecord(batteryLevel, DateTime.Now);
            }
            batteryLevelRecord = GetRecord();
            batteryLevelRecord.BatteryLevel = batteryLevelRecord.BatteryLevel - current * (DateTime.Now - batteryLevelRecord.Date).TotalSeconds / 3600;
            localDataRef.NewRecord(batteryLevelRecord.BatteryLevel, DateTime.Now);
            return batteryLevelRecord.BatteryLevel/2000*100;
        }

      
        /// <summary>
        /// Hver gang denne metode køres bliver batteriniveauet sat ud fra batteriets spænding.
        /// Hvis batteriniveauet er mellem ca. 10-90% vil det sidst huskede niveau, blive brugt til at udregne det nuværende
        /// </summary>
        /// <returns>Værdi for batteristatus</returns>
        public int AssumeLevel()
        { 
            voltage = GetVoltage();
            int batteryLevel = 0;
            if (voltage > 9.0)
            {
                batteryLevel = 2000;
            }
            else if (voltage > 8)
            { }
            else if (voltage > 3)
            {
                batteryLevel = 200;
            }
            else
            {
                batteryLevel = 100;
            }
            return batteryLevel;
        }

        /// <summary>
        /// Metode anvendt til test. Returnerer status på batteri angivet i %.
        /// </summary>
        /// <returns>Aktuelle status på batteri.</returns>
        public double ShowBatteryStatusTEST() // Metode kun til test
        {
            return localDataRef.ShowBatteryStatusTEST();
        }

        /// <summary>
        /// Ny registrering af parametre der indgår i DTO_BatteryLevel.
        /// </summary>
 

        /// <summary>
        /// Returnerer DTO med data om batteri. 
        /// </summary>
        /// <returns>DTO med værdier for batteri. </returns>
        public DTO_BatteryLevel GetRecord()
        {
            return localDataRef.GetRecord();
        }

       
    }

}

            

    

