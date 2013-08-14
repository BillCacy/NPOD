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
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;
        private string _nasaLatestImagesUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=1&offset={0}";
        private string _nasaImageBaseUrl = "http://www.nasa.gov";
        private string _currentScreenResolution;
        private int _defaultimageOffset = 0;

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
        /// <param name="selectedImagePosition">Position of the image within the node list of images, 1 being the very first (newest)</param>
        public BackgroundImage GetImage(int? selectedOffset)
        {
            try
            {
                GetCurrentScreenResolution();

                if (selectedOffset == null)
                    selectedOffset = _defaultimageOffset;

                //Get the JSON string data
                NasaImages nasaImages = JsonHelper.DownloadSerializedJsonData(string.Format(_nasaLatestImagesUrl, selectedOffset));
                if (nasaImages == null || nasaImages.Nodes.Length == 0)
                    throw new Exception("Unable to retrieve image data from JSON request");

                //Get the image node
                Node2 imageNode = nasaImages.Nodes[0].node;

                string currentImageFullUrl;
                switch (_currentScreenResolution)
                {
                    case "346x260":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image346x260);
                        break;
                    case "466x248":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image466x248);
                        break;
                    case "226x170":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image226x170);
                        break;
                    case "360x225":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image360x225);
                        break;
                    case "430x323":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image430x323);
                        break;
                    case "100x75":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image100x75);
                        break;
                    case "1600x1200":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image1600x1200);
                        break;
                    case "800x600":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image800x600);
                        break;
                    case "1024x768":
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.Image1024x768);
                        break;
                    default:
                        currentImageFullUrl = string.Format("{0}{1}", _nasaImageBaseUrl, imageNode.MasterImage);
                        break;
                }

                string localImageFolderPath = string.Format("{0}\\NASA\\PicOfTheDay", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

                if (!Directory.Exists(localImageFolderPath))
                {
                    Directory.CreateDirectory(localImageFolderPath);
                }

                int slashIdx = currentImageFullUrl.LastIndexOf("/");
                int extensionIdx = currentImageFullUrl.LastIndexOf(".jpg");
                string imageName = currentImageFullUrl.Substring(slashIdx + 1, (extensionIdx - slashIdx) + 3);

                string fullImagePath = string.Format("{0}\\{1}", localImageFolderPath, imageName);

                //Download the current image
                if (!DownloadHelper.DownloadImage(fullImagePath, currentImageFullUrl))
                    throw new Exception("Error downloading current image.");

                //Create BackgroundImage object 
                BackgroundImage backgroundImage = new BackgroundImage();
                backgroundImage.ImageTitle = imageNode.Title;
                backgroundImage.ImageUrl = currentImageFullUrl;
                backgroundImage.ImageDate = Convert.ToDateTime(imageNode.PromotionalDate);
                backgroundImage.ImageDescription = StripHtml(imageNode.TrimmedImageCaption);
                backgroundImage.DownloadedPath = fullImagePath;

                imageNode = null;
                currentImageFullUrl = null;
                localImageFolderPath = null;
                fullImagePath = null;

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
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fileName, SPIF_UPDATEINIFILE);
        }
    }
}
