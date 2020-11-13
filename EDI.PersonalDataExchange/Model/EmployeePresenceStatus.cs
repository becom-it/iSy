using Becom.EDI.PersonalDataExchange.Model.Enums;
using System;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class EmployeePresenceStatus : ModelBase
    {
        /// <summary>
        /// Aktuelles Tagesdatum - z1date
        /// </summary>
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// Kenneichen ob Anwesend oder Abwesend: AN = Anwesend, AB = Abwesend - z1stat
        /// </summary>
        public PresenceType Type { get; set; }
    }
}
