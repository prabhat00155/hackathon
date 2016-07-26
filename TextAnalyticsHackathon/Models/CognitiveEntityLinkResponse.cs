namespace TextAnalyticsHackathon.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class EntityLinkResponse
    {
        /// <summary>
        /// Gets or sets EntityLinks.
        /// </summary>
        [JsonProperty(PropertyName = "entities")]
        public EntityLink[] EntityLinks { get; set; }
    }

    /// <summary>
    /// Represents name of the entity, wikipedia id, matches and score.
    /// </summary>
    public class EntityLink
    {
        /// <summary>
        /// Name of the entity.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Id of the wikipedia linked entity.
        /// </summary>
        [JsonProperty(PropertyName = "wikipediaId")]
        public string WikipediaID { get; set; }

        /// <summary>
        /// Matches text of the entity within the given paragraph.
        /// </summary>
        [JsonProperty(PropertyName = "matches")]
        public IList<Match> Matches { get; set; }

        /// <summary>
        /// Confidence score of the linking.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }

    /// <summary>
    /// The match text of the knowledge base linked entity.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// The matched text.
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Matched entries within the given paragraph.
        /// </summary>
        [JsonProperty(PropertyName = "entries")]
        public IList<Entry> Entries { get; set; }
    }

    /// <summary>
    /// Represents the entry offset and score.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// Offset of the entry.
        /// </summary>
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        /// <summary>
        /// The score of the entry.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }
}