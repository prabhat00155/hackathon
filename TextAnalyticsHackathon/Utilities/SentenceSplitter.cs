namespace TextAnalyticsHackathon.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using Microsoft.NaturalLanguage.KeyPhraseExtraction;
    using Models;

    public class SentenceSplitter
    {
        private readonly KeyPhraseExtractor extractor;

        public SentenceSplitter()
        {
            string modulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (modulePath == null)
            {
                // really shouldn't happen
                throw new Exception("Assembly.GetExecutingAssembly().CodeBase returned null");
            }

            string mrrFilePath = Path.Combine(modulePath, @"Utilities\KeyPhrasePreprocessor.en-us.mrr");
            string localMrrFilePath = new Uri(mrrFilePath).LocalPath;
            extractor = new KeyPhraseExtractor(localMrrFilePath);
            extractor.Configs.UseLemma = false;
            extractor.Configs.AddNouns = true;
            extractor.Configs.AddAdjs = true;
            extractor.Configs.AddVerbs = true;
        }

        public List<SentenceResult> Split(string input)
        {
            int tokenCount;
            var preprocessedSentences =
                extractor.m_preprocessor.Preprocess(input, out tokenCount);
            var sentenceResults = preprocessedSentences.Select(s => new SentenceResult() {Text = s.Text, Tokens = s.Tokens}).ToList();
            return sentenceResults;
        }
    }
}