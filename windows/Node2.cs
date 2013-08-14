using Newtonsoft.Json;

namespace NasaPicOfDay
{
    public class Node2
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("image_caption")]
        public string ImageCaption { get; set; }

        [JsonProperty("trimmed_image_caption")]
        public string TrimmedImageCaption { get; set; }

        [JsonProperty("promotional_title")]
        public string PromotionalTitle { get; set; }

        [JsonProperty("promotional_leader_sentence")]
        public string PromotionalLeaderSentence { get; set; }

        [JsonProperty("promotional_date")]
        public string PromotionalDate { get; set; }

        [JsonProperty("master_image")]
        public string MasterImage { get; set; }

        [JsonProperty("thumbnail_image_346x260")]
        public string ThumbnailImage346x260 { get; set; }

        [JsonProperty("thumbnail_image_466x248")]
        public string ThumbnailImage466x248 { get; set; }

        [JsonProperty("thumbnail_image_226x170")]
        public string ThumbnailImage226x170 { get; set; }

        [JsonProperty("thumbnail_image_100x75")]
        public string ThumbnailImage100x75 { get; set; }

        [JsonProperty("image_346x260")]
        public string Image346x260 { get; set; }

        [JsonProperty("image_466x248")]
        public string Image466x248 { get; set; }

        [JsonProperty("image_226x170")]
        public string Image226x170 { get; set; }

        [JsonProperty("image_360x225")]
        public string Image360x225 { get; set; }

        [JsonProperty("image_430x323")]
        public string Image430x323 { get; set; }

        [JsonProperty("image_100x75")]
        public string Image100x75 { get; set; }

        [JsonProperty("image_1600x1200")]
        public string Image1600x1200 { get; set; }

        [JsonProperty("image_800x600")]
        public string Image800x600 { get; set; }

        [JsonProperty("image_1024x768")]
        public string Image1024x768 { get; set; }

        [JsonProperty("topics")]
        public string[] Topics { get; set; }

        [JsonProperty("missions")]
        public string[] missions { get; set; }

        [JsonProperty("collections")]
        public object[] Collections { get; set; }

        [JsonProperty("other_tags")]
        public object[] OtherTags { get; set; }

        [JsonProperty("routes")]
        public string[] Routes { get; set; }

        [JsonProperty("Missions")]
        public string Missions { get; set; }

        [JsonProperty("field_image_caption")]
        public string FieldImageCaption { get; set; }

        [JsonProperty("field_thumbnail_image_466x248")]
        public string FieldThumbnailImage466x248 { get; set; }

        [JsonProperty("field_thumbnail_image_226x170")]
        public string FieldThumbnailImage226x170 { get; set; }

        [JsonProperty("field_promo_leader_sentence")]
        public string FieldPromoLeaderSentence { get; set; }

        [JsonProperty("auto_image_466x248")]
        public string AutoImage466x248 { get; set; }

        [JsonProperty("auto_image_226x170")]
        public string AutoImage226x170 { get; set; }

        [JsonProperty("auto_image_360x225")]
        public string AutoImage360x225 { get; set; }

        [JsonProperty("auto_image_430x323")]
        public string AutoImage430x323 { get; set; }

        [JsonProperty("auto_image_100x75")]
        public string AutoImage100x75 { get; set; }

        [JsonProperty("field_master_image")]
        public string FieldMasterImage { get; set; }

        [JsonProperty("field_master_image_2")]
        public string FieldMasterImage2 { get; set; }

        [JsonProperty("field_topics")]
        public string FieldTopics { get; set; }

        [JsonProperty("field_missions")]
        public string FieldMissions { get; set; }

        [JsonProperty("field_collections")]
        public string FieldCollections { get; set; }
    }
}
