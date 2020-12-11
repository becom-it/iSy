using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class ZeiterfassungsCustomizing
    {
        [JsonPropertyName("KEY")]
        public string AbscenceKey { get; set; }

        [JsonPropertyName("DESCRIPTION")]
        public string Description { get; set; }
    }
}
