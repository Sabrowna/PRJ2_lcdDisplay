using System;
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

        private double voltage = 0;
        private const int maxVoltage = 5;
        private double current;
        private DTO_BatteryLevel batteryLevelRecord;

        private double BatteryStatus {get; set;}
        private ADC1015 adc;
        

        /// <summary>
        /// Constructor til klassen. Initialiserer referencer. 
        /// </summary>
        public Battery()
        {
            localDataRef = new LocalDataFile();
            batteryLevelRecord = localDataRef.GetRecord(); //KOMMENTERES IND EFTER FØRSTE GANG 
            batteryLevelRecord.Date = DateTime.Now; //KOMMENTERES IND EFTER FØRSTE GANG
            adc = new ADC1015();
        }

        /// <summary>
        /// Returnerer true hvis opladning er i gang. Indeholder reference til DataLayer.
        /// </summary>
        /// <returns></returns>
        public bool Charging() // Besked fra præsentationslaget
        {
            bool chargingBattery = localDataRef.ChargingBattery();

            return chargingBattery;

        }
        
        //JACOBS BATTERISTATUS KLASSER
        /// <summary>
        /// Returnerer spændingen på batteriet.
        /// </summary>
        /// <returns></returns>
        public double GetVoltage()
        {
            double voltageInput = adc.readADC_SingleEnded(1);
            voltage = maxVoltage * (voltageInput / (Math.Pow(2, 12) / 100)) / 100;
            return voltage;
        }

        /// <summary>
        /// Returnere strømtrækket i systemet. 
        /// </summary>
        /// <returns></returns>
        public double GetCurrent()
        {
            double currentInput = adc.readADC_SingleEnded(2);
            voltage = maxVoltage * (currentInput / (Math.Pow(2, 12) / 100)) / 100;
            current = voltage * 100 / 380;
            return current;
        }
        /// <summary>
        /// Returnere status på batteriet, angivet i %
        /// </summary>
        /// <returns></returns>
        public double ShowBatteryStatus()
        {
            localDataRef.NewRecord(2000, 0, 0, DateTime.Now); //KOMMENTERES UD EFTER FØRSTE GANG
            batteryLevelRecord = GetRecord();
            NewRecord();
            return batteryLevelRecord.BatteryLevel/2000*100;
        }

        /// <summary>
        /// Metode anvendt til test. Returnerer status på batteri angivet i %.
        /// </summary>
        /// <returns></returns>
        public double ShowBatteryStatusTEST() // Metode kun til test
        {
            return localDataRef.ShowBatteryStatusTEST();
        }

        /// <summary>
        /// Ny registrering af parametre der indgår i DTO_BatteryLevel.
        /// </summary>
        public void NewRecord()
        {
            current = GetCurrent();
            batteryLevelRecord.BatteryLevel = batteryLevelRecord.BatteryLevel - current * (DateTime.Now - batteryLevelRecord.Date).TotalSeconds / 3600;
            localDataRef.NewRecord(batteryLevelRecord.BatteryLevel, GetVoltage(), GetCurrent(), DateTime.Now);
        }

        /// <summary>
        /// Returnerer DTO med data om batteri. 
        /// </summary>
        /// <returns></returns>
        public DTO_BatteryLevel GetRecord()
        {
            return localDataRef.GetRecord();
        }

       
    }

}

            

    

