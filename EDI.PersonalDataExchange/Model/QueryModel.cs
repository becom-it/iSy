using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public enum QueryType
    {
        UNKNOWN,
        SELECT,
        UPDATE,
        INSERT,
        DELETE
    }
    public class QueryModel
    {
        public string Query { get; set; } = string.Empty;
    }
}
