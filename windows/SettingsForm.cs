using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;

namespace NasaPicOfDay
{
    public partial class SettingsForm : Form
    {
        private int _CurrentImagePosition = 1;
        private int _TotalNumberOfImages = 0;
        private string _NasaLatestImagesXmlUrl = "http://www.nasa.gov/multimedia/imagegallery/iotdxml.xml";
        private string _NasaImageBaseUrl = "http://www.nasa.gov";
        private XmlDocument _NasaImageListXml = null;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnCloseSettings_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBackImage_Click(object sender, EventArgs e)
        {
            if(_CurrentImagePosition < _TotalNumberOfImages)
                _CurrentImagePosition++;

            GetImageThumbnail(_CurrentImagePosition);
        }

        private void btnForwardImage_Click(object sender, EventArgs e)
        {
            if(_CurrentImagePosition > 1)
                _CurrentImagePosition--;

            GetImageThumbnail(_CurrentImagePosition);
        }

        private void btnCurrentImage_Click(object sender, EventArgs e)
        {
            _CurrentImagePosition = 1;
            GetImageThumbnail(_CurrentImagePosition);
        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            BackgroundChanger bgChanger = new BackgroundChanger();
            GlobalVariables.NasaImage = bgChanger.GetImage(_CurrentImagePosition);
            bgChanger.SetDesktopBackground(GlobalVariables.NasaImage.DownloadedPath);
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadNasaImageList();

            if (_NasaImageListXml == null)
            {
                btnBackImage.Enabled = false;
                btnForwardImage.Enabled = false;
                btnSetImage.Enabled = false;
                btnCurrentImage.Enabled = false;
            }
            else
            {
                btnBackImage.Enabled = true;
                btnForwardImage.Enabled = true;
                btnSetImage.Enabled = true;
                btnCurrentImage.Enabled = true;
            }

            if (!GetImageThumbnail(_CurrentImagePosition))
                MessageBox.Show("An error occured retrieving the image.", "Oops!", MessageBoxButtons.OK);
        }

        private void LoadNasaImageList()
        {
            try
            {
                _NasaImageListXml = new XmlDocument();
                _NasaImageListXml.Load(_NasaLatestImagesXmlUrl);

                XmlNode imageCountNode = _NasaImageListXml.SelectSingleNode("./rss[1]/channel[1]/totalImages");
                int imageCount = Convert.ToInt32(imageCountNode.InnerText);
                //For some reason the count in the file is 2 larger than the actual number of photos
                //maybe counting the rss and channel nodes?
                _TotalNumberOfImages = imageCount - 2;

                if (_NasaImageListXml == null)
                    throw new Exception("Unable to retrieve current image list information.");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteException(ex);
                throw ex;
            }
        }

        private bool GetImageThumbnail(int imagePosition)
        {
            try
            {
                if (_NasaImageListXml == null)
                    LoadNasaImageList();

                if (imagePosition < 1)
                    throw new Exception("Invalid image position.");

                XmlNode selectedImageNode = _NasaImageListXml.SelectSingleNode("./rss[1]/channel[1]/ig[" + imagePosition + "]/tn");
                string imageUrl = selectedImageNode.InnerText;
                string imageFullUrl = string.Format("{0}{1}", _NasaImageBaseUrl, imageUrl);

                string thumbNailImagePath = string.Format("{0}\\NASA\\PicOfTheDay\\Thumbnails", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

                if (!Directory.Exists(thumbNailImagePath))
                {
                    Directory.CreateDirectory(thumbNailImagePath);
                }

                int indexOfLastSlash = imageUrl.LastIndexOf("/");
                string imageName = imageUrl.Substring(indexOfLastSlash + 1);

                string fullImagePath = string.Format("{0}\\{1}", thumbNailImagePath, imageName);

                //If the thumbnail has been previously downloaded, no need to download it again, just use the current one
                if (!File.Exists(fullImagePath))
                {
                    if (!DownloadImage(fullImagePath, imageFullUrl))
                        throw new Exception("Error downloading image.");
                }

                picBoxCurrentImg.ImageLocation = fullImagePath;
                picBoxCurrentImg.SizeMode = PictureBoxSizeMode.StretchImage;

                return true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteException(ex);
                return false;
            }
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
    }
}
