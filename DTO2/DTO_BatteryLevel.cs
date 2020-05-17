using System;
using System.Collections.Generic;
using System.Text;

namespace DTO2
{
    public class DTO_BatteryLevel
    {
        public double BatteryLevel { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public DateTime Date { get; set; }

        public DTO_BatteryLevel(double batteryLevel, double voltage, double current, DateTime date)
        {
            BatteryLevel = batteryLevel;
            Voltage = voltage;
            Current = current;
            Date = date;
        }
    }
}
