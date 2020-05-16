using System;
using DTO;

namespace DataLayer2
{
    public interface IData
    {
        bool CheckDBForCPR(string socSecNb);
        bool CheckDBForEmployeeId(string EmployeeId);

        int CountID();

        void InsertEKGMeasurement(DTO_EKGMåling nyMåling);

        double GetBatteryStatus();

        bool ChargingBattery();
    }

}
