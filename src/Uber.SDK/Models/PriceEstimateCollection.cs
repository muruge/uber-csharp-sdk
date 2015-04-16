using System.Collections.Generic;
using Newtonsoft.Json;

namespace Uber.SDK.Models
{
    public class PriceEstimateCollection
    {
        [JsonProperty(PropertyName = "prices")]
        public IList<PriceEstimate> PriceEstimates { get; set; }
    }
}
