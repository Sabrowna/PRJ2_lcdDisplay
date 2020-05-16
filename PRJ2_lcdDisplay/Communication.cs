using System;
using System.Threading;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;
//using System.Reflection.Metadata.Ecma335;

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
        // private double Batterystatus { get; set; }


        public Communication()
        {
            lcd = new SerLCD();
            twist = new TWIST();
            ekgRecordRef = new Ekg_Record();
            displayRef = new Display();
            batteryRef = new Battery();
            batteryStatusRef = new BatteryStatus();

        }
        public void Program()
        {
            bool continueEKGMeasurement = false;
            bool answer;
            lcd.lcdNoDisplay();
            lcd.lcdDisplay(); //Tænder skærmen
            lcd.lcdClear(); // Nulstiller skærm
            lcd.lcdCursor(); //Tænder for cursoren
            lcd.lcdBlink();
            lcd.lcdSetBackLight(0, 0, 0);

            

            batteryStatusRef.ChargeBattery(); // Hvis oplader er tilstluttet køres denne metode - exit program. 
            batteryStatusRef.ShowBatteryStatus(); // Ændrer baggrundsfarven efter batteristatus

            displayRef.GetEmployeeId(); //Medarbejderen logger ind
           /* displayRef.CheckDBForEmployeeId(displayRef.EmployeeIdAsString);
            {

                {
                    while (displayRef.CheckDBForEmployeeId(displayRef.EmployeeIdAsString) == false)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            lcd.lcdClear();
                            lcd.lcdPrint("ID ikke godkendt");
                            Thread.Sleep(1000);
                            displayRef.GetEmployeeId();

                        }
                        lcd.lcdClear();
                        lcd.lcdPrint("Du har brugt dine 3  forsøg. Programmet  lukkes");
                        Thread.Sleep(1000);
                        break;
                        //Environment.Exit(0);
                    }
                    lcd.lcdClear();
                    lcd.lcdPrint("ID godkendt");
                    Thread.Sleep(1000);
                }
            }
            */
            lcd.lcdClear();
            lcd.lcdPrint("Maaling med CPR?");

            if (displayRef.Yes_No() == true)
            {
                displayRef.GetSocSecNumber(); //Skriver nummerlinjen + cpr
            }
            else
            {
                displayRef.SocSecNumberAsString = "1111111111";
            }

            lcd.lcdClear();
            lcd.lcdGotoXY(1, 0);
            lcd.lcdPrint("Start Ekg maaling");

            answer = displayRef.Yes_No();

            do
            {
                if (answer == true)
                {
                    ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO
                    //ekgRecordRef.SendToDB();
                    if (ekgRecordRef.StartEkgRecord() == true)
                    {
                        lcd.lcdClear();
                        lcd.lcdPrint("Maaling afsluttet");
                        Thread.Sleep(3000);

                    }


                    lcd.lcdClear();
                    lcd.lcdGotoXY(0, 2);
                    // lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.GetReceipt()}");
                    //}

                    lcd.lcdClear();
                    lcd.lcdPrint("Ny maaling?");

                    //displayRef.Yes_No??
                    answer = displayRef.Yes_No();
                    continueEKGMeasurement = answer;
                }


            }
            while (continueEKGMeasurement);
            lcd.lcdClear();
            lcd.lcdPrint("Program afsluttes");
            Thread.Sleep(2000);
            lcd.lcdClear();
            lcd.lcdNoDisplay();
            //EVT VIS BATTERISTATUS HER
        }
    }
}