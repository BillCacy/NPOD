using System;
using System.Net;

namespace NasaPicOfDay
{
   public static class DownloadHelper
   {
      public static bool DownloadImage(string targetDirectory, string sourceUrl)
      {
         if (targetDirectory == null) throw new ArgumentNullException("targetDirectory");
         if (sourceUrl == null) throw new ArgumentNullException("sourceUrl");
         try
         {
            using (var client = new WebClient())
            {
               client.DownloadFile(sourceUrl, targetDirectory);
            }
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Image download completed");

            return true;
         }
         catch (Exception ex)
         {
            ExceptionManager.WriteException(ex);
            return false;
         }
      }
   }
}
