/*
 * Class to allow multiple forms to use the same instance of variables
 */

using System.Reflection;

namespace NasaPicOfDay
{
	public static class GlobalVariables
	{
		static GlobalVariables()
		{
			LoggingEnabled = false;
			CurrentRevisionNumber = 0;
			CurrentBuildNumber = 0;
			CurrentMinorVersion = 0;
			CurrentMajorVersion = 0;
		}

		public static int CurrentMajorVersion { get; set; }
		public static int CurrentMinorVersion { get; set; }
		public static int CurrentBuildNumber { get; set; }
		public static int CurrentRevisionNumber { get; set; }
		public static bool LoggingEnabled { get; set; }
		public static BackgroundImage NasaImage { get; set; }

		public static void GetApplicationVersion()
		{
			var assembly = Assembly.GetExecutingAssembly();
			CurrentMajorVersion = assembly.GetName().Version.Major;
			CurrentMinorVersion = assembly.GetName().Version.Minor;
			CurrentBuildNumber = assembly.GetName().Version.Build;
			CurrentRevisionNumber = assembly.GetName().Version.Revision;
		}
	}
}
