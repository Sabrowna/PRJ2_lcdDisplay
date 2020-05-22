using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO2;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace DataLayer2
{   
    /// <summary>
    /// Kommunikation med tekstfiler. 
    /// </summary>
    public class LocalDataFile : IData
    {
        private FileStream input;
        private StreamReader reader;
        private FileStream output;
        private StreamWriter writer;
        private BinaryFormatter formatter;

        double BatteryStatus { get; set; }

       public LocalDataFile()
        { }

        public bool VerifyEmployeeId(string EmployeeId)
        {
            bool result = false;

            input = new FileStream("MedarbejderID.txt", FileMode.Open, FileAccess.Read);
            reader = new StreamReader(input);

            string inputRecord;
            string[] inputFields;

            while ((inputRecord = reader.ReadLine()) != null)
            {
                inputFields = inputRecord.Split(';');

                if (inputFields[0] == EmployeeId)
                {
                    result = true;
                    break;
                }
            }
            reader.Close();

            return result;
        }


        public int CountID()
        {
            int Retur = 0;

            input = new FileStream("EKGMaalingerTEST.txt", FileMode.Open, FileAccess.Read);
            reader = new StreamReader(input);

            string inputRecord;

            while ((inputRecord = reader.ReadLine()) != null)
            {
                Retur++;
            }

            reader.Close();

            Retur++;

            return Retur;
        }

        public void InsertEKGMeasurement(DTO_EKGMåling nyMåling)
        {
                     
            if (File.Exists("EKGMaalingerTEST.txt") == false)
            {
                output = new FileStream("EKGMaalingerTEST.txt", FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(output);
                foreach (double item in nyMåling.RåData)
                {
                    writer.Write(item + ";");
                }
                writer.WriteLine(nyMåling.MedarbejderID + ";" + nyMåling.BorgerCPR + ";" + nyMåling.StarttidspunktMåling + ";" + nyMåling.AntalMålepunkter + ";" + nyMåling.SampleRateHz);
            }

            output = new FileStream("EKGMaalingerTEST.txt", FileMode.Append, FileAccess.Write);
            writer = new StreamWriter(output);
            
            foreach (double item in nyMåling.RåData) //Vi indlæser først alle værdier fra array (de første 2500 værdier, adskilt med ;)
            {
                writer.Write(item + ";");
            }
            //Indsætter derefter resterende værdier
            writer.WriteLine(nyMåling.MedarbejderID + ";" + nyMåling.BorgerCPR + ";" + nyMåling.StarttidspunktMåling + ";" + nyMåling.AntalMålepunkter + ";" + nyMåling.SampleRateHz);
            writer.Close();
        }


        public bool ChargingBattery()
        {
            bool onOff = false;

            Random random = new Random();
            int number = random.Next(0, 11);

            if(number % 6 == 0)
            {
                onOff = true;
            }

            // onOff = false; // Linje kun til test
            return onOff;
        }
        //FRA JACOB
        public void NewRecord(double level, double voltage, double current, DateTime date)
        {
            //uploade new record of current Ah, voltage, current and time to database or datafile
            FileStream output = new FileStream("batteryLevel.txt", FileMode.Create, FileAccess.Write);
            StreamWriter fileWriter = new StreamWriter(output);
            fileWriter.WriteLine(level + ";" + voltage + ";" + current + ";" + date);
            fileWriter.Close();
        }

        //FRA JACOB
        public DTO_BatteryLevel GetRecord()
        {
            if (File.Exists("batteryLevel.txt") == false)
            {
                output = new FileStream("batteryLevel.txt", FileMode.Create, FileAccess.Write);
                StreamWriter fileWriter = new StreamWriter(output);
                fileWriter.WriteLine(2000 + ";" + 0 + ";" + 0 + ";" + DateTime.Now);
                fileWriter.Close();
            }
            input = new FileStream("batteryLevel.txt", FileMode.Open, FileAccess.Read);
            reader = new StreamReader(input);
            DTO_BatteryLevel result = new DTO_BatteryLevel(0, 0, 0, DateTime.Now);

            string inputRecord;
            string[] inputFields;

            while ((inputRecord = reader.ReadLine()) != null)
            {
                inputFields = inputRecord.Split(';');
                result = new DTO_BatteryLevel(Convert.ToDouble(inputFields[0]), Convert.ToDouble(inputFields[1]), Convert.ToDouble(inputFields[2]), Convert.ToDateTime(inputFields[3]));
            }

            reader.Close();
            return result;
        }

        public double ShowBatteryStatusTEST()
        {
            BatteryStatus = 15;
            return BatteryStatus;
        }
    }
}

