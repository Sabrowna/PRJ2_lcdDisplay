using System;
using DTO2;

namespace DataLayer2
{
    /// <summary>
    /// Interface til implementering i DataLayer. 
    /// </summary>
    public interface IData
    {
        /// <summary>
        /// Undersøger om det indtastede medarbejderID forefindes i databasen.
        /// </summary>
        /// <param name="EmployeeId">Parameter der kommer med fra indtastet værdi.</param>
        /// <returns>Returnerer true hvis medarbejderID forefindes i databasen.</returns>
        bool VerifyEmployeeId(string EmployeeId);
        /// <summary>
        /// Undersøger antallet af registrerede målinger. 
        /// </summary>
        /// <returns>Returnerer talværdi for næstkommende måling.</returns>
        int CountID();
        /// <summary>
        /// Indlæser DTO objekt i databasen.
        /// </summary>
        /// <param name="nyMåling">Modtager DTO objekt fra LogicLayer. </param>
        void InsertEKGMeasurement(DTO_EKGMåling nyMåling);
        /// <summary>
        /// Ny registrering af parametre til DTO
        /// </summary>
        /// <param name="level"></param>
        /// <param name="voltage"></param>
        /// <param name="current"></param>
        /// <param name="date"></param>
        void NewRecord(double level, double voltage, double current, DateTime date);

        /// <summary>
        /// Opretter DTO med de senest registrerede værdier for batteri.
        /// </summary>
        /// <returns>Returnerer DTO med værdier registreret ved seneste måling.</returns>
        DTO_BatteryLevel GetRecord();
       /// <summary>
       /// Afklarer om oplader er tilsluttet.
       /// </summary>
       /// <returns>Hvis oplader er tilsluttet returneres true.</returns>
        bool ChargingBattery();

        /// <summary>
        /// Anvendt til test.
        /// </summary>
        /// <returns>Returnerer værdi på batteri. Muligt at fastsætte for personen der udfører testen.</returns>
        double ShowBatteryStatusTEST();
      

    }

}
