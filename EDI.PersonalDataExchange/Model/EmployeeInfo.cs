using System;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class EmployeeInfo : EmployeeBaseInfo
    {
        /// <summary>
        /// Vorgesetzter Disziplinär (Personalnummer) - pnvgsd
        /// </summary>
        public int ManagerDisciplinary { get; set; }

        /// <summary>
        /// Vorgesetzter Fachlich (Personalnummer) - pnvgsf
        /// </summary>
        public int ManagerProfessional { get; set; }

        /// <summary>
        /// Eintrittsdatum - pneind
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Arbeitszeit Saldo Summe - pnazss
        /// </summary>
        public TimeSpan WorktimeSaldo { get; set; }

        /// <summary>
        /// Freizeitoption (offener Stundenpool) - pnfzop
        /// </summary>
        public TimeSpan FreeTimeOption { get; set; }

        /// <summary>
        /// Urlaub Gesamt (Verfügbarer Urlaub) - pnurlg
        /// </summary>
        public int HolidayAvailiable { get; set; }

        /// <summary>
        /// Urlaub Verbraucht (Eingetragener Urlaub) - pnurlv
        /// </summary>
        public int HolidayUsed { get; set; }
    }
}
