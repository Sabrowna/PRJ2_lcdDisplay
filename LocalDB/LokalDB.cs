using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using DTO;





namespace LocalDB
{
    public class LokalDB
    {
        private SqlConnection conn;
        private SqlDataReader rdr;
        private SqlCommand cmd;
        private const string db = "F20ST2ITS2201908775";
        public int Retur { get; set; }
        
        public LokalDB()
        {
            conn = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog =" + db + "; User ID =" + db + "; Password =" + db + "; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            
        }


        // Undersøg om CPR findes i LokalDB - tabel SP_NyeEkger. Returner bool. 
        public bool checkDBForCPR(string socSecNb)
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
        public bool checkDBForEmployeeId(string EmployeeId)
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

        public int CountRows()
        {
            int NumberOfRows;

            cmd = new SqlCommand("Select Count(*) from db_owner.SP_NyeEkger", conn);
            conn.Open();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            NumberOfRows = Convert.ToInt32(rdr.Read()); // Er konverteringen nødvendig? 

            return NumberOfRows;

        }


        public int InsertEKGMeasurement(DTO_EKGMåling nyMåling) // Indlæs DTO her med de respektive data. Set vores værdier ind i en tabel i SQL server
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";
            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
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

                Retur = (int)cmd.ExecuteScalar();
            }
            conn.Close();

            return Retur;
        }
    }
}

       
    


