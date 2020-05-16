using System;
using System.Threading;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;

namespace PresentationLayer // Nyeste version - Sabrina + Dannys rettelser + filer

{
    //Korrigerer mellem de forskellige klasser. Programmet starter
    public class Mål_Ekg 
    {
        public static Communication commRef;

        static void Main(string[] args)
        {
            commRef = new Communication();

            commRef.Program();
        }

    }
}

