using Newtonsoft.Json;

namespace NasaPicOfDay
{
   public class UberMeta
   {
      [JsonProperty("total_rows")]
      public string TotalRows { get; set; }
   }
}
