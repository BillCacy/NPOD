using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace NasaPicOfDay
{


	class SetWallpaper
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
		private static UInt32 SPI_SETDESKWALLPAPER = 20;
		private static UInt32 SPIF_UPDATEINIFILE = 0x1;



		static void Main(string[] args)
		{
			BackgroundChanger changer = new BackgroundChanger();
			BackgroundImage backgroundImage = changer.GetTodaysImage();
			changer.SetDesktopBackground(backgroundImage.DownloadedPath);

		}

	}




}