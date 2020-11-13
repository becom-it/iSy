using Becom.EDI.PersonalDataExchange.Model.Enums;
using System;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class EmployeeCheckIn : ModelBase
    {
        /// <summary>
        /// Abgefragtes Datum - z1Date
        /// </summary>
        public DateTime AskedForDate { get; set; }

        /// <summary>
        /// Kenneichen ob Anwesend oder Abwesend: AN = Anwesend, AB = Abwesend - z1stat
        /// </summary>
        public PresenceType Type { get; set; }

        /// <summary>
        /// Zeitpunkt der Statusänderung - z1Time
        /// </summary>
        public DateTime CheckinTime { get; set; }

        /// <summary>
        /// Abwesenheitskennzeichen, Grund für Abwesenheit (z.B.: Pause, Dienstgang) - z1aht1
        /// </summary>
        public string AbsenceType { get; set; }
    }
}
