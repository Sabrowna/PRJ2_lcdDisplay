using System;
using System.Collections.Generic;
using System.Text;

namespace DTO2
{
    [Serializable]
    public class DTO_EKGMåling
    {
        public string MedarbejderID { get; set; }
        public string BorgerCPR { get; set; }
        public DateTime StarttidspunktMåling { get; set; }
        public double[] RåData = new double[3000];

        public int AntalMålepunkter { get; set; }
        public double SampleRateHz { get; set; }

        public DTO_EKGMåling(string MedarbejderID_, string BorgerCPR_, DateTime StarttidspunktMåling_, double[] Rådata_, int AntalMålepunkter_, double SampleRateHz_)
        {
            MedarbejderID = MedarbejderID_;
            BorgerCPR = BorgerCPR_;
            StarttidspunktMåling = StarttidspunktMåling_;
            for (int i = 0; i < Rådata_.Length; i++)
            {
                RåData[i] = Rådata_[i];
            }
            RåData = Rådata_;
            AntalMålepunkter = AntalMålepunkter_;
            SampleRateHz = (1/(SampleRateHz_/1000)); 
        }
    }
}
