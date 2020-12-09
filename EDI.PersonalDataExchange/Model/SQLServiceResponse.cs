using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Becom.EDI.PersonalDataExchange.Model
{
    public class SQLServiceResponse<T>
    {
        [JsonPropertyName("metrics")]
        public Metrics? Metrics { get; set; }
        [JsonPropertyName("data")]
        public List<T>? Data { get; set; }
    }

    public class Metrics
    {
        [JsonPropertyName("took")]
        public string Took { get; set; } = string.Empty;
        [JsonPropertyName("sqlTime")]
        public string SqlTime { get; set; } = string.Empty;
    }
}
