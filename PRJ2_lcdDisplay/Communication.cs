using System;
using System.Threading;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;
using System.Collections.Immutable;
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
            twist.setCount(0);
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


            displayRef.CheckDBForEmployeeId(displayRef.EmployeeIdAsString);
            {
                while (displayRef.CheckDBForEmployeeId(displayRef.EmployeeIdAsString) == false)
                {
                    int index = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        
                        lcd.lcdClear();
                        lcd.lcdGotoXY(2, 1);
                        lcd.lcdPrint("ID ikke godkendt");
                        Thread.Sleep(1000);
                        displayRef.GetEmployeeId();

                        index++;
                        if (displayRef.CheckDBForEmployeeId(displayRef.EmployeeIdAsString) == true)
                        {
                            break;
                        }
                        if (index == 3)
                        {
                            for (int n = 0; n < 1; n++)
                            {
                                lcd.lcdClear();
                                lcd.lcdPrint("Du har brugt dine 3  forsøg. Programmet  lukkes");
                                Thread.Sleep(1000);
                            }
                            Environment.Exit(0);
                        }
                    }
                }
                lcd.lcdClear();
                lcd.lcdGotoXY(4,1);
                lcd.lcdPrint("ID godkendt");
                Thread.Sleep(1000);
            }


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
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Start Ekg maaling?");

            answer = displayRef.Yes_No();
            do
            {
                if (answer == true)
                {
                    lcd.lcdClear();
                    lcd.lcdPrint("Maaling paabegyndt");
                    ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO

                    while (ekgRecordRef.StartEkgRecord() == false) // Venter her indtil metoden returnerer true = måling færdig
                    { }
                    lcd.lcdClear();
                    lcd.lcdPrint("Maaling afsluttet");
                    Thread.Sleep(3000);


                    lcd.lcdClear();
                    lcd.lcdGotoXY(0, 2);
                    lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.GetReceipt()}"); //Kan ikke lade sig gøre, da vi kun kan gennemgå vores database gennem filer som bindeled.
                    Thread.Sleep(2000);

                    lcd.lcdClear();
                    lcd.lcdPrint("Ny maaling?");

                    answer = displayRef.Yes_No();
                    continueEKGMeasurement = answer; 
                }
                break;
            }
            while (continueEKGMeasurement);

            lcd.lcdClear();
            lcd.lcdPrint($"Batteristatus: {batteryRef.ShowBatteryStatus()} %");
            Thread.Sleep(2000);
            if (batteryRef.ShowBatteryStatus() < 20)
            {
                lcd.lcdGotoXY(0, 1);
                lcd.lcdPrint("Lavt batteri        Tilslut oplader     ");
                Thread.Sleep(2000);
            }
            lcd.lcdGotoXY(0, 3);
            lcd.lcdPrint("Program afsluttes");
            Thread.Sleep(2000);
            lcd.lcdClear();
            lcd.lcdNoDisplay();
        }
    }
}