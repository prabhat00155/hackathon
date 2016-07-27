
namespace TextAnalyticsHackathon.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Utilities;

    public class HomeController : Controller
    {
        private readonly SentenceSplitter sentenceSplitter;
        private readonly SentimentClient sentimentClient;
        private readonly CognitiveEntityLinkingClient entityClient;
        private readonly MediaWikiClient mediaWikiClient;
        private readonly QueryKnowledgeGraph queryKnowlegeGraph;

        public HomeController()
        {
            sentenceSplitter = new SentenceSplitter();
            sentimentClient = new SentimentClient();
            mediaWikiClient = new MediaWikiClient();
            entityClient = new CognitiveEntityLinkingClient();
            queryKnowlegeGraph = new QueryKnowledgeGraph();
        }

        public async Task<ActionResult> Analyze(string inputText)
        {
            var sentences = sentenceSplitter.Split(inputText);
            await sentimentClient.GetSentiment(sentences);
            await entityClient.GetEntities(sentences);
            await mediaWikiClient.GetCategories(sentences);
            await queryKnowlegeGraph.GetEntities(sentences);
            return Content(JsonConvert.SerializeObject(sentences), "text/html");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}