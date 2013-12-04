using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Updater
{
	public class NpodUpdater
	{
		private const string GitPath = "https://raw.github.com/BillCacy/NPOD/master/windows/bin/Release/";
		private readonly string[] _fileList = { "NasaPicOfDay.exe", "NasaPicOfDay.exe.config", "Newtonsoft.Json.dll", "world.ico" };

		public bool UpdateNpod()
		{
			try
			{
				//Get the executing directory
				var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
				UpdateLogger.WriteInformation(appDirectory, "Starting update process");

				//stop any running NPOD service
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
				using (var webClient = new WebClient())
				{
					foreach (var file in _fileList)
					{
						webClient.DownloadFile(new Uri(string.Format("{0}\\{1}", GitPath, file)), string.Format("{0}\\{1}", appDirectory, file));
					}
				}

				//Restart NPOD
				Process.Start("NasaPicOfDay.exe");

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
