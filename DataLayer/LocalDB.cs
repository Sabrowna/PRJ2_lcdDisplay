using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using DTO;


namespace DataLayer
{
    public class LocalDB : IData
    {
        private SqlConnection conn;
        private SqlConnection conn_online;
        private SqlDataReader rdr;
        private SqlCommand cmd;
        private const string db = "F20ST2ITS2201908775";
        //private const string source = "SABROWNA\SAB";


        //public int Retur { get; set; }
        public double BatteryStatus { get; set; }
       
        public LocalDB()
        {
            //conn = new SqlConnection("Data Source=10.10.7.72\\SQL_local; Initial Catalog =" + db + "; User ID =" + db + "; Password =" + db + "; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            //conn = new SqlConnection("Data Source=10.10.7.72\\SQL_local; Initial Catalog = SQL_local; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            //conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn = new SqlConnection("Data Source = 10.10.7.72\\SQL_local; Initial Catalog = SQL_local; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
        }

       


        // Undersøg om CPR findes i LokalDB - tabel SP_NyeEkger. Returner bool. 
        public bool CheckDBForCPR(string socSecNb)
        {
            bool result = false;

            // Hent kolonnen borger_cprnr fra tabellen SP_NyeEkger

            cmd = new SqlCommand("Select borger_cprnr from db_owner.SP_NyeEkger", conn);
            conn.Open();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            while (rdr.Read()) // Så længe der er data at læse, undersøg om indkomne data matcher medsendte parameter socSecNb
            {
                if (Convert.ToString(rdr) == socSecNb)
                    result = true;
                break;

            }

            conn.Close();

            return result;

        }

        // Undersøg om EmployeeID findes i LokalDB - tabel MedarbejderID.
        public bool CheckDBForEmployeeId(string EmployeeId)
        {
            bool result = false;

            // Hent kolonnen borger_cprnr fra tabellen SP_NyeEkger

            cmd = new SqlCommand("Select MedarbejderID from db_owner.MedarbejderID", conn);
            conn.Open();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            while (rdr.Read()) // Så længe der er data at læse, undersøg om indkomne data matcher medsendte parameter socSecNb
            {
                if (Convert.ToString(rdr) == EmployeeId)
                    result = true;
                break;
            }

            conn.Close();

            return result;

        }

        public int CountID()
        {
            int Retur;

            cmd = new SqlCommand("Select Count(*) from db_owner.SP_NyeEkger", conn);
            conn.Open();

            //rdr = cmd.ExecuteReader();
            //rdr.Read();

            Retur = (int)cmd.ExecuteScalar();
            return Retur;
        }


        // Udkommenteret 14/5 for test
        public void InsertEKGMeasurement(DTO_EKGMåling nyMåling) // Indlæs DTO her med de respektive data. Set vores værdier ind i en tabel i SQL server
        {
            
                    
            //SqlConnection conn;
            //const String db = "F20ST2ITS2201908775";
            //conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
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

                // Retur = (int)cmd.ExecuteScalar();
            }
            conn.Close();

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

            if (number % 6 == 0)
            {
                onOff = true;
            }
            */

            return onOff;
        }



    }
}



