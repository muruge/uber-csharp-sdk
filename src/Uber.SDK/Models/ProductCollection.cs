using System.Collections.Generic;
using Newtonsoft.Json;

namespace Uber.SDK.Models
{
    public class ProductCollection
    {
        [JsonProperty(PropertyName = "products")]
        public IList<Product> Products { get; set;  }
    }
}
