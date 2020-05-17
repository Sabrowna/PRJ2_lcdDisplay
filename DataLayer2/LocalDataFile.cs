using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO2;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlTypes;

namespace DataLayer2
{
    public class LocalDataFile : IData
    {
        private FileStream input;
        private StreamReader reader;
        private FileStream output;
        private StreamWriter writer;
        private BinaryFormatter formatter;

        public int Retur { get; set; }
        public double BatteryStatus { get; set; }

        
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

            
            input = new FileStream(@"\\C:\SMPRJ2_Filer\MedarbejderID.txt", FileMode.Open, FileAccess.Read);
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
            result = true;

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
            output = new FileStream("EKGMaaling3.txt", FileMode.OpenOrCreate, FileAccess.Write);
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
            // Metode opbygget til testning som den står her.
            //Random random = new Random();
            //BatteryStatus = Convert.ToDouble(random.Next(60)); // Batterystatus sættes til min 0 max 60
            // BatteryStatus = random.Next(20, 60); // Min 20, max 60
            BatteryStatus = 30;
            return BatteryStatus;
        }

        public bool ChargingBattery()
        {
            bool onOff = false;

            /*
            Random random = new Random();
            int number = random.Next(0, 11);

            if(number % 6 == 0)
            {
                onOff = true;
            }
            */
            return onOff;
        }
    }
}

