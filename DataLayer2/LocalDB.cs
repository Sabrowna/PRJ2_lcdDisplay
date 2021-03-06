﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using DTO2;
namespace DataLayer2
{
    /// <summary>
    /// Indeholder klasser der håndterer kommunikation med database og filer. 
    /// Derudover interfacet IData der implementeres i begge klasser.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    {
    }
    /// <summary>
    /// Direkte kommunikation med databasen.
    /// </summary>
    public class LocalDB : IData
    {
       /// <summary>
       /// Reference til objekt af klassen.
       /// </summary>
        private FileStream input;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private StreamReader reader;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private FileStream output;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private SqlConnection conn;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private SqlDataReader rdr;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private SqlCommand cmd;
        /// <summary>
        /// Tekststreng til hurtig indtastning i SqlCommand.
        /// </summary>
        private const string db = "F20ST2ITS2201908775";
        /// <summary>
        /// Get/Set for værdien af batteristatus.
        /// </summary>
        public double BatteryStatus { get; set; }
        /// <summary>
        /// Klassens constructor. Initialiserer referencen til SqlConnection. 
        /// </summary>
        public LocalDB()
        {
            conn = new SqlConnection("Data Source=10.10.7.72\\SQL_Local;Initial Catalog=" + db + ";User ID=" + db + ";Password=" + db + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        }

        /// <summary>
        /// Undersøger om det indtastede medarbejderID forefindes i databasen.
        /// </summary>
        /// <param name="EmployeeId">Parameter modtaget fra indtastning. </param>
        /// <returns>Returnerer default false. Hvis medarbejderID  findes i databasen, returneres true.</returns>
        public bool VerifyEmployeeId(string EmployeeId)
        {
            bool result = false;

            // Hent kolonnen SP_MedarbejderID fra tabellen SP_NyeEkger
            //conn = new SqlConnection("Data Source=10.10.7.72\\SQL_Local;Initial Catalog=" + db + ";User ID=" + db + ";Password=" + db + ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            
            cmd = new SqlCommand("Select * MedarbejderID from db_owner.SP_MedarbejderID", conn);
            
            conn.Open();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            while (rdr.Read()) // Så længe der er data at læse, undersøg om indkomne data matcher medsendte parameter employeeId
            {
                if (Convert.ToString(rdr) == EmployeeId)
                    result = true;
                break;
            }

            conn.Close();

            return result;

        }

        /// <summary>
        /// Kontrollerer antallet af eksisterende målinger i databasen.
        /// </summary>
        /// <returns>Returnerer værdien for måleID på næstkommende entry.</returns>
        public int CountID()
        {
            int Retur;

            cmd = new SqlCommand("Select Count(*) from db_owner.SP_NyeEkger", conn);
            conn.Open();

            Retur = (int)cmd.ExecuteScalar();
            return Retur;
        }

        /// <summary>
        /// Indlæser DTO objekt i databasen.
        /// <example>
        /// <code>
        /// 1. conn.Open()
        /// 2. Opret lokalt stringparamater(INSERT INTO SP_NyeEkger ())
        /// 3a Kør cmd.
        /// 3. Opret reference parametre fra DTO parametrene.
        /// 4. Eksempel:
        /// 
        ///             cmd.Parameters.AddWithValue("@data",
        ///             nyMåling.RåData.SelectMany(value =>
        ///             BitConverter.GetBytes(value)).ToArray());
        /// 
        /// 5. conn.Close()
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="nyMåling">Parameter modtaget fra LogicLayer.</param>
        public void InsertEKGMeasurement(DTO_EKGMåling nyMåling) // Indlæs DTO her med de respektive data. Set vores værdier ind i en tabel i SQL server
        { 
            conn.Open();
           
            string insertStringParam = $"INSERT INTO SP_NyeEkger ([raa_data],[id_medarbejder],[borger_cprnr],[start_tidspunkt],[antal_maalepunkter],[samplerate_hz]) OUTPUT INSERTED.id_måling VALUES(@data, @employeeID, @socSecNb, @startTime, @antalMålePkt, @hz)";
            
            using (SqlCommand cmd = new SqlCommand(insertStringParam, conn))
            {
                //Tilføjer vores rådata til et BLOB objekt
                
                cmd.Parameters.AddWithValue("@data",
                nyMåling.RåData.SelectMany(value =>
                BitConverter.GetBytes(value)).ToArray());

                cmd.Parameters.AddWithValue("@employeeID", (nyMåling.MedarbejderID));
                cmd.Parameters.AddWithValue("@socSecNb", (nyMåling.BorgerCPR));
                cmd.Parameters.AddWithValue("@startTime", (nyMåling.StarttidspunktMåling));
                cmd.Parameters.AddWithValue("@antalMålePkt", (nyMåling.AntalMålepunkter));
                cmd.Parameters.AddWithValue("@hz", (nyMåling.SampleRateHz));

            }
            conn.Close();
        }
        /// <summary>
        /// Genererer ved hjælp af Random.Next() en bool. 
        /// Indikerer om oplader er tilsluttet. Metoden er oprettet grundet Corona, 
        /// da denne funktion ellers skulle have været implementeret i hardware. 
        /// </summary>
        /// <returns>Returnerer true hvis oplader er tilsluttet. </returns>
        public bool ChargingBattery()
        {
            bool onOff = false;
 
            Random random = new Random();
            int number = random.Next(0, 11);

            if (number % 6 == 0)
            {
                onOff = true;
            }

            // onOff = false; // Linje kun til test
            return onOff;
        }

        //FRA JACOB
        /// <summary>
        /// Uploader ny registrering af de fire parametre til tekstfil. 
        /// </summary>
        /// <param name="level"> </param>
        /// <param name="date"> </param>
        public void NewRecord(double level, DateTime date)
        {
            //uploade new record of current Ah and time to database or datafile
            FileStream output = new FileStream("batteryLevel.txt", FileMode.Create, FileAccess.Write);
            StreamWriter fileWriter = new StreamWriter(output);
            fileWriter.WriteLine(level + ";" + date);
            fileWriter.Close();
        }

        //FRA JACOB
        /// <summary>
        /// Opretter DTO med de senest registrerede værdier for batteri.
        /// </summary>
        /// <returns>Returnerer DTO med værdier registreret ved seneste måling.</returns>
        public DTO_BatteryLevel GetRecord()
        {
            if (File.Exists("batteryLevel.txt") == false)
            {
                output = new FileStream("batteryLevel.txt", FileMode.Create, FileAccess.Write);
                StreamWriter fileWriter = new StreamWriter(output);
                fileWriter.WriteLine(2000 + ";" + DateTime.Now);
                fileWriter.Close();
            }
            input = new FileStream("batteryLevel.txt", FileMode.Open, FileAccess.Read);
            reader = new StreamReader(input);
            DTO_BatteryLevel result = new DTO_BatteryLevel(0, DateTime.Now);

            string inputRecord;
            string[] inputFields;

            while ((inputRecord = reader.ReadLine()) != null)
            {
                inputFields = inputRecord.Split(';');
                result = new DTO_BatteryLevel(Convert.ToDouble(inputFields[0]), Convert.ToDateTime(inputFields[1]));
            }

            reader.Close();
            return result;
        }
            /// <summary>
            /// Testmetode der anvendes til at simulere bestemte værdier for batteri. 
            /// </summary>
            /// <returns>Værdi for batteriets kapacitet som double.</returns>
            public double ShowBatteryStatusTEST()
        {
            BatteryStatus = 60;
            return BatteryStatus;
        }
    }
}
