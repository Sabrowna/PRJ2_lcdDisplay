﻿using System;
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
        public static Communication commRef;

        static void Main(string[] args)
        {
            commRef = new Communication();

            commRef.Program();
        }

    }
}

