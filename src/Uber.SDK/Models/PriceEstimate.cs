﻿using Newtonsoft.Json;

namespace Uber.SDK.Models
{
    public class PriceEstimate
    {
        [JsonProperty(PropertyName = "product_id")]
        public string ProductId { get; set; }

        [JsonProperty(PropertyName = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "estimate")]
        public string Estimate { get; set; }

        [JsonProperty(PropertyName = "low_estimate")]
        public float? LowEstimate { get; set; }

        [JsonProperty(PropertyName = "high_estimate")]
        public float? HighEstimate { get; set; }

        [JsonProperty(PropertyName = "surge_multiplier")]
        public float SurgeMultiplier { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }
    }
}
