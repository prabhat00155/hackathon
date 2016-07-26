namespace TextAnalyticsHackathon.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public abstract class BatchResultV2<T>
    {
        [JsonProperty(PropertyName = "documents")]
        public IList<T> Documents { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public IList<ErrorRecordV2> Errors { get; set; }
    }
}
