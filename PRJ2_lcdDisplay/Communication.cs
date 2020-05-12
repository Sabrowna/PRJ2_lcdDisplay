using System;
using System.Threading;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;
using DTO;

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
            
            lcd.lcdDisplay(); //Tænder skærmen
            lcd.lcdClear(); // Nulstiller skærm
            lcd.lcdSetBackLight(0,0,0);
            batteryStatusRef.ChangeBackground(); // Ændrer baggrundsfarven efter batteristatus

            lcd.lcdGotoXY(0, 1);
            lcd.lcdPrint($" Batteristatus: {batteryRef.ShowBatteryStatus()} %"); // Udskriver batteristatus på display

            Thread.Sleep(3000); // Venter i 3 sek. så det er muligt at se status på batteri både på display LED

           

            lcd.lcdClear();
            
           
                displayRef.GetEmployeeId(); //Medarbejderen logger ind
                Thread.Sleep(500);


            //while løkke - medarbejderen skal ikke logge ind igen men det er muligt at ind
            while (Console.KeyAvailable == false)//ændr i koden ved den rigtige test
            { }
                displayRef.GetSocSecNumber(); //Skriver nummerlinjen + cpr
                Thread.Sleep(500);
            
            while (Console.KeyAvailable == false)
            { }

            lcd.lcdClear();
            lcd.lcdGotoXY(1, 0);
            lcd.lcdPrint("Start Ekg maaling");

            //public bool YesNo()
            //{
            lcd.lcdGotoXY(7, 1);
            lcd.lcdPrint("Ja/Nej");
            lcd.lcdGotoXY(7, 1);
            twist.setCount(0);
            /*
            if (twist.getCount() < 2)
            {
                twist.getBlueConnect();//Mere blå jo mere man drejer på knappen?
            }
            // }
            */
            //ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO
            ekgRecordRef.StartEkgRecord();
            if (ekgRecordRef.StartEkgRecord() == true)
            {
                lcd.lcdPrint("måilng afsluttet");
            }
            while (Console.KeyAvailable == false)
            { }
            
            lcd.lcdClear();
            lcd.lcdGotoXY(0, 2);
            lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.GetReceipt()}");
            
            lcd.lcdClear();
            lcd.lcdNoDisplay();
        }

        
    }
}
