using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;

namespace PRJ2_lcdDisplay
{
    public class socSecNumber
    {
        public ADC1015 adc;
        public SerLCD lcd;
        public TWIST twist;

        public socSecNumber(ADC1015 adc, SerLCD lcd, TWIST twist)
        {
            this.adc = adc;
            this.lcd = lcd;
            this.twist = twist;
        }

        byte[] CprNumbersA = new byte[10];
        List<short> CprNumbersL = new List<short>();
        byte x = 0;
        byte y = 0;
        byte checkNumerOfPress = 0;

        
      
        public string getSocSecNumber()
        {
            byte countingIsPressed;

            lcd.lcdPrint("Indtast CPR nummer");

            lcd.lcdGotoXY(0, 2);
            
            for (byte i = 0; i < CprNumbersA.Length; i++)
            {
                CprNumbersA[i] = i;

                for (countingIsPressed = 0; countingIsPressed < CprNumbersA.Length; countingIsPressed++)
                {
                    while (twist.isPressed() == false)
                    { }
                    if (twist.isPressed()==true)
                    {
                        CprNumbersL.Add(twist.getCount());
                        lcd.lcdGotoXY(x, 1);

                        //lcd.lcdPrint("^");
                        //lcd.lcdGotoXY(x, 3);
                        //lcd.lcdPrint("v");
                        //lcd.lcdGotoXY(x, 2);
                        lcd.lcdPrint(CprNumbersA[twist.getCount()].ToString());
                        //x += countingIsPressed;
                        twist.setCount(0);
                        lcd.lcdGotoXY(x, 2);

                        break;
                    }
                } 
            }
            return CprNumbersL.ToString();
            //Er det i denne metode jeg skal udskrive på LCD display? Eller er denne metode bare til at gemme tallene fra mit CPR?
            //Prøv at brug det til at gemme. Så er det nok lettere at udskrive senere
          
        }
    }
}
