
using System.Net;

namespace NasaPicOfDay
{
	public static class NetworkHelper
	{
		public static bool InternetAccessIsAvailable()
		{
			const string url = "http://www.nasa.gov";
			try
			{
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Building internet connectivity request.");
				WebRequest myRequest = WebRequest.Create(url);
				//a 10 second timeout to limit the request attempt
				myRequest.Timeout = 10000;
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Starting request.");
				using (WebResponse myResponse = myRequest.GetResponse())
				{
					if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(myResponse.ToString());
					myResponse.Close();
					if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Request completed.");
					return true;
				}
			}
			catch (WebException wex)
			{
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Error occurred checking internet connection:\t{0}", wex.Message));
				ExceptionManager.WriteException(wex);
				return false;
			}
		}
	}
}
