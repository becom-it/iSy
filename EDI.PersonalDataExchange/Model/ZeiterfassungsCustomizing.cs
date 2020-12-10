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
        [JsonPropertyName("key")]
        public string AbscenceKey { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
