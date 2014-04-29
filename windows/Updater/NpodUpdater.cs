using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Updater
{
	public class NpodUpdater
	{
		private const string InstallerPath = "https://raw.github.com/BillCacy/NPOD/master/windows/Setup/NPODSetup/Release/setup.exe";

		public bool UpdateNpod()
		{
			try
			{
				//Get the executing directory
				var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
				UpdateLogger.WriteInformation(appDirectory, "Starting update process");

				//stop any running NPOD service
				UpdateLogger.WriteInformation(appDirectory, "Stopping NPOD process");
				foreach (var proc in Process.GetProcessesByName("NasaPicOfDay"))
				{
					try
					{
						proc.Kill();
						proc.WaitForExit();
					}
					catch (Win32Exception win32Ex)
					{
						UpdateLogger.WriteError(appDirectory, win32Ex);
						return false;
					}
					catch (InvalidOperationException invalidOperationEx)
					{
						UpdateLogger.WriteError(appDirectory, invalidOperationEx);
						return false;
					}
				}

				//Pull files down
				UpdateLogger.WriteInformation(appDirectory, "Retrieving updated NPOD files");
				using (var webClient = new WebClient())
				{
						webClient.DownloadFile(new Uri(InstallerPath), string.Format("{0}\\setup.exe", appDirectory));
				}
				UpdateLogger.WriteInformation(appDirectory, "Successfully retrieved files");

				//Run the installer
				//Need the 'runas' verb to allow admin privileges for update
				var startInfo = new ProcessStartInfo("setup.exe") { Verb = "runas" };
				Process.Start(startInfo);

				return true;
			}
			catch (Exception ex)
			{
				UpdateLogger.WriteError(ex);
				return false;
			}
		}
	}
}
