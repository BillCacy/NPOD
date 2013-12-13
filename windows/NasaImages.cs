using Newtonsoft.Json;

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
