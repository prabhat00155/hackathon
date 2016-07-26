namespace TextAnalyticsHackathon.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;

    public class CognitiveEntityLinkingClient
    {
        private const string baseUri = "https://api.projectoxford.ai/";
        private const string key = "a1846309782d4b16802e7d450f0aadca";

        public async Task GetEntities(List<SentenceResult> sentences)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUri);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                var uri = "entitylinking/v1.0/link";

                foreach (var sentence in sentences)
                {
                    var response =
                            (await httpClient.PostAsync(uri, new StringContent(sentence.Text)));
                    var content = await response.Content.ReadAsStringAsync();
                    sentence.Entities = JsonConvert.DeserializeObject<EntityLinkResponse>(content);
                }
            }
        }
    }
}