using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DataLayer
{
    public interface IData
    {
        bool CheckDBForCPR(string socSecNb);
        bool CheckDBForEmployeeId(string EmployeeId);

        int CountID();

        void InsertEKGMeasurement(DTO_EKGMåling nyMåling);

        double GetBatteryStatus();
    }
}
