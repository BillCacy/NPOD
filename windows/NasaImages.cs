using Newtonsoft.Json;

namespace NasaPicOfDay
{
	public class NasaImages
	{
		[JsonProperty("ubernodes")]
		public UberNode[] UberNodes { get; set; }

		[JsonProperty("meta")]
		public UberMeta Meta { get; set; }
	}
}
