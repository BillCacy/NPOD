/*
 * Class to allow multiple forms to use the same instance of variables
 */
namespace NasaPicOfDay
{
	public static class GlobalVariables
	{
		private static bool _LoggingEnabled = false;
		private static BackgroundImage _NasaImage;

		public static BackgroundImage NasaImage
		{
			get { return _NasaImage; }
			set { _NasaImage = value; }
		}

		public static bool LoggingEnabled { get { return _LoggingEnabled; } set { _LoggingEnabled = value; } }
	}
}
