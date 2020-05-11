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
        private ekgRecord ekgRecordRef;


        public Communication()
        {
            lcd = new SerLCD();
            ekgRecordRef = new ekgRecord();
            twist = new TWIST();
            displayRef = new Display();
        }
        public void Program()
        {
            
            lcd.lcdDisplay(); //Tænder skærmen
            lcd.lcdClear();

           
                displayRef.getEmployeeId(); //Medarbejderen logger ind
                Thread.Sleep(500);


            //while løkke - medarbejderen skal ikke logge ind igen men det er muligt at ind
            while (Console.KeyAvailable == false)//ændr i koden ved den rigtige test
            { }
                displayRef.getSocSecNumber(); //Skriver nummerlinjen + cpr
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
            ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO
            
            while (Console.KeyAvailable == false)
            { }
            
            lcd.lcdClear();
            lcd.lcdGotoXY(0, 2);
            lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.getReceipt()}");
            
            lcd.lcdClear();
            lcd.lcdPrint("slut");
            
        }

        
    }
}
