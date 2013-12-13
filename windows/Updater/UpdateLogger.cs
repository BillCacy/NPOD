using System;
using System.IO;

/* Class that handles the logging for the NPOD Updater application */
namespace Updater
{
	public static class UpdateLogger
	{
		/// <summary>
		/// Writes the exception information to the file system.
		/// </summary>
		/// <param name="ex">Exception to write to the file system</param>
		public static void WriteError(Exception ex)
		{
			var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
			WriteError(appDirectory, ex);
		}

		/// <summary>
		/// Writes the exception information to the specified location in the file system.
		/// </summary>
		/// <param name="filePath">Path on the system to write the file to</param>
		/// <param name="ex">Exception to write to the file</param>
		public static void WriteError(string filePath, Exception ex)
		{
			var fullFilePath = string.Format("{0}\\UpdateError.log", filePath);
			var message = string.Format("{0:yyyyMMdd HH:mm:ss}\tMessage: {1}", DateTime.Now, ex.Message);

			using (var writer = new StreamWriter(fullFilePath, true))
			{
				writer.WriteLine(message);
				writer.Flush();
			}
		}

		/// <summary>
		/// Writes the information to the specified location in the file system.
		/// </summary>
		/// <param name="filePath">Path on the system to write the file to</param>
		/// <param name="message">Information to write to the file</param>
		public static void WriteInformation(string filePath, string message)
		{
			var fullFilePath = string.Format("{0}\\UpdateInfo.log", filePath);
			using (var writer = new StreamWriter(fullFilePath, true))
			{
				writer.WriteLine("{0:yyyyMMdd HH:mm:ss}\tMessage: {1}", DateTime.Now, message);
				writer.Flush();
			}
		}
	}
}
