namespace TextAnalyticsHackathon.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.NaturalLanguage.ExtractionPreprocessing;

    public class SentenceResult
    {
        // text for this sentence
        public string Text { get; set; }

        // tokens in this sentence
        public IList<IPreprocessedToken> Tokens { get; set; }
        public double SentimentScore { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public EntityLinkResponse Entities { get; set; }

        public IEnumerable<string> GetNouns()
        {
            if (Tokens == null)
            {
                return Enumerable.Empty<string>();
            }
            return Tokens.Where(t => !t.IsStopWord && t.TokenTag == TokenTag.Noun).Select(t => t.Text);
        }
    }
}