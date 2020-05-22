using System;
using System.Collections.Generic;
using System.Text;

namespace DTO2
{
    /// <summary>
    /// Indeholder klasser der håndterer oprettelse af DTO objekter. 
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    {
    }
    /// <summary>
    /// Properties i klassen svarer til parametre der indgår i DTO_EKGMåling. 
    /// Properties har relation til kolonnenavne i tabellen SP_NyeEkger. 
    /// </summary>

    public class DTO_EKGMåling
    {
        /// <summary>
        /// MedarbejderID
        /// </summary>
        public string MedarbejderID { get; set; }
        /// <summary>
        /// CPR-nummer
        /// </summary>
        public string BorgerCPR { get; set; }
        /// <summary>
        /// Starttidspunkt for måling.
        /// </summary>
        public DateTime StarttidspunktMåling { get; set; }
        /// <summary>
        /// Array indeholdende samtlige målte værdier for en enkelt måling.
        /// </summary>
        public double[] RåData = new double[3000];
        /// <summary>
        /// Antallet af målepunkter for en hel måling.
        /// </summary>
        public int AntalMålepunkter { get; set; }
        /// <summary>
        /// Modtager beregnet værdi af samplerate i Hz. 
        /// </summary>
        public double SampleRateHz { get; set; }

        /// <summary>
        ///  /// Constructor til objekt af klassen. DTO indeholdende parametre der skal videresendes til databasen.
        /// <example>
        /// <code>
        /// {
        ///       MedarbejderID = MedarbejderID_;
        ///       BorgerCPR = BorgerCPR_;
        ///       StarttidspunktMåling = StarttidspunktMåling_;
        ///
        ///        for (int i = 0; i Rådata_.Length; i++)
        ///         {
        ///             RåData[i] = Rådata_[i];
        ///         }
        ///
        ///       RåData = Rådata_;
        ///       AntalMålepunkter = AntalMålepunkter_;
        ///       SampleRateHz = (1/(SampleRateHz_/1000)); 
        /// }
        ///</code>
        ///</example>
        /// </summary>
        /// <param name="MedarbejderID_">Indtastning fra Display.cs</param>
        /// <param name="BorgerCPR_">Indtastning fra Display.cs</param>
        /// <param name="StarttidspunktMåling_">Starttidspunkt for målingen. Property der sættes i klassen.</param>
        /// <param name="Rådata_">Samtlige målte værdier. </param>
        /// <param name="AntalMålepunkter_">Antallet af målte værdier</param>
        /// <param name="SampleRateHz_">Beregnet værdi: 1/(interval mellem målinger/1000)</param>
        public DTO_EKGMåling(string MedarbejderID_, string BorgerCPR_, DateTime StarttidspunktMåling_, double[] Rådata_, int AntalMålepunkter_, double SampleRateHz_)
        {
            MedarbejderID = MedarbejderID_;
            BorgerCPR = BorgerCPR_;
            StarttidspunktMåling = StarttidspunktMåling_;
            for (int i = 0; i < Rådata_.Length; i++)
            {
                RåData[i] = Rådata_[i];
            }
            RåData = Rådata_;
            AntalMålepunkter = AntalMålepunkter_;
            SampleRateHz = (1/(SampleRateHz_/1000)); 
        }
    }
}
