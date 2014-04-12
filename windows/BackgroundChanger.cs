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
		private const string NasaLatestImagesUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=1&offset={0}&routes=1446";
		private const string NasaImageBaseUrl = "http://www.nasa.gov";
		private string _currentScreenResolution;
		private const int DefaultimageOffset = 0;

		/// <summary>
		/// Retrieves the user's current screen resolution to try and pick the image that will fit.
		/// </summary>
		private void GetCurrentScreenResolution()
		{
			try
			{
				int width = Screen.PrimaryScreen.Bounds.Width;
				int height = Screen.PrimaryScreen.Bounds.Height;

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
			return GetImage(0);
		}
		/// <summary>
		/// Overloaded GetImage that allows the retrieval of previous images based on their position in the XML document
		/// provided from the nasa.gov website.
		/// </summary>
		/// <param name="selectedOffset">Position of the image within the node list of images, 1 being the very first (newest)</param>
		public BackgroundImage GetImage(int? selectedOffset)
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
				var nasaImages = JsonHelper.DownloadSerializedJsonData(string.Format(NasaLatestImagesUrl, selectedOffset));
				if (nasaImages == null || nasaImages.Nodes.Length == 0)
					throw new Exception("Unable to retrieve image data from JSON request");

				//Get the image node
				Node2 imageNode = nasaImages.Nodes[0].node;

				string currentImageFullUrl;
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Selecting image based on current screen resolution");
				switch (_currentScreenResolution)
				{
					case "346x260":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image346x260);
						break;
					case "466x248":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image466x248);
						break;
					case "226x170":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image226x170);
						break;
					case "360x225":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image360x225);
						break;
					case "430x323":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image430x323);
						break;
					case "100x75":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image100x75);
						break;
					case "1600x1200":
					case "1600x900":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image1600x1200);
						break;
					case "800x600":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image800x600);
						break;
					case "1024x768":
					case "1366x768":
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.Image1024x768);
						break;
					default:
						currentImageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageNode.MasterImage);
						break;
				}

				var localImageFolderPath = string.Format("{0}\\NASA\\PicOfTheDay", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

				if (!Directory.Exists(localImageFolderPath))
				{
					Directory.CreateDirectory(localImageFolderPath);
				}

				string imageExtension = GetImageExtensionFromUrl(currentImageFullUrl);
				if (string.IsNullOrEmpty(imageExtension))
					throw new Exception("Invalid image extension from URL");

				//Retrieving the file name of the image
				int slashIdx = currentImageFullUrl.LastIndexOf("/");
				int extensionIdx = currentImageFullUrl.LastIndexOf(imageExtension);
				/* Retrieving the image name was tricky due to resolution matching. All images, besides the master image
				 * had ?<random letters> after the file extension. So i had to figure out what the extension was,
				 * substring from the last '/' up to the end of the extension.
				 * There might be better ways to do this, but i've been staring at it too long tonight.
				 */
				string imageName = currentImageFullUrl.Substring(slashIdx + 1, ((extensionIdx - 1) + imageExtension.Length) - slashIdx);

				//Setting the full local image path to save the file
				var fullImagePath = string.Format("{0}\\{1}", localImageFolderPath, imageName);

				//Download the current image
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Preparing to download image: {0}", currentImageFullUrl));
				if (!DownloadHelper.DownloadImage(fullImagePath, currentImageFullUrl))
					throw new Exception("Error downloading current image.");

				//Create BackgroundImage object 
				var backgroundImage = new BackgroundImage
				{
					ImageTitle = imageNode.Title,
					ImageUrl = currentImageFullUrl,
					ImageDate = Convert.ToDateTime(imageNode.PromotionalDate),
					ImageDescription = StripHtml(imageNode.TrimmedImageCaption),
					DownloadedPath = fullImagePath
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
			Regex htmlRegex = new Regex(@"<.*?>");
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
				if(GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("An error occurred updating the desktop background.");
				ExceptionManager.WriteException(ex);
			}
		}

		private string GetImageExtensionFromUrl(string url)
		{
			if (url.Contains(".jpg")) return ".jpg";
			if (url.Contains(".jpeg")) return ".jpeg";
			if (url.Contains(".png")) return ".png";
			if (url.Contains(".gif")) return ".gif";
			if (url.Contains(".bmp")) return ".bmp";
			if (url.Contains(".tiff")) return ".tiff";
			if (url.Contains(".tif")) return ".tif";

			return string.Empty;
		}
	}
}
