using System;
using System.Threading;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;
using DTO;
using System.Reflection.Metadata.Ecma335;

namespace PresentationLayer
{

    public class Communication
    {
        private SerLCD lcd;
        private TWIST twist;
        private Display displayRef;
        private Ekg_Record ekgRecordRef;
        private Battery batteryRef;
        private BatteryStatus batteryStatusRef;
        public bool isCharging = false; // Værdi skal hentes fra det fysiske system. Knap, kontakt etc.
        // der bør også i forhold til isCharging laves en binding der tjekker om oplader pludselig bliver tilsluttet.
        private double Batterystatus { get; set; }


        public Communication()
        {
            lcd = new SerLCD();
            twist = new TWIST();
            ekgRecordRef = new Ekg_Record();
            displayRef = new Display();
            batteryRef = new Battery();
            batteryStatusRef = new BatteryStatus();
            Batterystatus = 20; // Få en ægte værdi ind her. 
        }
        public void Program()
        {
            bool continueEKGMeasurement = false;
            bool answer;
            lcd.lcdDisplay(); //Tænder skærmen
            lcd.lcdClear(); // Nulstiller skærm
            lcd.lcdSetBackLight(0, 0, 0);
            batteryStatusRef.ChangeBackground(); // Ændrer baggrundsfarven efter batteristatus
            lcd.lcdClear();



            displayRef.GetEmployeeId(); //Medarbejderen logger ind
            Thread.Sleep(500);

            /*
            //while ()
            //{

            batteryStatusRef.ChargeBattery();
            batteryRef.ShowBatteryStatus();

            
            //while løkke - medarbejderen skal ikke logge ind igen men det er muligt at ind
            while (Console.KeyAvailable == false)//ændr i koden ved den rigtige test
            { }
            lcd.lcdClear();
            lcd.lcdPrint("Maaling med CPR?");

            if (displayRef.Yes_No() == true)
            {
                displayRef.GetSocSecNumber(); //Skriver nummerlinjen + cpr
            }
            else
            */
                displayRef.SocSecNumberAsString = "1111111111";
            /*
            
            lcd.lcdClear();
            lcd.lcdGotoXY(1, 0);
            lcd.lcdPrint("Start Ekg maaling");

            answer = displayRef.Yes_No();

            do 
            {
                if (answer == true)
                {
                */
                    ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO
                    ekgRecordRef.StartEkgRecord();
                    if (ekgRecordRef.StartEkgRecord() == true)
                    {
                        lcd.lcdPrint("Maaling afsluttet");
                Thread.Sleep(3000);

                    }
                    //while (Console.KeyAvailable == false)
                    //{ }

                    lcd.lcdClear();
                    lcd.lcdGotoXY(0, 2);
                   // lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.GetReceipt()}");
                //}

                lcd.lcdClear();
                lcd.lcdPrint("Ny maaling?");

                //displayRef.Yes_No??
                answer = displayRef.Yes_No();
                continueEKGMeasurement = answer;
                
                

            //} while (continueEKGMeasurement) ;

        }
    }
}
