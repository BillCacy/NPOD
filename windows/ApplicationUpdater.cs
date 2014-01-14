/* 
 * Handles the checking for application updates from GitHub 
 */


using System;
using System.Xml;

namespace NasaPicOfDay
{
	public class ApplicationUpdater
	{
		private const string ServerConfigFilePath = "https://raw.github.com/BillCacy/NPOD/master/windows/bin/Release/NasaPicOfDay.exe.config";

		private static int _serverMajorVersion;
		private static int _serverMinorVersion;
		private static int _serverBuildNumber;

		public static bool UpdateIsAvailable()
		{
			//Read in the XML file
			var configDocument = new XmlDocument();
			try
			{
				configDocument.Load(ServerConfigFilePath);
			}
			catch (Exception ex)
			{
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("An error occurred while checking for the update.");
				ExceptionManager.WriteException(ex);
				return false;
			}


			//Extract configuration/applicationSettings/NasaPicOfDay.Properties.Settings/setting
			var settingNode =
				configDocument.SelectSingleNode(
					"/configuration/applicationSettings/NasaPicOfDay.Properties.Settings/setting[@name='CurrentVersion']/value");

			if (settingNode == null)
			{
				return false;
			}

			var versionStrings = settingNode.InnerText.Split('.');

			Int32.TryParse(versionStrings[0], out _serverMajorVersion);
			Int32.TryParse(versionStrings[1], out _serverMinorVersion);
			Int32.TryParse(versionStrings[2], out _serverBuildNumber);

			//Compare against global variables for version
			if (GlobalVariables.CurrentMajorVersion < _serverMajorVersion)
			{
				//Update is required
				return true;
			}
			else
			{
				//Need to check other fields
				if (GlobalVariables.CurrentMinorVersion < _serverMinorVersion)
				{
					//Update is required
					return true;
				}
				else
				{
					//Need to check the other fields
					if (GlobalVariables.CurrentBuildNumber < _serverBuildNumber)
					{
						//Update is required
						return true;
					}
				}
			}

			return false;
		}
	}
}
