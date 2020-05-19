﻿using System;
using System.Threading;
using System.Collections.Generic;
using DataLayer2;
using DTO2;
using RaspberryPiCore.ADC;
using RaspberryPiCore;

namespace LogicLayer
{ 
    public class Ekg_Record 
    {
        IData localDataRef;
        private ADC1015 adc;

        public Ekg_Record()
        {
            localDataRef = new LocalDataFile();
            adc = new ADC1015();
        }
 
        double sample = 0; //En sample er ét punkt
        int antalSamples = 1200; //2500 samplesRettest tilbage til 12000 //Hvor mange samples skal der være i løbet af målingen
        string starttidspunkt;
        private double[] ekgRawData;
        int samplerate = 2; //Variabel til at regulere hvor længe der går mellem hver måling

        public bool StartEkgRecord() //Start modtagelse af signal fra elektroderne
        {
            bool ekgRecordEnd = false;
            starttidspunkt = DateTime.Now.ToString("dd MMMM yyyy HH: mm:ss");
            ekgRawData = new double[antalSamples];

            

            for (int i = 0; i < antalSamples; i++)
            {
                sample = (adc.readADC_SingleEnded(0) / 2048.0) * 6.144; //Konverterer fra adc til strøm (eller omvendt)
                ekgRawData[i] = sample;

                Thread.Sleep(samplerate);
            }
            ekgRecordEnd = true;
            return ekgRecordEnd;
        }
        
        public void CreateEKGDTO(string EmployeeIdAsString, string SocSecNumberAsString) //Modtager disse to fra presentationlayer
        {
            StartEkgRecord(); //Starter målingen

            DTO_EKGMåling nyMåling = new DTO_EKGMåling(EmployeeIdAsString,SocSecNumberAsString,Convert.ToDateTime(starttidspunkt),ekgRawData,antalSamples,samplerate);

           localDataRef.InsertEKGMeasurement(nyMåling); //Sender målingen til databasen
        }

        public string GetReceipt() //Får en værdi på antallet af målinger
        {
             int id = localDataRef.CountID();

             return Convert.ToString(id);
        }
         
        public bool CheckDBForEmployeeId(string EmployeeId)
        {  
            bool result;
            result = localDataRef.CheckDBForEmployeeId(EmployeeId);
            return result;
        }

       
    }
}
