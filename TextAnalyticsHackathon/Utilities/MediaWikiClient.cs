using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TextAnalyticsHackathon.Models;

namespace TextAnalyticsHackathon.Utilities
{
    public class MediaWikiClient
    {
        private const string baseUri = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=categories&titles=";

        public async Task GetCategories(List<SentenceResult> sentences)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var sentence in sentences)
                {
                    var response = await (await httpClient.GetAsync(baseUri + HttpUtility.UrlEncode(sentence.Text))).Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(response);
                    var page = jsonResponse.SelectToken("query")?.SelectToken("pages")?.First?.First;
                    var categoriesToken = page?.SelectToken("categories");
                    List<string> categoriesList = new List<string>();
                    foreach (var categoryToken in categoriesToken?.Children())
                    {
                        var category = categoryToken?.SelectToken("title")?.ToString();
                        if (IsValidCategory(category))
                        {
                            categoriesList.Add(ParseCategory(category));
                        }                       
                    }
                    sentence.Categories = categoriesList;
                }
            }
        }

        private string ParseCategory(string category)
        {
            var prefix = "Category:";
            if (category?.StartsWith(prefix) ?? false)
                return category.Substring(prefix.Length);
            return category;
        }

        private bool IsValidCategory(string category)
        {
            return category != null && !(category.StartsWith("Category:All") || category.StartsWith("Category:Article") || category.StartsWith("Category:Commons")
                    || category.StartsWith("Category:Pages") || category.StartsWith("Category:Wikipedia"));
        }
    }
}