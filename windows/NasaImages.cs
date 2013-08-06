using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NasaPicOfDay
{
    public class NasaImages
    {
        [JsonProperty("nodes")]
        public Node[] Nodes { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
