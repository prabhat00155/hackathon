using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TextAnalyticsHackathon.Models;

namespace TextAnalyticsHackathon.Utilities
{
    public class SatoriClient
    {
        private const string baseUri = "https://www.bing.com/api/v5/entities/custom/SatoriTextAnalytics/Search?appid=D41D8CD98F00B204E9800998ECF8427E2CDA1C5E&q=";

        public async Task GetCategories(List<SentenceResult> sentences)
        {
            await Task.WhenAll(sentences.Select(async sentence =>
            {
                var nounsCategories = await Task.WhenAll(sentence.GetNouns().Select(async noun => await GetCategoriesForNoun(noun)));
                sentence.SatoriCategories = nounsCategories.SelectMany(nounCategories => nounCategories).ToList();
            }));
        }

        private async Task<IEnumerable<string>> GetCategoriesForNoun(string noun)
        {
            using (var httpClient = new HttpClient())
            {
                IEnumerable<string> categoriesList = new List<string>();
                var response = await (await httpClient.GetAsync(baseUri + HttpUtility.UrlEncode(noun))).Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(response);
                return GetCategoriesInResponse(jsonResponse);
            }
        }

        private IEnumerable<string> GetCategoriesInResponse(JToken jsonResponse)
        {
            if (jsonResponse == null) return null;
            var bestEntity = jsonResponse.SelectToken("answers").First;
            if (bestEntity == null) return null;
            var properties = bestEntity.SelectToken("value").First.SelectToken("properties");

            List<string> categoriesList = new List<string>();
            if (properties == null) return categoriesList;

            JToken categoriesToken = null;
            foreach (var property in properties.Children())
            {
                var propertyToken = property.SelectToken("propertyName").ToString();
                if (propertyToken == "Type")
                {
                    categoriesToken = property.SelectToken("values");
                }
            }
            if (categoriesToken == null) return categoriesList;
            foreach (var categoryToken in categoriesToken.Children())
            {
                var category = categoryToken.SelectToken("value").ToString();
                if (IsValidCategory(category))
                {
                    categoriesList.Add(ParseCategory(category));
                }
            }
            return categoriesList;
        }

        private string ParseCategory(string category)
        {
            if (category.StartsWith("mso:"))
                category = category.Split(new char[] {':', '.'}).Last();

            return string.Join(" ",category.Split('_'));
        }

        private bool IsValidCategory(string category)
        {
            return category != null && !(category.Split(' ').Count() > 2 || category.StartsWith("PrecisionGraphEntity") || category.StartsWith("http") || category.StartsWith("dev:"));
        }
    }
}