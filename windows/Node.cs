using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NasaPicOfDay
{
    public class Node
    {
        [JsonProperty]
        public Node2 node { get; set; }
    }
}
