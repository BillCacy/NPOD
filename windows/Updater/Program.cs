using System;

namespace Updater
{
	class Program
	{
		static void Main()
		{
			var updater = new NpodUpdater();
			Console.WriteLine(updater.UpdateNpod() ? "Update Completed" : "Update Failed");
		}
	}
}
