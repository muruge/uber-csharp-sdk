using Newtonsoft.Json;

namespace Uber.SDK.Models
{
    public class RequestMap
    {
        [JsonProperty(PropertyName = "request_id")]
        public string RequestId { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }
}
