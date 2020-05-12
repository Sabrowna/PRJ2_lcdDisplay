﻿using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Threading;
using LogicLayer;
using DTO;

namespace PresentationLayer
{
    public class Display
    {
        public SerLCD lcd;
        public TWIST twist;
        private Ekg_Record ekgRecordRef;

        string number;
        bool result;
        public string EmployeeIdAsString { get; set; } //Konvertering af medarbejderlisten til en string
        public string SocSecNumberAsString { get; set; } //Convertering af CPR listen til en string
        private List<short> cprNumbersL = new List<short>(); //Tilføjer de indskrevne CPR-numre én efter én //Short fordi drejeknappen er en short
        private List<short> employeeIdList = new List<short>(); //Liste til medarbejder id //Short fordi drejeknappen er en short


        public Display()
        {
            lcd = new SerLCD();
            twist = new TWIST();

            ekgRecordRef = new Ekg_Record();
            //twist.setCount(0)
        }

        public void WritenumberLine()
        {
            byte number = 0;
            byte x = 6; //Værdien på vores x-akse
            lcd.lcdGotoXY(x, 1);
            for (int i = 0; i < 10; i++)
            {
                lcd.lcdPrint(Convert.ToString(number));
                x++;
                lcd.lcdGotoXY(x, 1);
                number++;
            }
        }



        // public string GetSocSecNumber()
        public void GetSocSecNumber()
        {
            byte countingIsPressed;
            byte xValueCPRLine = 6;
            byte xStartValueNumberLine = 6;


            lcd.lcdClear();
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Indtast CPR nummer");
            WritenumberLine(); // Kører denne metode for at få vist NumberLine

            lcd.lcdGotoXY(xStartValueNumberLine, 1); //starter samme sted som numberline

            /*
            lcd.lcdGotoXY(x, 1); //starter samme sted som numberline
            
            for (countingIsPressed = 0; countingIsPressed < 10; countingIsPressed++)
            {
                if (countingIsPressed == 5)
                {
                    lcd.lcdGotoXY((xValueCPRLine + 1), 2);
                    lcd.lcdPrint("-");
                    xValueCPRLine++;

                }

                else
                {
                    while (twist.isPressed() == false)
                    {

                        if (twist.getCount() < 0 && twist.getCount() > 9)
                        {

                            if (twist.getCount() < 0)
                            {
                                lcd.lcdGotoXY(15, 1);
                                twist.setCount() = 9;
                            }

                            else if (twist.getCount() > 9)
                            {
                                lcd.lcdGotoXY(6, 1);
                                twist.setCount() = 0;
                            }
                        }
                        else if (twist.getCount() >= 0 && twist.getCount() <= 9)
                        {
                            lcd.lcdGotoXY((twist.getCount()) + xStartValueNumberLine, 1);
                        }

                        else if (twist.isPressed() == true) // irrelevant statement? Hvis linje 66 er true, kører den her ikke uanset hvad
                        {
                            cprNumbersL.Add(twist.getCount());
                            lcd.lcdGotoXY(xValueCPRLine, 2); //Bruger ser cpr nummer på denne linje
                            lcd.lcdPrint(twist.getCount().ToString()); //udskriver på pladsen til cpr nummer
                            xValueCPRLine++;
                            twist.setCount(twist.getCount()); // Her bliver cursoren stående på positionen på numberline

                        }

                        lcd.lcdGotoXY((twist.getCount()) + xStartValueNumberLine, 1);
                    }
                }                
            }
            SocSecNumberAsString = CprNumbersL.ToString();
            */
            List<int> tempSocID = new List<int>();
            int number = 0;
            lcd.lcdGotoXY(xValueCPRLine, 2);
            /* Skal slettes igen
            for (int i = 0; i < 10; i++)
            {
                number = i;
                tempSocID.Add(number);
                //lcd.lcdPrint(number.ToString());
            }
            SocSecNumberAsString = tempSocID.ToString();
            */
            SocSecNumberAsString = "0123456789";
            lcd.lcdPrint(SocSecNumberAsString);
            //return SocSecNumberAsString;

        }

        // public string GetEmployeeId()
        public void GetEmployeeId()
        {
            byte countingIsPressed;
            byte xValueCPRLine = 8;
            lcd.lcdClear();
            lcd.lcdGotoXY(1, 0);
            lcd.lcdPrint("Indtast ID nummer");
            WritenumberLine(); // Kør denne metode for at få vist NumberLine

            /*
            lcd.lcdGotoXY(x, 1); //starter samme sted som numberline

            
            for (countingIsPressed = 0; countingIsPressed < 4; countingIsPressed++)
            {
                while (twist.isPressed() == false)
                {
                    // Kode der gør at metoden venter på twist.isPressed
                }

                if (twist.isPressed() == true)
                {
                    if (twist.getCount() < 0 && twist.getCount() > 9)
                    {

                        lcd.lcdGotoXY(0, 3);
                        lcd.lcdPrint("FEJL");
                        Thread.Sleep(3000);
                        lcd.lcdPrint("                    "); //Kan man slette en linje uden at slette det andet?
                        twist.setCount(0);
                        lcd.lcdGotoXY(6, 1);
                        
                    }

                 

                    employeeIdList.Add(twist.getCount());
                    lcd.lcdGotoXY(x, 2); //Bruger ser cpr nummer på denne linje
                    lcd.lcdPrint(twist.getCount().ToString()); //udskriver på pladsen til cpr nummer
                                                               //x += countingIsPressed;
                    twist.setCount(0);
                    lcd.lcdGotoXY(x, 1);
                    // break;
                }
            }
            

            EmployeeIdAsString = EmployeeIdList.ToString();
            */
            List<int> tempEmpID = new List<int>();
            int number = 0;
            /* Dette skal slettes igen
            for (int i = 0; i <4; i++)
            {
                number = i;
                tempEmpID.Add(number);
                //lcd.lcdPrint(number.ToString());
                
            } */
            EmployeeIdAsString = "1234";//tempEmpID.ToString();
            lcd.lcdGotoXY(xValueCPRLine, 2);
            lcd.lcdPrint(EmployeeIdAsString);
            //return EmployeeIdAsString;
        }

            //EmployeeIdAsString = employeeIdList.ToString();
            
        //}

        // Indsæt metode fra CPR checker der kontrollerer om det er validt CPR-nummer? 
    }
}
 
