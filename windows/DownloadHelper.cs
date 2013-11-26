using System;
using System.Net;

namespace NasaPicOfDay
{
	public static class DownloadHelper
	{
		public static bool DownloadImage(string targetDirectory, string sourceUrl)
		{
			try
			{
				using (var client = new WebClient())
				{
					client.DownloadFile(sourceUrl, targetDirectory);
				}

				return true;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
				return false;
			}
		}
	}
}
