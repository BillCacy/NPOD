using System;

namespace NasaPicOfDay
{
	/// <summary>
	/// Object that contains the basic properties that are returned in the XML data from NASA.
	/// </summary>
	public class BackgroundImage
	{
		public string ImageTitle { get; set; }
		public DateTime ImageDate { get; set; }
		public string ImageDescription { get; set; }
		public string AssetId { get; set; }
		public string ImageUrl { get; set; }
		public string DownloadedPath { get; set; }
	}
}
