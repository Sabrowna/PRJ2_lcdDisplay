using System;
using System.Threading;
using RaspberryPiCore.ADC;
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

            //while løkke - medarbejderen skal ikke logge ind igen
            displayRef.getSocSecNumber(); //Skriver nummerlinjen + cpr

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
            ekgRecordRef.CreateEKGDTO(); //Starter målingen); //Opretter en DTO
            ekgRecordRef.SendToDB();
            displayRef.getReceipt();
        }
    }
}
