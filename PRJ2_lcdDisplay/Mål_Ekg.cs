using System;
using System.Threading;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;
using DTO;

namespace PresentationLayer 

{
    //Korrigerer mellem de forskellige klasser. Programmet starter
    public class Mål_Ekg //TEST
    {
        public ADC1015 adc;
        public SerLCD lcd;
        public TWIST twist;
        public Display displayRef;
        public ekgRecord ekgRecordRef;

        public Mål_Ekg(Display displayRef)
        {
            
        }
        //public Mål_Ekg målEkg; 

        static void Main(string[] args)
        {
            målEkg = new Mål_Ekg(displayRef,ekgRecordRef);
            Mål_Ekg målEkg = new Mål_Ekg(displayRef.getSocSecNumber());
        }

    }
}

