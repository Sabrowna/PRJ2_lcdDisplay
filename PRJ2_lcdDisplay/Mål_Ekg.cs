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
    /// <summary>
    /// Programmets hovedklasse, hvorfra selve programmet kaldes.
    /// </summary>
    public class Mål_Ekg 
    {
        /// <summary>
        /// Reference til Communication.cs
        /// </summary>
        public static Communication commRef;

        /// <summary>
        /// Mainmetode der med reference til Communication.cs kalder metoden Program. Initialiserer referencen til Communication.cs.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            commRef = new Communication();
            
            commRef.Program();
        }

    }
}

