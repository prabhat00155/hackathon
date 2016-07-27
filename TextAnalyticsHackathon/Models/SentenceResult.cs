namespace TextAnalyticsHackathon.Models
{
    using System.Collections.Generic;

    public class SentenceResult
    {
        public string Text { get; set; }
        public double SentimentScore { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public EntityLinkResponse Entities { get; set; }
    }
}