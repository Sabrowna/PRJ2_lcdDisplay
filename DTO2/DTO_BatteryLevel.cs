using System;
using System.Collections.Generic;
using System.Text;

namespace DTO2
{
    /// <summary>
    /// Varetager oprettelse af DTO indeholdende oplysninger om batteristatus.
    /// </summary>
    public class DTO_BatteryLevel
    {
        /// <summary>
        /// Property til BatteryLevel parameter i DTO objekt. 
        /// </summary>
        public double BatteryLevel { get; set; }
        /// <summary>
        /// Property til Voltage parameter i DTO objekt. 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Constructor til objekt af klassen. 
        /// </summary>
        /// <param name="batteryLevel"></param>
        /// <param name="date"></param>
        public DTO_BatteryLevel(double batteryLevel, DateTime date)
        {
            BatteryLevel = batteryLevel;
            Date = date;
        }
    }
}
