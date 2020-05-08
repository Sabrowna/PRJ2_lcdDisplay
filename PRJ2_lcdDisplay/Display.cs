using System;
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

        private string number;
        private bool result;
        public string EmployeeIdAsString { get; set; } //Konvertering af medarbejderlisten til en string
        public string SocSecNumberAsString { get; set; } //Convertering af CPR listen til en string
        private List<short> cprNumbersL = new List<short>(); //Tilføjer de indskrevne CPR-numre én efter én
        private List<short> employeeIdList = new List<short>(); //Liste til medarbejder id
       

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
                x += x++;
                lcd.lcdGotoXY(x, 1);
                number = number++;
            }
        }

     

        // public string GetSocSecNumber()
        public void GetSocSecNumber()
        {
            byte countingIsPressed;
            byte x = 6;

            lcd.lcdClear();
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Indtast CPR nummer");
            WritenumberLine(); // Kør denne metode for at få vist NumberLine??? 



            lcd.lcdGotoXY(x, 1); //starter samme sted som numberline

            for (countingIsPressed = 0; countingIsPressed < 10; countingIsPressed++)
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
                        //Virker det?
                    }
                    if (countingIsPressed == 6)
                    {
                        lcd.lcdGotoXY(12, 1);
                        lcd.lcdPrint("-");
                    }
                    cprNumbersL.Add(twist.getCount());
                    lcd.lcdGotoXY(x, 2); //Bruger ser cpr nummer på denne linje
                    lcd.lcdPrint(twist.getCount().ToString()); //udskriver på pladsen til cpr nummer
                                                               //x += countingIsPressed;
                    twist.setCount(0);
                    lcd.lcdGotoXY(x, 1);
                    // break;
                }
            }
            SocSecNumberAsString = cprNumbersL.ToString();
            // return SocSecNumberAsString;

        }

        // public string GetEmployeeId()
        public void GetEmployeeId()
        {
            byte countingIsPressed;
            byte x = 6;
            lcd.lcdClear();
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Indtast ID nummer");
            WritenumberLine(); // Kør denne metode for at få vist NumberLine??? 



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
                        //Virker det?
                    }
                    if (countingIsPressed == 6)
                    {
                        lcd.lcdGotoXY(12, 1);
                        lcd.lcdPrint("-");
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

            EmployeeIdAsString = employeeIdList.ToString();
            // return EmployeeIdAsString;
        }

        //private string socSecNb;
        //public bool verifySocSecNb(string socSecNb)
        //{
        //    int[] integer = new int[10];

        //    for (int index = 0; index < 10; index++)
        //    {
        //        integer[index] = Convert.ToInt16(number[index]) - 48; //Karakteren på plads index konverteres til den tilhørende integer - eksempel '6' konverteres til 6
        //    }

        //    // Algoritme der kotrollerer om cifrene danner et gyldigt personnummer
        //    if ((4 * integer[0] + 3 * integer[1] + 2 * integer[2] + 7 * integer[3] + 6 * integer[4] + 5 * integer[5] + 4 * integer[6] + 3 * integer[7] + 2 * integer[8] + integer[9]) % 11 != 0)
        //        return false;
        //    else
        //        return true;
        //}


    }
}

