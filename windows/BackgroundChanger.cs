using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

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
        //private string _nasaLatestImagesXmlUrl = "http://www.nasa.gov/multimedia/imagegallery/iotdxml.xml";
        private string _nasaLatestImagesXmlUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=50&offset=0&Routes=1446";
        private string _nasaImageBaseUrl = "http://www.nasa.gov";
        //private string _nasaCurrentImageXPath = "./rss[1]/channel[1]";
        private string _currentScreenResolution;
        private int _defaultImagePositionInDoc = 1;

        public BackgroundChanger() { }

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
                _currentScreenResolution = "Full_Size";
            }
        }
        /// <summary>
        /// Retrieves a BackgroundImage object containing the information downloaded from NASA
        /// regarding the current photo of the day
        /// </summary>
        /// <returns>Null is returned if an error occurred</returns>
        public BackgroundImage GetImage()
        {
            return GetImage(_defaultImagePositionInDoc);
        }
        /// <summary>
        /// Overloaded GetImage that allows the retrieval of previous images based on their position in the XML document
        /// provided from the nasa.gov website.
        /// </summary>
        /// <param name="selectedImagePosition">Position of the image within the node list of images, 1 being the very first (newest)</param>
        public BackgroundImage GetImage(int? selectedImagePosition)
        {
            try
            {
                GetCurrentScreenResolution();

                if (selectedImagePosition == null)
                    selectedImagePosition = _defaultImagePositionInDoc;

                //Get the JSON string data
                NasaImages nasaImages = DownloadSerializedJsonData();
                if (nasaImages == null || nasaImages.Nodes.Length == 0)
                    throw new Exception("Unable to retrieve image data from JSON request");

                #region Old Image Retrieval from XML feed
                ////XmlDocument nasaImagesXml = new XmlDocument();
                //////Retrieve the base XML document
                ////nasaImagesXml.Load(_nasaLatestImagesXmlUrl);

                ////if (nasaImagesXml == null)
                ////    throw new Exception("Unable to retrieve current image list information.");

                //////Retrieve the information to create the XML document for the current image
                ////XmlNode currentImageXmlNode = nasaImagesXml.SelectSingleNode("./rss[1]/channel[1]/ig[" + selectedImagePosition + "]/ap");
                ////string currentImageXmlUrl = currentImageXmlNode.InnerText;
                ////string currentImageXmlFullUrl = string.Format("{0}{1}.xml", _nasaImageBaseUrl, currentImageXmlUrl);

                //////We are done with the base XML now
                ////nasaImagesXml = null;
                ////currentImageXmlUrl = null;

                //////Load the XML doc with information for the current image
                ////XmlDocument currentImageXml = new XmlDocument();
                ////currentImageXml.Load(currentImageXmlFullUrl);
                ////if (currentImageXml == null)
                ////    throw new Exception("Unable to retrieve current image information.");

                ////XmlNode currentImageNode;
                //////First try to grab the image for the current resolution, if unable grab the Full_Size image
                ////currentImageNode = currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/image[1]/size[type=\"" + _currentScreenResolution + "\"][1]/href");
                ////if (currentImageNode == null)
                ////    currentImageNode = currentImageXml.SelectSingleNode(_nasaCurrentImageXPath + "/image[1]/size[type=\"Full_Size\"][1]/href");

                //////temporary string to hold the unformatted data
                ////string temp = currentImageNode.InnerText;

                //////pulling the file name from the node text since the url node contains
                //////images/content/<image name>
                ////int indexOfLastSlash = temp.LastIndexOf("/");
                ////string imageName = temp.Substring(indexOfLastSlash + 1);
                ////temp = null;

                ////if (currentImageNode == null)
                ////    throw new Exception("Current image node does not exist.");
                #endregion

                //Get the first (newest) image node
                Node2 imageNode = nasaImages.Nodes[0].node;

                /*TODO
                 * Try to grab the URL for the current screen resolution
                 */
                string currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.MasterImage);

                string localImageFolderPath = string.Format("{0}\\NASA\\PicOfTheDay", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

                if (!Directory.Exists(localImageFolderPath))
                {
                    Directory.CreateDirectory(localImageFolderPath);
                }

                int slashIdx = currentImageFullUrl.LastIndexOf("/");
                int extensionIdx = currentImageFullUrl.LastIndexOf(".jpg");
                string imageName = currentImageFullUrl.Substring(slashIdx + 1, (extensionIdx - slashIdx) + 3);

                string fullImagePath = string.Format("{0}\\{1}", localImageFolderPath, imageName);

                //////We are done with currentImageNode now
                ////currentImageNode = null;
                ////imageName = null;

                //Download the current image
                if (!DownloadImage(fullImagePath, currentImageFullUrl))
                    throw new Exception("Error downloading current image.");

                //Create BackgroundImage object from XML document
                BackgroundImage backgroundImage = new BackgroundImage();
                backgroundImage.ImageTitle = imageNode.Title;
                backgroundImage.ImageUrl = currentImageFullUrl;
                backgroundImage.ImageDate = Convert.ToDateTime(imageNode.PromotionalDate);
                backgroundImage.ImageDescription = StripHtml(imageNode.TrimmedImageCaption);
                backgroundImage.DownloadedPath = fullImagePath;

                ////currentImageXml = null;
                ////currentImageFullUrl = null;
                ////localImageFolderPath = null;
                ////fullImagePath = null;

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
            catch (Exception ex)
            {
                ExceptionManager.WriteException(ex);
                return false;
            }
        }

        public NasaImages DownloadSerializedJsonData()
        {
            using (WebClient client = new WebClient())
            {
                string jsonData = string.Empty;

                try
                {
                    jsonData = client.DownloadString(_nasaLatestImagesXmlUrl);

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        NasaImages images = JsonConvert.DeserializeObject<NasaImages>(jsonData);
                        return images;
                    }
                    else
                        throw new Exception("Unable to retrieve JSON data");
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteException(ex);
                    return null;
                }
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
