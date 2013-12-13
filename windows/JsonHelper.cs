using System;
using System.Net;
using Newtonsoft.Json;

namespace NasaPicOfDay
{
	public static class JsonHelper
	{
		public static NasaImages DownloadSerializedJsonData(string nasaUrl)
		{
			try
			{
				using (var client = new WebClient())
				{
					var jsonData = client.DownloadString(nasaUrl);

					if (!string.IsNullOrEmpty(jsonData))
					{
						var images = JsonConvert.DeserializeObject<NasaImages>(jsonData);
						return images;
					}


					throw new Exception("Unable to retrieve JSON data");
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
				return null;
			}
		}
	}
}
