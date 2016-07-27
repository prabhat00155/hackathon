using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextAnalyticsHackathon.Models
{
    using Newtonsoft.Json;

    public class CategoryInput
    {
        [JsonProperty(PropertyName = "documents")]
        public List<SentimentInputRow> Documents { get; set; }

        public CategoryInput(string text)
        {
            Documents = new List<SentimentInputRow>()
            {
                new SentimentInputRow() { Id = "0", Text = text}
            };
        }
    }

    public class CategoryInputRow
    {
        public string Id { get; set; }
        public string Text { get; set; }

    }
}