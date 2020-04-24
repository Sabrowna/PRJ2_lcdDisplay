﻿using System;
using System.Threading;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;


namespace Raspberry_Pi_Dot_Net_Core_Console_Application3
{
    class Program
    {
        public ADC1015 adc;
        public SerLCD lcd;
        public TWIST twist;
        

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ADC1015 adc = new ADC1015();
            SerLCD lcd = new SerLCD();
            TWIST twist = new TWIST();

            lcd.lcdDisplay();
            lcd.lcdClear();

            
            /*UIntPtr[] arrow = { 0*00, 0x04, 0x06, 0x1f, 0x06, 0x04, 0x00 }; //Send 0,4,6,1F,6,4,0 for the arrow


            lcd.lcdGotoXY(0, 5);
            lcd.lcdPrint(arrow.ToString());
            Thread.Sleep(5000);
            */
        }

    }
}

