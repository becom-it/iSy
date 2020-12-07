using Becom.EDI.PersonalDataExchange.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecordings.Models
{
    public class TimeRecordingsPresenceStatusViewModel
    {
        /// <summary>
        /// Personalnummer - pnpern
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Vorname - pnvnam
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Nachname - pnname
        /// </summary>
        public string LastName { get; set; }

        public PresenceType Type { get; set; }
    }
}
