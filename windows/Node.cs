using Newtonsoft.Json;

namespace NasaPicOfDay
{
    public class Node
    {
        [JsonProperty("node")]
        public Node2 node { get; set; }
    }
}
