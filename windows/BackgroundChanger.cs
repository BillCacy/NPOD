using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NasaPicOfDay
{
   /// <summary>
   /// Retrieves the xml file data to grab the current image of the day
   /// </summary>
   public class BackgroundChanger
   {
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

      private const UInt32 SpiSetdeskwallpaper = 20;
      private const UInt32 SpifUpdateinifile = 0x1;
      private const string NasaLatestImagesUrl = "https://www.nasa.gov/api/1/query/ubernodes.json?unType%5B%5D=image&routes%5B%5D=1446&page={0}&pageSize=24";
      private const string NasaImageBaseUrl = "http://www.nasa.gov";
      private const string NasaApiUrl = "http://www.nasa.gov/api/1/record/node/";
      private string _currentScreenResolution;
      private const int DefaultimageOffset = 0;

      /// <summary>
      /// Retrieves the user's current screen resolution to try and pick the image that will fit.
      /// </summary>
      private void GetCurrentScreenResolution()
      {
         try
         {
            var width = Screen.PrimaryScreen.Bounds.Width;
            var height = Screen.PrimaryScreen.Bounds.Height;

            _currentScreenResolution = string.Format("{0}x{1}", width, height);
         }
         catch
         {
            //default in the event of an error
            _currentScreenResolution = "1600x1200";
         }
      }
      /// <summary>
      /// Retrieves a BackgroundImage object containing the information downloaded from NASA
      /// regarding the current photo of the day
      /// </summary>
      /// <returns>Null is returned if an error occurred</returns>
      public BackgroundImage GetImage()
      {
         return GetImage(0, 0);
      }
      /// <summary>
      /// Overloaded GetImage that allows the retrieval of previous images based on their position in the XML document
      /// provided from the nasa.gov website.
      /// </summary>
      /// <param name="selectedOffset">Position of the image within the node list of images, 1 being the very first (newest)</param>
      /// <param name="selectedPage">Page number to retrieve the page that contains the selected image.</param>
      public BackgroundImage GetImage(int? selectedOffset, int? selectedPage)
      {
         try
         {
            if (!NetworkHelper.InternetAccessIsAvailable())
               throw new Exception("Attempted to retrieve image, but internet connection was not available.");

            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Retrieving system screen resolution");
            GetCurrentScreenResolution();
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("System screen resolution is: {0}", _currentScreenResolution));

            if (selectedOffset == null)
               selectedOffset = DefaultimageOffset;

            //Get the JSON string data
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Preparing to download image information");
            var nasaImages = JsonHelper.DownloadSerializedJsonData(string.Format(NasaLatestImagesUrl, selectedPage));
            if (nasaImages == null || nasaImages.UberNodes.Length == 0)
               throw new Exception("Unable to retrieve image data from JSON request");

            //Get the image node
            var imageId = nasaImages.UberNodes[(int) selectedOffset].UberNodeId;

            var currentImageFullUrl = string.Format("{0}{1}.json", NasaApiUrl, imageId);

            var currentImage = JsonHelper.DownloadImageData(currentImageFullUrl);
            if (currentImage == null)
               throw new Exception(string.Format("Unable to retrieve selected image: {0}", currentImageFullUrl));

            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Selecting image based on current screen resolution");
            switch (_currentScreenResolution)
            {
               case "100x75":
               case "226x170":
                  currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, currentImage.ImageData[0].LrThumbnail);
                  break;
               case "346x260":
               case "360x225":
               case "466x248":
               case "430x323":
                  currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, currentImage.ImageData[0].Crop1X1);
                  break;
               case "800x600":
                  currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, currentImage.ImageData[0].Crop2X1);
                  break;
               default:
                  currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, currentImage.ImageData[0].FullWidthFeature);
                  break;
            }

            var localImageFolderPath = string.Format("{0}\\NASA\\PicOfTheDay", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

            if (!Directory.Exists(localImageFolderPath))
            {
               Directory.CreateDirectory(localImageFolderPath);
            }

            var imageName = currentImage.ImageData[0].FileName;

            //Setting the full local image path to save the file
            var fullImagePath = string.Format("{0}\\{1}", localImageFolderPath, imageName);

            //Download the current image
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Preparing to download image: {0}", currentImageFullUrl));
            if (!DownloadHelper.DownloadImage(fullImagePath, currentImageFullUrl))
               throw new Exception("Error downloading current image.");

            //Create BackgroundImage object 
            var backgroundImage = new BackgroundImage
            {
               ImageUrl = currentImageFullUrl,
               ImageDate = Convert.ToDateTime(currentImage.UberData.PromoDateTime),
               ImageDescription = StripHtml(currentImage.UberData.Body),
               DownloadedPath = fullImagePath,
               ImageTitle =
                  !string.IsNullOrEmpty(currentImage.ImageData[0].Title)
                     ? currentImage.ImageData[0].Title
                     : currentImage.UberData.Title
            };

            return backgroundImage;
         }
         catch (Exception ex)
         {
            ExceptionManager.WriteException(ex);
            return null;
         }
      }
      /// <summary>
      /// Removes HTML markup tags from the passed in string
      /// </summary>
      /// <param name="textToClean">String of text to remove the HTML markup tags from</param>
      private string StripHtml(string textToClean)
      {
         var htmlRegex = new Regex(@"<.*?>");
         return htmlRegex.Replace(textToClean, "");
      }
      /// <summary>
      /// Set the desktop to the current picture of the day
      /// <param name="fileName">Full file path to the image to set as the desktop background.</param>
      /// </summary>
      public void SetDesktopBackground(string fileName)
      {
         try
         {
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Preparing to change desktop background");
            SystemParametersInfo(SpiSetdeskwallpaper, 0, fileName, SpifUpdateinifile);
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Completed desktop background change");
         }
         catch (Exception ex)
         {
            if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("An error occurred updating the desktop background.");
            ExceptionManager.WriteException(ex);
         }
      }
   }
}
