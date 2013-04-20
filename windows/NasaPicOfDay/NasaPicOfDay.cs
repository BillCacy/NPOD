using System;
using System.Collections.Generic;
using System.Text;

namespace NasaPicOfDay
{

	class SetWallpaper
	{
		static void Main(string[] args)
		{
			BackgroundChanger changer = new BackgroundChanger();
			BackgroundImage backgroundImage = changer.GetTodaysImage();
			changer.SetDesktopBackground(backgroundImage.DownloadedPath);
		}
	}
}