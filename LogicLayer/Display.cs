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
    public class Display
    {
        public ADC1015 adc;
        public SerLCD lcd;
        public TWIST twist;

        LokalDB dataObjectRef;

        public Display(ADC1015 adc, SerLCD lcd, TWIST twist)
        {
            this.adc = adc;
            this.lcd = lcd;
            this.twist = twist;

            dataObjectRef = new LokalDB();

            //twist.setCount(0)

        }

        private string number;
        private bool result;



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
            int Id = dataObjectRef.CountRows();
            lcd.lcdPrint($"Dine data er sendt til den lokale database med IDnummer: {Id}");
        }



        //byte[] CprNumbersA = new byte[10];
        List<short> CprNumbersL = new List<short>();
        //byte x = 0;
        //byte y = 0;
        //byte checkNumerOfPress = 0;



        public string getSocSecNumber()
        {
            byte countingIsPressed;
            byte x = 6;
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
            //}
            return CprNumbersL.ToString();


        }

        public DTO_EKGMåling CreateEKGDTO()
        {
            
            DTO_EKGMåling Nymåling = new DTO_EKGMåling();
            return Nymåling;
        }
    }
}

