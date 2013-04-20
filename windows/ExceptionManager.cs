using System;
using System.IO;
using System.Text;

namespace NasaPicOfDay
{
	/// <summary>
	/// Static class that writes Exceptions to the user's current Local Application Data folder
	/// under a folder called 'NPOD'
	/// </summary>
	public static class ExceptionManager
	{
		/// <summary>
		/// Write an exceptiong to the local app data folder for the current user
		/// </summary>
		/// <param name="ex">Exception object containing information about the error.</param>
		public static void WriteException(Exception ex)
		{
			//Get the user's application directory
			string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string fullLogFileFolderPath = logFilePath + "\\NPOD";

			if (!Directory.Exists(fullLogFileFolderPath))
			{
				Directory.CreateDirectory(fullLogFileFolderPath);
			}

			string errorLogFilePath = string.Format("{0}\\Error.log", fullLogFileFolderPath);

			StringBuilder errorBuilder = new StringBuilder();

			errorBuilder.Append(string.Format("{0:yyyyddMMHHmmss}\t", DateTime.Now));
			errorBuilder.Append(string.Format("{0}\t", ex.Message));
			errorBuilder.Append(string.Format("{0}\t", ex.StackTrace));

			WriteData(errorLogFilePath, errorBuilder.ToString());

			errorBuilder.Remove(0, errorBuilder.Length);
			errorBuilder = null;
			errorLogFilePath = null;
			fullLogFileFolderPath = null;
			logFilePath = null;
		}
		/// <summary>
		/// Handles the actual writing of the data to the file system
		/// </summary>
		/// <param name="fullFilePath">Complete path of the file to write the data into</param>
		/// <param name="dataToWrite">String data that will be appended to the file</param>
		private static void WriteData(string fullFilePath, string dataToWrite)
		{
			StreamWriter writer = null;
			try
			{
				writer = new StreamWriter(fullFilePath, true);
				writer.WriteLine(dataToWrite);
				writer.Close();
				writer = null;
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
					writer = null;
				}
			}
		}
	}
}
