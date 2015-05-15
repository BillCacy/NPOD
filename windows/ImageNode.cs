using System.Collections.Generic;
using Newtonsoft.Json;

namespace NasaPicOfDay
{
   public class ImageNode
   {
      [JsonProperty("images")]
      public List<ImageNodeImage> ImageData { get; set; }

      [JsonProperty("ubernode")]
      public ImageNodeUber UberData { get; set; }
   }
}
