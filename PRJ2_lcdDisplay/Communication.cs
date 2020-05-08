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
            Batterystatus = 20; // Få en ægte værdi ind her. 
        }
        public void Program()
        {
            
            lcd.lcdDisplay(); //Tænder skærmen
            lcd.lcdClear(); // Nulstiller skærm
            batteryRef.ShowBatteryStatus(Batterystatus); // Tjekker status på batteri

            lcd.lcdGotoXY(0, 1);
            lcd.lcdPrint($" Batteristatus: {batteryRef.ShowBatteryStatus(Batterystatus)}"); // Udskriver batteristatus på display

            Thread.Sleep(3000); // Venter i 3 sek. så det er muligt at se status på batteri både på display LED

            if (batteryRef.ShowBatteryStatus(Batterystatus) < 20) // Hvis batteristatus er lav jf. UC, udskrives nedenstående
            {
                while (batteryRef.ShowBatteryStatus(Batterystatus) < 20) // Så længe batteristatus er lav, bliver systemet i denn løkke
                {
                    lcd.lcdGotoXY(0, 0);
                    lcd.lcdPrint("Enhed deaktiveret   Batteristatus lav   Tilslut oplader");
                }

            }

            if (batteryRef.Charging(isCharging) == true) // Hvis opladning er i gang, køres og udskrives som nedenfor
            {
                while (batteryRef.Charging(isCharging) == true) // Så længe opladning er i gang, bliver systemet i denne løkke
                {
                    lcd.lcd.lcdGotoXY(0, 1);
                    lcd.lcdPrint("Opladning i gang    Måling ikke mulig");
                }
            }

            lcd.lcdClear();


            displayRef.GetEmployeeId(); //Medarbejderen logger ind

            //while løkke - medarbejderen skal ikke logge ind igen
            displayRef.GetSocSecNumber(); //Skriver nummerlinjen + cpr

            lcd.lcdClear();
            lcd.lcdGotoXY(6, 0);
            lcd.lcdPrint("Start Ekg måling");

            //public bool YesNo()
            //{
            lcd.lcdGotoXY(7, 1);
            lcd.lcdPrint("Ja/Nej");
            lcd.lcdGotoXY(7, 1);
            twist.setCount(0);
            if (twist.getCount() < 2)
            {
                twist.getBlueConnect();//Mere blå jo mere man drejer på knappen?
            }
            // }
            ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO
            
            lcd.lcdClear();
            lcd.lcdGotoXY(0, 2);
            lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.GetReceipt()}");
        }
    }
}
