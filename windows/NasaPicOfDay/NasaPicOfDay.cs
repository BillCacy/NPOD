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
private static string imageFileName = "C:\\nasaimages\\NASA_ImageOfDay.bmp";


        static void Main(string[] args)
        {
            setImage(imageFileName);

        }
      public static void setImage( string filename )
        {
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
        } 
    }




}