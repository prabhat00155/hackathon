//-----------------------------------------------------------------------
// <copyright file="ErrorRecordV2.cs" company="Microsoft Corporation">
//     Copyright Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TextAnalyticsHackathon.Models
{
    using Newtonsoft.Json;

    public class ErrorRecordV2
    {
        public ErrorRecordV2()
        {
        }

        /// <summary>
        /// Input document unique identifier the error refers to. 
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}