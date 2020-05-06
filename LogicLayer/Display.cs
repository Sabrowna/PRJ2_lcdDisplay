using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Threading;
using LocalDB;
using DTO;

namespace LogicLayer
{
    //Denne klasse er til at logge ind på display - indtastning + verificering af CPR
    //Her oprettes CPR og medarbejderId
    public class Display
    {
        public ADC1015 adc;
        public SerLCD lcd;
        public TWIST twist;

        private string number;
        private bool result;

        LokalDB dataObjectRef;
        public string EmployeeIdAsString { get; set; }
        public string SocSecNumberAsString { get; set; }

        public Display(ADC1015 adc, SerLCD lcd, TWIST twist)
        {
            this.adc = adc;
            this.lcd = lcd;
            this.twist = twist;

            dataObjectRef = new LokalDB();

            //twist.setCount(0)

        }

        public void WritenumberLine()
        {
            byte number = 0;
            byte x = 6;
            lcd.lcdGotoXY(x, 1);
            for (int i = 0; i < 10; i++)
            {
                lcd.lcdPrint(Convert.ToString(number));
                x += x++;
                lcd.lcdGotoXY(x, 1);
                number = number++;
            }
        }

        public void getReceipt()
        {
            int Id = dataObjectRef.CountRows()+1001;
            lcd.lcdPrint($"Dine data er sendt til den lokale database med IDnummer: {Id}");
        }


        //byte[] CprNumbersA = new byte[10];
        public List<short> CprNumbersL = new List<short>();
        public List<short> EmployeeIdList = new List<short>(); //Liste til medarbejder id
        //byte x = 0;
        //byte y = 0;
        //byte checkNumerOfPress = 0;



        public string getSocSecNumber()
        {
            
            byte countingIsPressed;
            byte x = 6;

            lcd.lcdClear();
            WritenumberLine(); // Kør denne metode for at få vist NumberLine??? 
            
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Indtast CPR nummer");

            lcd.lcdGotoXY(x, 1); //starter samme sted som numberline

            // for (byte i = 0; i < 10; i++)
            //{
            //CprNumbersA[i] = i;

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
                    CprNumbersL.Add(twist.getCount());
                    lcd.lcdGotoXY(x, 2); //Bruger ser cpr nummer på denne linje
                    lcd.lcdPrint(twist.getCount().ToString()); //udskriver på pladsen til cpr nummer
                                                               //x += countingIsPressed;
                    twist.setCount(0);
                    lcd.lcdGotoXY(x, 1);
                    // break;
                }
            }
         

            SocSecNumberAsString =  CprNumbersL.ToString();
            return SocSecNumberAsString;

        }

        public string getEmployeeId()
        {
           
            byte countingIsPressed;
            byte x = 6;
            lcd.lcdClear();
            WritenumberLine(); // Kør denne metode for at få vist NumberLine??? 
            
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Indtast ID nummer");

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
                    EmployeeIdList.Add(twist.getCount());
                    lcd.lcdGotoXY(x, 2); //Bruger ser cpr nummer på denne linje
                    lcd.lcdPrint(twist.getCount().ToString()); //udskriver på pladsen til cpr nummer
                                                               //x += countingIsPressed;
                    twist.setCount(0);
                    lcd.lcdGotoXY(x, 1);
                    // break;
                }
            }


            EmployeeIdAsString = EmployeeIdList.ToString();
            return EmployeeIdAsString;

        }

        private string socSecNb;
        public bool verifySocSecNb(string socSecNb)
        {
            int[] integer = new int[10];

            // TILFØJ KODE HER. Hvis antal cifre er forkert returner false

            for (int index = 0; index < 10; index++)
            {
                // TILFØJ KODE HER. Hvis karakteren på plads index i den modtagne streng ikke er et tal returner false

                // Karakteren på plads index konverteres til den tilhørende integer - eksempel '6' konverteres til 6
                integer[index] = Convert.ToInt16(number[index]) - 48;
            }

            // Algoritme der kotrollerer om cifrene danner et gyldigt personnummer
            if ((4 * integer[0] + 3 * integer[1] + 2 * integer[2] + 7 * integer[3] + 6 * integer[4] + 5 * integer[5] + 4 * integer[6] + 3 * integer[7] + 2 * integer[8] + integer[9]) % 11 != 0)
                return false;
            else
                return true;
        }
    }
}

