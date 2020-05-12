using System;
using System.Threading;
using System.Collections.Generic;
using DataLayer;
using DTO;
using RaspberryPiCore.ADC;

namespace LogicLayer
{ 
    public class Ekg_Record 
    {
        private LocalDB localDBRef;
        private ADC1015 adc;

        public Ekg_Record()
        {
            localDBRef = new LocalDB();
            adc = new ADC1015();
        }

        
        double sample = 0; //En sample er ét punkt
        int antalSamples = 12000; //Hvor mange samples skal der være i løbet af målingen
        string starttidspunkt;
        private List<double> ekgRawData;
        int samplerate = 5; //Variabel til at regulere hvor længe der går mellem hver måling

        public bool StartEkgRecord() //Start modtagelse af signal fra elektroderne
        {
            bool ekgRecordEnd = false;
            starttidspunkt = DateTime.Now.ToString("dd MMMM yyyy HH: mm:ss");
            ekgRawData = new List<double>();

            for (int i = 0; i < antalSamples; i++)
            {
                sample = (adc.readADC_SingleEnded(0) / 2048.0) * 6.144; //Konverterer fra adc til strøm (eller omvendt)
                ekgRawData.Add(sample);

                Thread.Sleep(samplerate);
            }
            ekgRecordEnd = true;
            return ekgRecordEnd;
        }
        
        public void CreateEKGDTO(string EmployeeIdAsString, string SocSecNumberAsString) //Modtager disse to fra presentationlayer
        {
            StartEkgRecord();
            
          
            DTO_EKGMåling nyMåling = new DTO_EKGMåling(EmployeeIdAsString,SocSecNumberAsString,Convert.ToDateTime(starttidspunkt),ekgRawData,antalSamples,samplerate);

            localDBRef.InsertEKGMeasurement(nyMåling); //Sender målingen til databasen
        }
        
        //public void SendToDB(DTO_EKGMåling NyMåling)
        //{
        //    localDBRef.InsertEKGMeasurement(NyMåling);
        //}

        
        public string GetReceipt() //TILRETTES til databasen
        {
            int id = localDBRef.Retur;
            
            return Convert.ToString(id);
        }
        
    }
}
