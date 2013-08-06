using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace NasaPicOfDay
{
    public partial class SettingsForm : Form
    {
        private int _CurrentImagePosition = 0;
        private int _TotalNumberOfImages = 0;
        private string _NasaLatestImagesUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=50&offset=0&Routes=1446";
        private string _NasaImageBaseUrl = "http://www.nasa.gov";
        private NasaImages _Images;

        public SettingsForm()
        {
            InitializeComponent();
            SetButtonEnabled();
        }

        private void btnCloseSettings_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBackImage_Click(object sender, EventArgs e)
        {
            if (_CurrentImagePosition < _TotalNumberOfImages)
                _CurrentImagePosition++;

            SetButtonEnabled();
            GetImageThumbnail(_CurrentImagePosition);
        }

        private void btnForwardImage_Click(object sender, EventArgs e)
        {
            if (_CurrentImagePosition > 0)
                _CurrentImagePosition--;

            SetButtonEnabled();
            GetImageThumbnail(_CurrentImagePosition);
        }

        private void SetButtonEnabled()
        {
            if (_CurrentImagePosition == _TotalNumberOfImages)
            {
                btnBackImage.Enabled = false;
            }
            else
            {
                btnBackImage.Enabled = true;
            }

            if (_CurrentImagePosition == 0)
            {
                btnForwardImage.Enabled = false;
            }
            else
            {
                btnForwardImage.Enabled = true;
            }
        }

        private void btnCurrentImage_Click(object sender, EventArgs e)
        {
            _CurrentImagePosition = 0;
            SetButtonEnabled();
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

            if (_Images == null)
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
                _Images = JsonHelper.DownloadSerializedJsonData(_NasaLatestImagesUrl);
                if (_Images == null)
                    throw new Exception("Unable to retrieve the image data");

                /* NOTE
                 * Using Length for now because the json deserialization process only creates 
                 * 50 nodes, but the actual Count of nodes is alot higher
                 */
                _TotalNumberOfImages = _Images.Nodes.Length - 1;
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
                if (_Images == null)
                    LoadNasaImageList();

                if (imagePosition < 0)
                    throw new Exception("Invalid image position.");

                Node2 imageNode = _Images.Nodes[(int)imagePosition].node;

                string imageUrl = imageNode.AutoImage466x248;
                string imageFullUrl = string.Format("{0}{1}", _NasaImageBaseUrl, imageUrl);

                string thumbNailImagePath = string.Format("{0}\\NASA\\PicOfTheDay\\Thumbnails", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

                if (!Directory.Exists(thumbNailImagePath))
                {
                    Directory.CreateDirectory(thumbNailImagePath);
                }

                int slashIdx = imageUrl.LastIndexOf("/");
                int extensionIdx = imageUrl.LastIndexOf(".jpg");
                string imageName = imageUrl.Substring(slashIdx + 1, (extensionIdx - slashIdx) + 3);

                string fullImagePath = string.Format("{0}\\thumb{1}", thumbNailImagePath, imageName);

                //If the thumbnail has been previously downloaded, no need to download it again, just use the current one
                if (!File.Exists(fullImagePath))
                {
                    if (!DownloadHelper.DownloadImage(fullImagePath, imageFullUrl))
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
    }
}
