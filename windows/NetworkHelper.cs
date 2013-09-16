
namespace NasaPicOfDay
{
    public static class NetworkHelper
    {
        public static bool InternetAccessIsAvailable()
        {
            string url = "http://www.nasa.gov";
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                //a 10 second timeout to limit the request attempt
                myRequest.Timeout = 10000;
                using (System.Net.WebResponse myResponse = myRequest.GetResponse())
                {
                    myResponse.Close();
                    return true;
                }
            }
            catch (System.Net.WebException)
            {
                return false;
            }
        }
    }
}
