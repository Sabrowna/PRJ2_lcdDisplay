using System;
using System.Threading;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LocalDB;

namespace LogicLayer
{
    public class ekgRecord 
    {

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
