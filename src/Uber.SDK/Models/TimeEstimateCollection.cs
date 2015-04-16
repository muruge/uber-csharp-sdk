using System.Collections.Generic;
using Newtonsoft.Json;

namespace Uber.SDK.Models
{
    public class TimeEstimateCollection
    {
        [JsonProperty(PropertyName = "times")]
        public IList<TimeEstimate> TimeEstimates { get; set; }
    }
}
