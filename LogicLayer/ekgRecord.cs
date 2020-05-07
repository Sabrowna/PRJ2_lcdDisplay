using System;
using System.Threading;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LocalDB;
using DTO;
using System.Threading;


namespace LogicLayer
{
    
       
    public class ekgRecord 
    {
        public ADC1015 adc;
        public SerLCD lcd;
        public TWIST twist;
        public Display displayRef;
        public LokalDB localDBRef;

        public ekgRecord()
        {
            ADC1015 adc = new ADC1015();
            SerLCD lcd = new SerLCD();
            TWIST twist = new TWIST();
            displayRef = new Display();
            localDBRef = new LokalDB();
        }

        
        double sample = 0; //En sample er ét punkt
        int antalSamples = 12000; //Hvor mange samples skal der være i løbet af målingen
        //int rawEKG = 0;
        string starttidspunkt;
        public List<double> EkgRawData;
        private int samplerate = 5; //Variabel til at regulere hvor længe der går mellem hver måling

        public void StartEkgRecord() //Start modtagelse af signal fra elektroderne
        {
            starttidspunkt = DateTime.Now.ToString("dd MMMM yyyy HH: mm:ss") ;
            //rawEKG = adc.readADC_SingleEnded(0); //ADC'en modtager signalet fra elektroderne
            EkgRawData = new List<double>();

            for (int i = 0; i < antalSamples; i++)
            {
                sample = (adc.readADC_SingleEnded(0) / 2048.0) * 6.144; //Konverterer fra adc til strøm (eller omvendt)
                EkgRawData.Add(sample);

                Thread.Sleep(samplerate);
            }
            //return EkgRawData;
        }
        
        public DTO_EKGMåling CreateEKGDTO(string EmployeeIdAsString, string SocSecNumberAsString) //Modtager disse to fra presentationlayer
        {
            StartEkgRecord();
            //string EmployeeIdAsString = displayRef.EmployeeIdAsString;
            //string SocSecNumberAsString = displayRef.SocSecNumberAsString;
          
            DTO_EKGMåling NyMåling = new DTO_EKGMåling(EmployeeIdAsString,SocSecNumberAsString,Convert.ToDateTime(starttidspunkt),EkgRawData,antalSamples,samplerate);
          
            return NyMåling;
        }
        
        public void SendToDB(DTO_EKGMåling NyMåling)
        {
            localDBRef.InsertEKGMeasurement(NyMåling);
        }
        
    }
}
