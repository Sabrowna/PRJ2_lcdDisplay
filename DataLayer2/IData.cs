using System;
using DTO2;

namespace DataLayer2
{
    public interface IData
    {
        //bool CheckDBForCPR(string socSecNb);
        
        bool CheckDBForEmployeeId(string EmployeeId);
        
        int CountID();

        void InsertEKGMeasurement(DTO_EKGMåling nyMåling);

        //double GetBatteryStatus();

        void newRecord(double level, double voltage, double current, DateTime date);

        DTO_BatteryLevel getRecord();
        bool ChargingBattery();
        
    }

}
