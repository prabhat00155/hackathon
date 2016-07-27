using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            await Task.WhenAll( sentences.Select(async sentence =>
           {
               var nounsCategories = await Task.WhenAll(sentence.GetNouns().Select(async noun => await GetCategoriesForNoun(noun)));
               sentence.MediaWikiCategories = nounsCategories.SelectMany(nounCategories => nounCategories).ToList();
           }));
        }

        private async Task<IEnumerable<string>> GetCategoriesForNoun(string noun)
        {
            using (var httpClient = new HttpClient())
            {
                var queryCountLimit = 5;
                string clContinueToken = null, continueToken = null;
                IEnumerable<string> categoriesList = new List<string>();
                string response = null;
                bool isFirstQuery = true;
                do
                {
                    response = await (await httpClient.GetAsync(baseUri + HttpUtility.UrlEncode(noun) + GetContinueParams(isFirstQuery, clContinueToken, continueToken)))
                                    .Content.ReadAsStringAsync();
                   
                    clContinueToken = continueToken = null;
                    var jsonResponse = JObject.Parse(response);
                    categoriesList = categoriesList.Concat(GetCategoriesInResponse(jsonResponse));
                    isFirstQuery = false;
                    if (jsonResponse != null)
                    {
                        var continueSectionToken = jsonResponse.SelectToken("continue");
                        if (continueSectionToken != null)
                        {
                            continueToken = continueSectionToken.SelectToken("continue") != null ? continueSectionToken.SelectToken("continue").ToString() : null;
                            clContinueToken = continueSectionToken.SelectToken("clcontinue") != null ? continueSectionToken.SelectToken("clcontinue").ToString() : null;
                        }
                    }
                } while (!string.IsNullOrEmpty(clContinueToken) && !string.IsNullOrEmpty(continueToken) && --queryCountLimit > 0);
                return categoriesList;
            }
        }

        private string GetContinueParams(bool isFirstQuery, string clContinueToken, string continueToken)
        {
            if (isFirstQuery || string.IsNullOrEmpty(clContinueToken) || string.IsNullOrEmpty(continueToken)) return "";
            return string.Format("&continue={0}&clcontinue={1}",continueToken,clContinueToken);
        }

        private IEnumerable<string> GetCategoriesInResponse(JToken jsonResponse)
        {           
            var page = jsonResponse != null ? jsonResponse.SelectToken("query").SelectToken("pages").First.First : null;
            var categoriesToken = page != null ? page.SelectToken("categories") : null;
            List<string> categoriesList = new List<string>();
            foreach (var categoryToken in categoriesToken.Children())
            {
                var category = categoryToken != null ? categoryToken.SelectToken("title").ToString() : null;
                if (IsValidCategory(category))
                {
                    categoriesList.Add(ParseCategory(category));
                }
            }
            return categoriesList;
        }

        private string ParseCategory(string category)
        {
            var prefix = "Category:";
            if (category != null ? category.StartsWith(prefix) : false)
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