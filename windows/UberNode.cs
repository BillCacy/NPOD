using Newtonsoft.Json;

namespace NasaPicOfDay
{
    public class UberNode
    {
        [JsonProperty("type")]
        public string UberNodeType { get; set; }

        [JsonProperty("nid")]
        public string UberNodeId { get; set; }

        [JsonProperty("promoDateTime")]
        public string UberPromoData { get; set; }
    }
}
