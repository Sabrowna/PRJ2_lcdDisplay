using System;
using System.Threading;
using System.Collections.Generic;
using DataLayer2;
using DTO2;
using RaspberryPiCore.ADC;
using RaspberryPiCore;

namespace LogicLayer
{ 
    /// <summary>
    /// Varetager målingen af de elektriske imnpulser fra klamperne. 
    /// Konverterer via ADC elektrisk signal. Opretter DTO med de påkrævede parametre. 
    /// </summary>
    public class Ekg_Record 
    {
        /// <summary>
        /// Reference til DataLayer. 
        /// </summary>
        IData localDataRef;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private ADC1015 adc;
        /// <summary>
        /// Constructor til klassen. Initialiserer referencen til DataLayer.
        /// </summary>
        public Ekg_Record()
        {
            localDataRef = new LocalDataFile();
            adc = new ADC1015();
        }
        /// <summary>
        /// Variabel der anvendes ved indlæsning af værdier når måling er aktiv. Defaultværdi = 0.
        /// </summary>
        double sample = 0; //En sample er ét punkt
        /// <summary>
        /// Det samlede antal punkter der måles. 
        /// </summary>
        int antalSamples = 3000; //2500 samplesRettest tilbage til 12000 //Hvor mange samples skal der være i løbet af målingen
        /// <summary>
        /// Lokal variabel der sættes til DateTime.Now i formatet ToString("dd MMMM yyyy HH: mm:ss"), når StartEkgRecord() køres.
        /// </summary>
        string starttidspunkt;
        /// <summary>
        /// Array til at holde de målte værdier.
        /// </summary>
        private double[] ekgRawData;
        /// <summary>
        /// Intervallet mellem hver måling - angivet i millisekunder.
        /// </summary>
        int samplerate = 2; //Variabel til at regulere hvor længe der går mellem hver måling

        /// <summary>
        /// Foretager en måling med intervallet angivet i attributten samplerate. 
        /// Indlæser hver værdi i lokalt array ekgRawData.
        /// </summary>
        /// <returns> Returnerer true når måling er afsluttet. </returns>
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
        /// <summary>
        /// Opretter DTO med parametre tilsvarende kolonner i databasens tabel SP_NyeEkger. 
        /// Sender målingen videre til datalaget, hvorfra målingen endeligt indlæses i databasen. 
        /// </summary>
        /// <param name="EmployeeIdAsString">Det indtastede medarbejderID som string. </param>
        /// <param name="SocSecNumberAsString">Det indtastede CPR-nummer som string. 
        /// Er måling foretaget uden CPR, er default 111111-1111 </param>
        public void CreateEKGDTO(string EmployeeIdAsString, string SocSecNumberAsString) //Modtager disse to fra presentationlayer
        {
            StartEkgRecord(); //Starter målingen

            DTO_EKGMåling nyMåling = new DTO_EKGMåling(EmployeeIdAsString,SocSecNumberAsString,Convert.ToDateTime(starttidspunkt),ekgRawData,antalSamples,samplerate);

           localDataRef.InsertEKGMeasurement(nyMåling); //Sender målingen til databasen
        }
        /// <summary>
        /// Returnerer værdien af antallet af målinger i databasen +1. 
        /// </summary>
        /// <returns> Returværdien er en beregnet fortløbende ascenderende værdi, 
        /// baseret på antallet af målinger i databasen. </returns>
        public string GetReceipt() //Får en værdi på antallet af målinger
        {
             int id = localDataRef.CountID();

             return Convert.ToString(id);
        }
        /// <summary>
        /// Kontrollerer om medarbejderID er registreret i databasen.
        /// </summary>
        /// <param name="EmployeeId">Modtager medarbejderID fra PresentationLayer - Display.cs</param>
        /// <returns> Returnerer true eller false, afhængig af om medarbejderID findes i databasen. </returns>
        public bool VerifyEmployeeId(string EmployeeId)
        {  
            bool result;
            result = localDataRef.VerifyEmployeeId(EmployeeId);
            return result;
        }

       
    }
}
