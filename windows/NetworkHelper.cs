
namespace NasaPicOfDay
{
	public static class NetworkHelper
	{
		public static bool InternetAccessIsAvailable()
		{
			string url = "http://www.nasa.gov";
			try
			{
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Building internet connectivity request.");
				System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
				//a 10 second timeout to limit the request attempt
				myRequest.Timeout = 10000;
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Starting request.");
				using (System.Net.WebResponse myResponse = myRequest.GetResponse())
				{
					myResponse.Close();
					if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Request completed.");
					return true;
				}
			}
			catch (System.Net.WebException wex)
			{
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Error occurred checking internet connection:\t{0}", wex.Message));
				ExceptionManager.WriteException(wex);
				return false;
			}
		}
	}
}
