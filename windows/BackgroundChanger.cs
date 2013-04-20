using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Net;
using System.Runtime.InteropServices;

namespace NasaPicOfDay
{
	/// <summary>
	/// Retrieves the xml file data to grab the current image of the day
	/// </summary>
	public class BackgroundChanger
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]

		private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
		private static UInt32 SPI_SETDESKWALLPAPER = 20;
		private static UInt32 SPIF_UPDATEINIFILE = 0x1;

		private string _nasaLatestImagesXmlUrl = "http://www.nasa.gov/multimedia/imagegallery/iotdxml.xml";
		private string _nasaImageBaseUrl = "http://www.nasa.gov";
		private string _nasaCurrentImageXPath = "./rss[1]/channel[1]";

		public BackgroundChanger() { }

		/// <summary>
		/// Retrieves a BackgroundImage object containing the information downloaded from NASA
		/// regarding the current photo of the day
		/// </summary>
		/// <returns>Null is returned if an error occurred</returns>
		public BackgroundImage GetTodaysImage()
		{
			try
			{
				XmlDocument nasaImagesXml = new XmlDocument();
				//Retrieve the base XML document
				nasaImagesXml.Load(_nasaLatestImagesXmlUrl);

				if (nasaImagesXml == null)
					throw new Exception("Unable to retrieve current image list information.");

				//Retrieve the information to create the XML document for the current image
				XmlNode currentImageXmlNode = nasaImagesXml.SelectSingleNode("./rss[1]/channel[1]/ig[1]/ap[1]");
				string currentImageXmlUrl = currentImageXmlNode.InnerText;
				string currentImageXmlFullUrl = string.Format("{0}{1}.xml", _nasaImageBaseUrl, currentImageXmlUrl);

				//We are done with the base XML now
				nasaImagesXml = null;
				currentImageXmlUrl = null;

				//Load the XML doc with information for the current image
				XmlDocument currentImageXml = new XmlDocument();
				currentImageXml.Load(currentImageXmlFullUrl);
				if (currentImageXml == null)
					throw new Exception("Unable to retrieve current image information.");

				XmlNode currentImageNode = currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/image[1]/size[type=\"Full_Size\"][1]/href");

				//temporary string to hold the unformatted data
				string temp = currentImageNode.InnerText;

				//pulling the file name from the node text since the url node contains
				//images/content/<image name>
				int indexOfLastSlash = temp.LastIndexOf("/");
				string imageName = temp.Substring(indexOfLastSlash + 1);
				temp = null;

				if (currentImageNode == null)
					throw new Exception("Current image node does not exist.");

				string currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, currentImageNode.InnerText);

				string localImageFilePath = string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), imageName);


				//We are done with currentImageNode now
				currentImageNode = null;
				imageName = null;

				//Download the current image
				if (!DownloadImage(localImageFilePath, currentImageFullUrl))
					throw new Exception("Error downloading current image.");

				//Create BackgroundImage object from XML document
				BackgroundImage backgroundImage = new BackgroundImage();
				backgroundImage.AssetId = currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/assetId").InnerText;
				backgroundImage.ImageTitle = currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/title").InnerText;
				backgroundImage.ImageUrl = currentImageFullUrl;
				backgroundImage.ImageDate = Convert.ToDateTime(currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/pubDate").InnerText);
				backgroundImage.ImageDescription = StripHtml(currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/description").InnerText);
				backgroundImage.DownloadedPath = localImageFilePath;

				currentImageXml = null;
				currentImageFullUrl = null;
				localImageFilePath = null;

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
		/// Handles the actual image download and saving to the user's file system (My Pictures)
		/// </summary>
		/// <param name="destinationDir">Physical location of the My Pictures directory on the user's system</param>
		/// <param name="imageUrl">The NASA url to retrieve the image from</param>
		/// <returns>True if the download was successful; otherwise false is returned.</returns>
		private bool DownloadImage(string destinationDir, string imageUrl)
		{
			try
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFile(imageUrl, destinationDir);
				}

				return true;
			}
			catch(Exception ex)
			{
				ExceptionManager.WriteException(ex);
				return false;
			}
			
		}

		/// <summary>
		/// Set the desktop to the current picture of the day
		/// <param name="fileName">Full file path to the image to set as the desktop background.</param>
		/// </summary>
		public void SetDesktopBackground(string fileName)
		{
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fileName, SPIF_UPDATEINIFILE);
		}
	}
}
