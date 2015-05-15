using Newtonsoft.Json;

namespace NasaPicOfDay
{
   public class ImageNodeImage
   {
      [JsonProperty("fid")]
      public string FId { get; set; }
      [JsonProperty("uid")]
      public string UId { get; set; }
      [JsonProperty("filename")]
      public string FileName { get; set; }
      [JsonProperty("uri")]
      public string Uri { get; set; }
      [JsonProperty("filemime")]
      public string FileMime { get; set; }
      [JsonProperty("filesize")]
      public string FileSize { get; set; }
      [JsonProperty("status")]
      public string Status { get; set; }
      [JsonProperty("timestamp")]
      public string TimeStamp { get; set; }
      [JsonProperty("uuid")]
      public string UuId { get; set; }
      [JsonProperty("alt")]
      public string Alt { get; set; }
      [JsonProperty("title")]
      public string Title { get; set; }
      [JsonProperty("width")]
      public string Width { get; set; }
      [JsonProperty("height")]
      public string Height { get; set; }
      [JsonProperty("crop1x1")]
      public string Crop1X1 { get; set; }
      [JsonProperty("crop2x1")]
      public string Crop2X1 { get; set; }
      [JsonProperty("crop2x2")]
      public string Crop2X2 { get; set; }
      [JsonProperty("crop3x1")]
      public string Crop3X1 { get; set; }
      [JsonProperty("crop1x2")]
      public string Crop1X2 { get; set; }
      [JsonProperty("crop4x3ratio")]
      public string Crop4X3Ratio { get; set; }
      [JsonProperty("cropHumongo")]
      public string CropHumongo { get; set; }
      [JsonProperty("cropBanner")]
      public string CropBanner { get; set; }
      [JsonProperty("cropUnHoriz")]
      public string CropUnHoriz { get; set; }
      [JsonProperty("cropUnVert")]
      public string CropUnVert { get; set; }
      [JsonProperty("fullWidthFeature")]
      public string FullWidthFeature { get; set; }
      [JsonProperty("lrThumbnail")]
      public string LrThumbnail { get; set; }
   }
}
