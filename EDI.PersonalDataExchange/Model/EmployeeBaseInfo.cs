namespace Becom.EDI.PersonalDataExchange.Model
{
    public class EmployeeBaseInfo : ModelBase
    {
        /// <summary>
        /// Vorname - pnvnam
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Nachname - pnname
        /// </summary>
        public string LastName { get; set; }
    }
}