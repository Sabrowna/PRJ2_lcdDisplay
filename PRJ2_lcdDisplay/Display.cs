using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Threading;
using LogicLayer;

namespace PresentationLayer
{
    /// <summary>
    /// Varetager processerne omkring indtastning af medarbejderID og CPR-nummer. 
    /// </summary>
    public class Display
    {
        public SerLCD lcd;
        public TWIST twist;
        private Ekg_Record ekgRecordRef;

        public string EmployeeIdAsString { get; set; } //Konvertering af medarbejderlisten til en string
        public string SocSecNumberAsString { get; set; } //Convertering af CPR listen til en string
        private List<short> cprNumbersL = new List<short>(); //Tilføjer de indskrevne CPR-numre én efter én //Short fordi drejeknappen er en short
        private List<short> employeeIdList = new List<short>(); //Liste til medarbejder id //Short fordi drejeknappen er en short

        /// <summary>
        /// Constructor til klassen. Instantierer referencerne til LCD og Encoder, samt referencen til EKG_Record.cs.
        /// </summary>
        public Display()
        {
            lcd = new SerLCD();
            twist = new TWIST();

            ekgRecordRef = new Ekg_Record();
            lcd.lcdCursor();
            lcd.lcdBlink();

        }
        /// <summary>
        /// Udskriver cifrene 0-9 på én linje. Anvendes til at vælge cifre fra ved indtastning af både medarbejderID og CPR-nummer.
        /// </summary>
        public void WritenumberLine()
        {
            byte number = 0; // Lokal variabel til brug for udskrivning af NumberLine
            byte x = 0; //Værdien på vores x-akse
            lcd.lcdGotoXY(x, 1);
            for (int i = 0; i < 10; i++)
            {
                lcd.lcdPrint(Convert.ToString(number));
                x++;
                lcd.lcdGotoXY(x, 1);
                number++;
            }
        }

        /// <summary>
        /// Metode til indtastning af CPR-nummer. Gemmer indtastede værdier som værdier i en liste, 
        /// og konverterer afslutningsvis listen til én samlet string. 
        /// </summary>
        public void GetSocSecNumber()
        {
            byte xValueCPRLine = 0; //variabel
            byte xStartValueNumberLine = Convert.ToByte(0); //konstant
            byte countingIsPressed = 0;

            lcd.lcdClear();
            twist.setCount(0);
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Indtast CPR nummer");
            WritenumberLine(); // Kører denne metode for at få vist NumberLine

            lcd.lcdGotoXY(xStartValueNumberLine, 1); //starter samme sted som numberline

            while (countingIsPressed < 11)
            {
                while (twist.isPressed() == false)
                {
                    if (twist.getCount() < 0) //Limit-metode - hvis cursoren går forbi 0-tallet, hopper den hen på 9
                    {
                        lcd.lcdGotoXY(9, 1);
                        twist.setCount(9);
                    }

                    else if (twist.getCount() > 9) //Limit-metode - hvis cursoren går forbi 9-tallet, hopper den hen på 0
                    {
                        lcd.lcdGotoXY(0, 1);
                        twist.setCount(0);
                    }

                    else if (twist.getCount() >= 0 || twist.getCount() <= 9)
                    {
                        byte getCount = Convert.ToByte(twist.getCount() + xStartValueNumberLine);
                        lcd.lcdGotoXY(getCount, 1);
                    }
                }
                Thread.Sleep(500);
                cprNumbersL.Add(twist.getCount()); //Tilføj til en liste som vi senere kan videresende
                lcd.lcdGotoXY(xValueCPRLine, 2); //Bruger ser cpr nummer på denne linje
                lcd.lcdPrint(twist.getCount().ToString()); //udskriver på pladsen til cpr nummer
                xValueCPRLine++;
                twist.setCount(twist.getCount()); // Her bliver cursoren stående på positionen på numberline

                //Sætter cursoren tilbage der hvor den sluttede
                byte getCountEnd = Convert.ToByte(twist.getCount() + xStartValueNumberLine);
                lcd.lcdGotoXY(getCountEnd, 1);
                countingIsPressed++;

                if (countingIsPressed == 6) //Der skal være en bindestreg efter tal nr 6
                {
                    lcd.lcdGotoXY((xValueCPRLine++), 2);
                    lcd.lcdPrint("-");
                    countingIsPressed++;
                } 
            }
            SocSecNumberAsString = string.Join("", cprNumbersL.ToArray());
        }


        /// <summary>
        /// Metode til indtastning af medarbejderID. Gemmer indtastede værdier som værdier i en liste, 
        /// og konverterer afslutningsvis listen til én samlet string. 
        /// </summary>
        public void GetEmployeeId()
        {
            byte xValueIDLine = 0; //variabel
            byte xStartValueNumberLine = Convert.ToByte(0); //konstant
            byte countingIsPressed = 0;

            lcd.lcdClear();
            twist.setCount(0);
            lcd.lcdGotoXY(0, 0);
            employeeIdList.Clear();
            //EmployeeIdAsString = string.Join("", employeeIdList.ToArray());
            lcd.lcdPrint("Indtast ID nummer");
            WritenumberLine(); // Kør denne metode for at få vist NumberLine

            lcd.lcdGotoXY(xStartValueNumberLine, 1);
            while (countingIsPressed < 4)
            {
                while (twist.isPressed() == false)
                {
                    if (twist.getCount() < 0) //Limit-metode - hvis cursoren går forbi 0-tallet, hopper den hen på 9
                    {
                        lcd.lcdGotoXY(9, 1);
                        twist.setCount(9);
                    }

                    else if (twist.getCount() > 9) //Limit-metode - hvis cursoren går forbi 9-tallet, hopper den hen på 0
                    {
                        lcd.lcdGotoXY(0, 1);
                        twist.setCount(0);
                    }

                    else if (twist.getCount() >= 0 || twist.getCount() <= 9)
                    {
                        byte getCount = Convert.ToByte(twist.getCount() + xStartValueNumberLine);
                        lcd.lcdGotoXY(getCount, 1);
                    }
                }
                    Thread.Sleep(500);
                    employeeIdList.Add(twist.getCount()); //Tilføj til en liste som vi senere kan videresende
                    lcd.lcdGotoXY(xValueIDLine, 2); // Går til linjen ID Nummer udskrives på
                    lcd.lcdPrint(twist.getCount().ToString()); //Udskriver valgte værdi fra numberline som værdi i MedarbejderID
                    xValueIDLine++; // Lægger 1 til værdien på den lokale variabel, så næste ciffer skrives på feltet til højre for
                    twist.setCount(twist.getCount()); // Sætter værdien på twisterens count til nuværende


                    byte getCountEnd = Convert.ToByte(twist.getCount() + xStartValueNumberLine);
                    lcd.lcdGotoXY(getCountEnd, 1); //Sætter cursoren tilbage på numberline hvor den stod før
                    countingIsPressed++; // Lægger 1 til countet på antal cifte i indtastet MedarbejderID

                EmployeeIdAsString = string.Join("",employeeIdList.ToArray());
            }
            for (int i = 0; i < 2; i++)
            {
                lcd.lcdGotoXY(0, 3);
                lcd.lcdPrint("Tjekker ID");
                Thread.Sleep(300);
                lcd.lcdGotoXY(10, 3);
                lcd.lcdPrint(".");
                Thread.Sleep(300);
                lcd.lcdGotoXY(11, 3);
                lcd.lcdPrint(".");
                Thread.Sleep(300);
                lcd.lcdGotoXY(12, 3);
                lcd.lcdPrint(".");
                Thread.Sleep(800);
                lcd.lcdGotoXY(0, 3);
                lcd.lcdPrint("             ");
                Thread.Sleep(500);
            }
        }              

        /// <summary>
        /// Muligheden for at give brugeren et valg mellem henholdsvis ja eller nej. 
        /// Eksempelvis som svar til om der ønskes at fortsætte måling med CPR-nummer.
        /// </summary>
        /// <returns></returns>
        public bool Yes_No()
        {
            bool værdi = false;
            twist.setCount(0);

            lcd.lcdGotoXY(0, 1);
            lcd.lcdPrint("1. Ja");

            lcd.lcdGotoXY(0, 2);
            lcd.lcdPrint("2. Nej");

            while (twist.isPressed() == false)
            {

                if ((twist.getCount()) % 2 == 0) //Går cursoren op i et lige tal, skriver man 'ja'
                {
                    værdi = true; //Man har skrevet ja
                    lcd.lcdGotoXY(0, 1);
                }

                else if ((twist.getCount()) % 2 == 1) //Går cursoren op i et ulige tal, skriver man 'nej'
                {
                    værdi = false;
                    lcd.lcdGotoXY(0, 2);
                }
            }
            return værdi;
        }

        /// <summary>
        /// Kontrollerer om det indtastede medarbejderID findes i systemet - den lokale database. 
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public bool CheckDBForEmployeeId(string EmployeeId)
        {
            
            bool result;
            result = ekgRecordRef.CheckDBForEmployeeId(EmployeeId);

            return result;
        }

    }
}


