namespace TextAnalyticsHackathon.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Text;

    public class AnalysisResult
    {
        public List<SentenceResult> Sentences { get; set; }

        public Dictionary<string, Tuple<double, int>> WikipediaCategories = new Dictionary<string, Tuple<double, int>>();

        public List<string> CognitiveEntities = new List<string>();

        public Dictionary<string, Tuple<double, int>> GoogleEntities = new Dictionary<string, Tuple<double, int>>();

        public Dictionary<string, Tuple<double, int>> SatoriEntities = new Dictionary<string, Tuple<double, int>>();

        public AnalysisResult(List<SentenceResult> sentences)
        {
            Sentences = sentences;
            Prepare();
        }

        private void Prepare()
        {
            foreach (var sentence in Sentences)
            {
                foreach (var wikiCategory in sentence.MediaWikiCategories)
                {
                    if (!WikipediaCategories.ContainsKey(wikiCategory))
                    {
                        WikipediaCategories[wikiCategory] = new Tuple<double, int>(0, 0);
                    }
                    WikipediaCategories[wikiCategory] = new Tuple<double, int>(
                        WikipediaCategories[wikiCategory].Item1 + Math.Round(sentence.SentimentScore, 3), WikipediaCategories[wikiCategory].Item2 + 1);
                }
                CognitiveEntities = Sentences.Where(s => (s.Entities.EntityLinks != null)).SelectMany(s => (s.Entities.EntityLinks .Select(e => e.Name))).ToList();

                if (sentence.KnowledgeGraphCategories != null)
                {
                    foreach (var googleCat in sentence.KnowledgeGraphCategories)
                    {
                        if (!GoogleEntities.ContainsKey(googleCat))
                        {
                            GoogleEntities[googleCat] = new Tuple<double, int>(0, 0);
                        }
                        GoogleEntities[googleCat] = new Tuple<double, int>(
                            GoogleEntities[googleCat].Item1 + Math.Round(sentence.SentimentScore, 3),
                            GoogleEntities[googleCat].Item2 + 1);
                    }
                }

                if (sentence.SatoriCategories != null)
                {
                    foreach (var satoriCat in sentence.SatoriCategories)
                    {
                        if (!SatoriEntities.ContainsKey(satoriCat))
                        {
                            SatoriEntities[satoriCat] = new Tuple<double, int>(0, 0);
                        }
                        SatoriEntities[satoriCat] = new Tuple<double, int>(
                            SatoriEntities[satoriCat].Item1 + Math.Round(sentence.SentimentScore, 3),
                            SatoriEntities[satoriCat].Item2 + 1);
                    }
                }

            }
        }

        public string ToHtml()
        {
            StringBuilder str = new StringBuilder();
            str.Append("<table class=\"table\"><thead><tr><th>Category</th><th>Avg sentiment</th></thead><tbody>");
            foreach (var kvp in WikipediaCategories)
            {
                str.Append("<tr><td>" + kvp.Key + "</td><td>" + (kvp.Value.Item1 / kvp.Value.Item2) + "</td></tr>");
            }
            str.Append("</tbody></table>");

            if (CognitiveEntities.Any())
            {
                str.Append(
                    "<table class=\"table\"><thead><tr><th>Entity</th></thead><tbody>");
                foreach (var entity in CognitiveEntities)
                {
                    str.Append("<tr><td>" + entity + "</td></tr>");
                }
            }

            if (GoogleEntities.Any())
            {
                str.Append("<table class=\"table\"><thead><tr><th>Google category</th><th>Avg sentiment</th></thead><tbody>");
                foreach (var kvp in GoogleEntities)
                {
                    str.Append("<tr><td>" + kvp.Key + "</td><td>" + (kvp.Value.Item1 / kvp.Value.Item2) + "</td></tr>");
                }
                str.Append("</tbody></table>");
                
            }

            if (SatoriEntities.Any())
            {
                str.Append("<table class=\"table\"><thead><tr><th>Satori category</th><th>Avg sentiment</th></thead><tbody>");
                foreach (var kvp in SatoriEntities)
                {
                    str.Append("<tr><td>" + kvp.Key + "</td><td>" + (kvp.Value.Item1 / kvp.Value.Item2) + "</td></tr>");
                }
                str.Append("</tbody></table>");
                
            }
            return str.ToString();
        }
    }
}