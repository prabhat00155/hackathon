//-----------------------------------------------------------------------
// <copyright file="SentimentBatchResultItemV2.cs" company="Microsoft Corporation">
//     Copyright Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TextAnalyticsHackathon.Models
{
    using Newtonsoft.Json;

    public class SentimentBatchResultItemV2
    {
        public SentimentBatchResultItemV2()
        {
        }


        /// <summary>
        /// A decimal number between 0 and 1 denoting the sentiment of the document. 
        /// A score above 0.7 usually refers to a positive document while a score below 0.3 normally has a negative connotation.
        /// Mid values refer to neutral text.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }

        /// <summary>
        /// Unique document identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }        
    }
}