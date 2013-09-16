
namespace NasaPicOfDay
{
    public static class NetworkHelper
    {
        public static bool InternetAccessIsAvailable()
        {
            string url = "http://www.google.com";
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                //a 10 second timeout to limit the request attempt
                myRequest.Timeout = 10000;
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }
    }
}
