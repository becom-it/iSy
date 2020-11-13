using System;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class EmployeeTimeDetail : ModelBase
    {
        /// <summary>
        /// Abwesenheitskennzeichen Halbtag 1 - zeaht1
        /// </summary>
        public string AbsentKey1 { get; set; }

        /// <summary>
        /// Abwesenheitskennzeichen Halbtag 2 - zeaht1
        /// </summary>
        public string AbsentKey2 { get; set; }

        /// <summary>
        /// Arbeitszeit Brutto - zeazbr
        /// </summary>
        public TimeSpan GrossWorktime { get; set; }

        /// <summary>
        /// Arbeitszeit Netto Anwesenheit - zeazna
        /// </summary>
        public TimeSpan NetWorktime { get; set; }

        /// <summary>
        /// Arbeitszeit Differenz Brutto - zeazdb
        /// </summary>
        public TimeSpan GrossWorktimeDifference { get; set; }

        /// <summary>
        /// Arbeitszeit Differenz Netto - zeazdn
        /// </summary>
        public TimeSpan NetWorktimeDifference { get; set; }

        /// <summary>
        /// Arbeitszeit Dienstgang - zeazdi
        /// </summary>
        public TimeSpan WorktimeOffSite { get; set; }

        /// <summary>
        /// Arbeitszeit Soll - zeazso
        /// </summary>
        public TimeSpan TargetWorktime { get; set; }

        /// <summary>
        /// Arbeitszeit Saldo Brutto - zeazsb
        /// </summary>
        public TimeSpan GrossWorktimeBalance { get; set; }

        /// <summary>
        /// Arbeitszeit Saldo Summe - zeazss
        /// </summary>
        public TimeSpan WorktimeBalanceSum { get; set; }

        /// <summary>
        /// Arbeitszeit Überstunden - zeazue
        /// </summary>
        public TimeSpan WorkOvertime { get; set; }

        /// <summary>
        /// Datum Anwesenheit - zedate
        /// </summary>
        public DateTime PresenceDate { get; set; }

        /// <summary>
        /// Datum Anwesenheit 2 - zedat2
        /// </summary>
        public DateTime PresenceDate2 { get; set; }

        /// <summary>
        /// Fabrikskalendertag - zefkal
        /// </summary>
        public int FactoryDate { get; set; }

        /// <summary>
        /// Art des Zeiterfassungsdatensatzes - zesart
        /// </summary>
        public int TimeType { get; set; }
    }
}
