using System;
using System.Threading;
using System.Collections.Generic;
using LocalDB;
using DTO;
using RaspberryPiCore.ADC;

namespace LogicLayer
{ 
    public class Ekg_Record 
    {
        private LokalDB localDBRef;
        private ADC1015 adc;

        public Ekg_Record()
        {
            localDBRef = new LokalDB();
            adc = new ADC1015();
        }

        
        double sample = 0; //En sample er ét punkt
        int antalSamples = 12000; //Hvor mange samples skal der være i løbet af målingen
        string starttidspunkt;
        public List<double> EkgRawData;
        private int samplerate = 5; //Variabel til at regulere hvor længe der går mellem hver måling

        public void StartEkgRecord() //Start modtagelse af signal fra elektroderne
        {
            starttidspunkt = DateTime.Now.ToString("dd MMMM yyyy HH: mm:ss");
            EkgRawData = new List<double>();

            for (int i = 0; i < antalSamples; i++)
            {
                sample = (adc.readADC_SingleEnded(0) / 2048.0) * 6.144; //Konverterer fra adc til strøm (eller omvendt)
                EkgRawData.Add(sample);

                Thread.Sleep(samplerate);
            }
        }
        
        public void CreateEKGDTO(string EmployeeIdAsString, string SocSecNumberAsString) //Modtager disse to fra presentationlayer
        {
            StartEkgRecord();
            //string EmployeeIdAsString = displayRef.EmployeeIdAsString;
            //string SocSecNumberAsString = displayRef.SocSecNumberAsString;
          
            DTO_EKGMåling NyMåling = new DTO_EKGMåling(EmployeeIdAsString,SocSecNumberAsString,Convert.ToDateTime(starttidspunkt),EkgRawData,antalSamples,samplerate);

            localDBRef.InsertEKGMeasurement(NyMåling); //Sender målingen til databasen
        }
        
        //public void SendToDB(DTO_EKGMåling NyMåling)
        //{
        //    localDBRef.InsertEKGMeasurement(NyMåling);
        //}

        public string GetReceipt() //TILRETTES til databasen
        {
            int id = localDBRef.Retur;
            //string receipt = $"Data er sendt med ID: {id}";

            return Convert.ToString(id);
        }
        
    }
}
