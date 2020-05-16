using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTO
{
    [Serializable]
    public class DTO_EKGMåling
    {
        public string MedarbejderID { get; set; }
        public string BorgerCPR { get; set; }
        public DateTime StarttidspunktMåling { get; set; }
        public List<double> RåData { get; set; }

        public int AntalMålepunkter { get; set; }
        public double SampleRateHz { get; set; }


        public DTO_EKGMåling(string MedarbejderID_,string BorgerCPR_, DateTime StarttidspunktMåling_,List<double> Rådata_, int AntalMålepunkter_, double SampleRateHz_)
        {
            MedarbejderID = MedarbejderID_;
            BorgerCPR = BorgerCPR_;
            StarttidspunktMåling = StarttidspunktMåling_;
            RåData = Rådata_;
            AntalMålepunkter = AntalMålepunkter_;
            SampleRateHz = SampleRateHz_;
        }
    }
}
