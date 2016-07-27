using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Microsoft.NaturalLanguage.KeyPhraseExtraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextAnalyticsHackathon.Models;

namespace TextAnalyticsHackathon.Utilities
{
    public class QueryKnowledgeGraph
    {
        private const string baseUri = "https://kgsearch.googleapis.com/v1/entities:search?key=AIzaSyCr8nMx3Qi0ANsAa-RfodtPdKU8-_Mg8yE&limit=1&indent=True&query=";
        public async Task GetEntities(List<SentenceResult> sentences)
        {
            foreach(SentenceResult sentence in sentences)
            {
                IEnumerable<string> queries = sentence.GetNouns();
                sentence.KnowledgeGraphCategories = new List<string>();
                foreach (string query in queries)
                {
                    using (var httpClient = new HttpClient())
                    {
                        try
                        {
                            var response = await (await httpClient.GetAsync(baseUri + HttpUtility.UrlEncode(query))).Content.ReadAsStringAsync();
                            var jsonResponse = JObject.Parse(response);
                            var entities = jsonResponse.SelectToken("itemListElement[0].result").SelectToken("@type");

                            foreach (JToken j in entities.Children())
                                if (!("Thing".Equals((string)j)) && !sentence.KnowledgeGraphCategories.Contains((string)j) )
                                    sentence.KnowledgeGraphCategories.Add((string)j);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }
                    }
                }
            }
        }
    }
}