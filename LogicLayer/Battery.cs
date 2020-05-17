using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using DataLayer2;
using RaspberryPiCore.ADC;
using DTO2;



namespace LogicLayer
{
    public class Battery
    {
        IData localDataRef;

        private double voltage = 0;
        private const int maxVoltage = 5;
        private double current;
        private DTO_BatteryLevel batteryLevelRecord;

        private double BatteryStatus {get; set;}
        private ADC1015 adc;
        public Battery()
        {
            localDataRef = new LocalDB();
            batteryLevelRecord = localDataRef.getRecord(); //KOMMENTERES IND EFTER FØRSTE GANG 
            batteryLevelRecord.Date = DateTime.Now; //KOMMENTERES IND EFTER FØRSTE GANG
            adc = new ADC1015();
        }


        public bool Charging() // Besked fra præsentationslaget
        {
            bool chargingBattery = localDataRef.ChargingBattery();

            return chargingBattery;

        }
        
        //JACOBS BATTERISTATUS KLASSER
        public double getVoltage()

        {
            double voltageInput = adc.readADC_SingleEnded(1);
            voltage = maxVoltage * (voltageInput / (Math.Pow(2, 12) / 100)) / 100;
            return voltage;
        }

        public double getCurrent()
        {
            double currentInput = adc.readADC_SingleEnded(2);
            voltage = maxVoltage * (currentInput / (Math.Pow(2, 12) / 100)) / 100;
            current = voltage * 100 / 380;
            return current;
        }

        public double ShowBatteryStatus()
        {
            //localDataRef.newRecord(2000, 0, 0, DateTime.Now); //KOMMENTERES UD EFTER FØRSTE GANG VI KØRER
            batteryLevelRecord = getRecord();
            newRecord();
            return batteryLevelRecord.BatteryLevel/2000*100;
        }
        public void newRecord()
        {
            current = getCurrent();
            batteryLevelRecord.BatteryLevel = batteryLevelRecord.BatteryLevel - current * (DateTime.Now - batteryLevelRecord.Date).TotalSeconds / 3600;
            localDataRef.newRecord(batteryLevelRecord.BatteryLevel, getVoltage(), getCurrent(), DateTime.Now);
        }

        public DTO_BatteryLevel getRecord()
        {
            return localDataRef.getRecord();
        }
    }

}

            

    

