using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataLayer
{
    class LocalDataFile : IData
    {
        private FileStream input;
        private StreamReader reader;
        private FileStream output;
        private StreamWriter writer;
        private BinaryFormatter formatter;


        public int Retur { get; set; }
        public double BatteryStatus { get; set; }
        public bool ChargingBattery { get; set; } // Kode til at sætte værdien - hvorfra? 

        public LocalDataFile()
        {

        }

        public bool CheckDBForCPR(string socSecNb)
        {
            bool result = false;

            input = new FileStream("CPRNUmmer.txt", FileMode.Open, FileAccess.Read);
            reader = new StreamReader(input);

            string inputRecord;
            //string[] inputFields;

            while ((inputRecord = reader.ReadLine()) != null)
            {
                //inputFields = inputRecord.Split(';');

                if (inputRecord == socSecNb)
                {
                    result = true;
                    break;
                }
            }

            
            reader.Close();

            return result;

        }

        public bool CheckDBForEmployeeId(string EmployeeId)
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

            input = new FileStream("EKGMaaling.txt", FileMode.Open, FileAccess.Read);
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
            output = new FileStream("EKGMaaling.txt", FileMode.Append, FileAccess.Write);
            //writer = new StreamWriter(output);
            //writer.WriteLine(nyMåling.MedarbejderID + ";" + nyMåling.BorgerCPR + ";" + )

            formatter = new BinaryFormatter();
            formatter.Serialize(output, nyMåling);



            // writer.Close();

            output.Close();

            // Retur = CountID();

            // return Retur;

        }

        public double GetBatteryStatus()
        {
            BatteryStatus = 45;
            return BatteryStatus;
        }
    }
}
}
