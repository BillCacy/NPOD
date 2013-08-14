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
                using (WebClient client = new WebClient())
                {
                    string jsonData = string.Empty;
                    jsonData = client.DownloadString(nasaUrl);

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        NasaImages images = JsonConvert.DeserializeObject<NasaImages>(jsonData);
                        return images;
                    }
                    else
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
