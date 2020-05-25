using System;
using System.Threading;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using LogicLayer;
using System.Collections.Immutable;
//using System.Reflection.Metadata.Ecma335;


namespace PresentationLayer
{
    /// <summary>
    /// Varetager den overordnede og gennemgående kommunikation mellem systemets lag. 
    /// </summary>
    public class Communication
    {
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private SerLCD lcd;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private TWIST twist;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private Display displayRef;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private Ekg_Record ekgRecordRef;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private Battery batteryRef;
        /// <summary>
        /// Reference til objekt af klassen.
        /// </summary>
        private BatteryStatus batteryStatusRef;
        

        /// <summary>
        /// Constructor for objekt af klassen. Instansierer referencerne. 
        /// </summary>        
        public Communication()
        {
            lcd = new SerLCD();
            twist = new TWIST();
            twist.setCount(0);
            ekgRecordRef = new Ekg_Record();
            displayRef = new Display();
            batteryRef = new Battery();
            batteryStatusRef = new BatteryStatus();

        }

        /// <summary>
        /// Kalder de respektive metoder gennem de tre lag, og sikrer det ønskede flow i programmets afvikling
        /// <example>
        /// <code>
        /// Pseudo kode:
        ///1. Tænder lcddisplay, og nulstiller lcddisplay.
        ///2. Aktiverer cursor, og aktiverer blink på cursor
        ///3. Sætter baggrundsbelysning på lcddisplay til værdien(0,0,0)
        ///4. Kører Batterry.ChargeBattery()
        ///5. Kører Battery.ShowBatteryStatus
        ///6. Kører Display.GetEmployeeID()
        ///7. Kører Display.VerifyEmployeeID()
        ///8. Hvis metoden returnerer false, gives 3 forsøg til indtastning af korrekt medarbejderID. 
        ///9. Første forsøg inklusiv.Ved forkert indtastning udskrives ”ID ikke godkendt”.
        ///10. Der ventes 1 sekund før forsøg på ny indtastning gives.
        ///11. Indtastes korrekt ID stadig ikke, udskrives ”Du har brugt dine 3 forsøg.Programmet lukkes.” Programmet lukker efter 3 sekunder.
        ///12. Ved korrekt indtastet medarbejderID nulstilles lcddisplay, og der udskrives ”ID godkendt”, hvorefter der ventes i 1 sekund.
        ///13. Udskriver ”Maaling med CPR?”. Herefter køres Display.Yes_No(). Returneres true, køres Display.GetSocSecNumber().
        ///14. Ellers Display.SocSecNumberAsString = ”11111111”
        ///15. Udskriver "Start Ekg maaaling?"
        ///16. Herefter køres Display.Yes_No().
        ///17. Vælges ja, udskrives "Maaling paabegyndt"
        ///18. ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString)
        ///19. Når målingen er afsluttet udskrives "Maaling afsluttet".
        ///20. ekgRecordRef.GetReceipt() udskriver kvittering for afsendelse + ID-nummer på målingen.
        ///21. batteryRef.ShowBatteryStatus() udskriver batteristatus.
        ///22. Er batteristatus lav, udskrives ""Lavt batteri        Tilslut oplader     ".
        ///23. Program afsluttes.
        ///
        /// OBS!    Det er muligt at vælge ny måling efter punkt 20. Vælges dette, startes forfra ved punkt 18.
        ///         Det er muligt at vælge nej i punkt 17. Vælges dette, springes der direkte til punkt 21. 
        /// </code>
        /// </example>
        /// </summary>
        public void Program()
        {
            bool continueEKGMeasurement = false;
            bool answer;
            lcd.lcdNoDisplay();
            lcd.lcdDisplay(); //Tænder skærmen
            lcd.lcdClear(); // Nulstiller skærm
            lcd.lcdCursor(); //Tænder for cursoren
            lcd.lcdBlink();
            lcd.lcdSetBackLight(0, 0, 0);
            Thread.Sleep(1000);



            batteryStatusRef.ChargeBattery(); // Hvis oplader er tilstluttet køres denne metode - exit program. 
            batteryStatusRef.ShowBatteryStatus(); // Ændrer baggrundsfarven efter batteristatus
            Thread.Sleep(1000);

            displayRef.GetEmployeeId(); //Medarbejderen logger ind


            displayRef.VerifyEmployeeId(displayRef.EmployeeIdAsString);
            {
                while (displayRef.VerifyEmployeeId(displayRef.EmployeeIdAsString) == false)
                {
                    int index = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        
                        lcd.lcdClear();
                        lcd.lcdGotoXY(2, 1);
                        lcd.lcdPrint("ID ikke godkendt");
                        Thread.Sleep(3000);
                        displayRef.GetEmployeeId();

                        index++;
                        if (displayRef.VerifyEmployeeId(displayRef.EmployeeIdAsString) == true)
                        {
                            break;
                        }
                        if (index == 3)
                        {
                            for (int n = 0; n < 1; n++)
                            {
                                lcd.lcdClear();
                                lcd.lcdPrint("Du har brugt dine 3  forsøg. Programmet  lukkes");
                                Thread.Sleep(1000);
                            }
                            Environment.Exit(0);
                        }
                    }
                }
                lcd.lcdClear();
                lcd.lcdGotoXY(4,1);
                lcd.lcdPrint("ID godkendt");
                Thread.Sleep(3000);
            }


            lcd.lcdClear();
            lcd.lcdPrint("Maaling med CPR?");

            if (displayRef.Yes_No() == true)
            {
                displayRef.GetSocSecNumber(); //Skriver nummerlinjen + cpr
            }
            else
            {
                displayRef.SocSecNumberAsString = "1111111111";
            }

            lcd.lcdClear();
            lcd.lcdGotoXY(0, 0);
            lcd.lcdPrint("Start Ekg maaling?");

            answer = displayRef.Yes_No();
            do
            {
                if (answer == true)
                {
                    lcd.lcdClear();
                    lcd.lcdPrint("Maaling paabegyndt");
                    ekgRecordRef.CreateEKGDTO(displayRef.EmployeeIdAsString, displayRef.SocSecNumberAsString); //Starter målingen); //Opretter en DTO

                    while (ekgRecordRef.StartEkgRecord() == false) // Venter her indtil metoden returnerer true = måling færdig
                    { }
                    lcd.lcdClear();
                    lcd.lcdPrint("Maaling afsluttet");
                    Thread.Sleep(3000);


                    lcd.lcdClear();
                    lcd.lcdGotoXY(0, 2);
                    lcd.lcdPrint($"Dine data er sendt  med IDnr: {ekgRecordRef.GetReceipt()}"); //Kan ikke lade sig gøre, da vi kun kan gennemgå vores database gennem filer som bindeled.
                    Thread.Sleep(3000);

                    lcd.lcdClear();
                    lcd.lcdPrint("Ny maaling?");

                    answer = displayRef.Yes_No();
                    continueEKGMeasurement = answer; 
                }
            
            } while (continueEKGMeasurement);


            lcd.lcdClear();
            lcd.lcdPrint($"Batteristatus: {batteryRef.ShowBatteryStatus()} %");
            Thread.Sleep(3000);
            if (batteryRef.ShowBatteryStatus() < 20)
            {
                lcd.lcdGotoXY(0, 1);
                lcd.lcdPrint("Lavt batteri        Tilslut oplader     ");
                Thread.Sleep(3000);
            }
            lcd.lcdGotoXY(0, 3);
            lcd.lcdPrint("Program afsluttes");
            Thread.Sleep(2000);
            lcd.lcdSetBackLight(0, 0, 0);
            lcd.lcdClear();
            lcd.lcdNoDisplay();
        }
    }
}