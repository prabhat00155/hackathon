namespace TextAnalyticsHackathon.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;

    public class SentimentClient
    {
        private const string key = "b677d3ef28ea4c6ca06afa9c24080377";
        private const string baseUri = "https://westus.api.cognitive.microsoft.com/";

        public async Task GetSentiment(List<SentenceResult> sentences)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUri);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var uri = "text/analytics/v2.0/sentiment";

                foreach (var sentence in sentences)
                {
                    var input = new SentimentInput(sentence.Text);
                    var response = await (await httpClient.PostAsJsonAsync(uri, input)).Content.ReadAsStringAsync();
                    var sentimentResult = JsonConvert.DeserializeObject<SentimentBatchResultV2>(response);
                    sentence.SentimentScore = sentimentResult.Documents[0].Score;
                }
            }
        }
    }
}