using Newtonsoft.Json;

namespace NasaPicOfDay
{
   public class ImageNodeUber
   {
      [JsonProperty("title")]
      public string Title { get; set; }
      [JsonProperty("nid")]
      public string NId { get; set; }
      [JsonProperty("type")]
      public string Type { get; set; }
      [JsonProperty("changed")]
      public string Changed { get; set; }
      [JsonProperty("uuid")]
      public string UuId { get; set; }
      [JsonProperty("body")]
      public string Body { get; set; }
      [JsonProperty("name")]
      public string Name { get; set; }
      [JsonProperty("uri")]
      public string Uri { get; set; }
      [JsonProperty("collections")]
      public object[] Collections { get; set; }
      [JsonProperty("enableComments")]
      public string EnableComments { get; set; }
      [JsonProperty("linkOrAttachment")]
      public string LinkOrAttachment { get; set; }
      [JsonProperty("masterImage")]
      public string MasterImage { get; set; }
      [JsonProperty("missions")]
      public object[] Missions { get; set; }
      [JsonProperty("primaryTag")]
      public string PrimaryTag { get; set; }
      [JsonProperty("promoDateTime")]
      public string PromoDateTime { get; set; }
      [JsonProperty("routes")]
      public object[] Routes { get; set; }
      [JsonProperty("secondaryTag")]
      public string SecondaryTag { get; set; }
      [JsonProperty("topics")]
      public object[] Topics { get; set; }
      [JsonProperty("ubernodeImage")]
      public string UberNodeImage { get; set; }
      [JsonProperty("ubernodeType")]
      public string UberNodeType { get; set; }
      [JsonProperty("imageFeatureCaption")]
      public string ImageFeatureCaption { get; set; }
      [JsonProperty("cardfeedTitle")]
      public string CardFeedTitle { get; set; }
      [JsonProperty("onS3")]
      public string OnS3 { get; set; }
   }
}
