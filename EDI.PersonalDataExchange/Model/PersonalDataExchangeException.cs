using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class PersonalDataExchangeException : Exception
    {
        public ErrorResponse ErrorResponse { get; set; }

        public PersonalDataExchangeException(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }
    }
}
