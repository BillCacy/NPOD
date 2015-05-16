using NasaPicOfDay.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace NasaPicOfDay
{
   public partial class ImagesForm : Form
   {
      private int _totalDocImageCount;
      private int _currentImageNumber;
      private const string NasaLatestImagesUrl = "https://www.nasa.gov/api/1/query/ubernodes.json?unType%5B%5D=image&routes%5B%5D=1446&page={0}&pageSize=24";
      private const string NasaImageBaseUrl = "http://www.nasa.gov";
      private const string NasaApiUrl = "http://www.nasa.gov/api/1/record/node/";
      private NasaImages _images;
      private int _displayImageNumber;
      private int _currentPage = 0;

      public ImagesForm()
      {
         InitializeComponent();
         SetButtonEnabled();
         versionLabel.Text = string.Format("{0}.{1}.{2}", GlobalVariables.CurrentMajorVersion, GlobalVariables.CurrentMinorVersion, GlobalVariables.CurrentBuildNumber);
      }

      private void btnCloseSettings_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void btnBackImage_Click(object sender, EventArgs e)
      {
         if (_displayImageNumber == _totalDocImageCount - 1)
         {
            btnBackImage.Enabled = false;
            return;
         }

         //Current image number out of the total amount of images
         if (_currentImageNumber < _totalDocImageCount - 1)
         {
            _currentImageNumber++;
            _displayImageNumber++;
         }

         if (_currentImageNumber == 23)
         {
            //need to grab the next 24 images
            //so the offset should be set back to 0 to run through the next 24 images
            _currentImageNumber = 0;
            _currentPage++;
            LoadNasaImageList(_currentPage);
         }

         SetButtonEnabled();
         GetImageThumbnail(_currentImageNumber);
         lblCount.Text = string.Format("{0} of {1} images", (_displayImageNumber + 1), _totalDocImageCount);
      }

      private void btnForwardImage_Click(object sender, EventArgs e)
      {
         if (_displayImageNumber == 0)
         {
            btnForwardImage.Enabled = false;
            return;
         }

         if (_currentImageNumber > 0)
         {
            _currentImageNumber--;
            _displayImageNumber--;
         }

         if (_currentImageNumber == 23)
         {
            //need to grab the next 24 images
            _displayImageNumber--;
            _currentPage = _currentPage != 0 ? _currentPage-- : 0;
            LoadNasaImageList(_currentPage);
            _currentImageNumber = 0;
         }

         SetButtonEnabled();
         GetImageThumbnail(_currentImageNumber);
         lblCount.Text = string.Format("{0} of {1} images", (_displayImageNumber + 1), _totalDocImageCount);
      }

      private void SetButtonEnabled()
      {
         btnBackImage.Enabled = _currentImageNumber != _totalDocImageCount;

         btnForwardImage.Enabled = _currentImageNumber != 0;
      }

      private void btnCurrentImage_Click(object sender, EventArgs e)
      {
         _currentImageNumber = 0;
         _displayImageNumber = 0;
         _currentPage = 0;
         LoadNasaImageList(0);
         SetButtonEnabled();
         GetImageThumbnail(_currentImageNumber);
         lblCount.Text = string.Format("{0} of {1} images", (_displayImageNumber + 1), _totalDocImageCount);
      }

      private void btnSetImage_Click(object sender, EventArgs e)
      {
         var bgChanger = new BackgroundChanger();
         GlobalVariables.NasaImage = bgChanger.GetImage(_currentImageNumber, _currentPage);
         bgChanger.SetDesktopBackground(GlobalVariables.NasaImage.DownloadedPath);
         Close();
      }

      private void SettingsForm_Load(object sender, EventArgs e)
      {
         LoadNasaImageList(0);

         if (_images == null)
         {
            btnBackImage.Enabled = false;
            btnForwardImage.Enabled = false;
            btnSetImage.Enabled = false;
            btnCurrentImage.Enabled = false;
         }
         else
         {
            btnBackImage.Enabled = true;
            btnForwardImage.Enabled = false;
            btnSetImage.Enabled = true;
            btnCurrentImage.Enabled = true;
         }

         if (!GetImageThumbnail(_currentImageNumber))
            MessageBox.Show(Resources.ImageRetrievalError, Resources.Oops, MessageBoxButtons.OK);

         lblCount.Text = string.Format("{0} of {1} images", (_displayImageNumber + 1), _totalDocImageCount);
      }

      private void LoadNasaImageList(int pageNumber)
      {
         try
         {
            if (pageNumber < 0)
            {
               pageNumber = 0;
               _currentImageNumber = 0;
               _displayImageNumber = 0;
            }

            _images = JsonHelper.DownloadSerializedJsonData(string.Format(NasaLatestImagesUrl, pageNumber));
            if (_images == null)
               throw new Exception("Unable to retrieve the image data");

            _totalDocImageCount = Convert.ToInt32(_images.Meta.TotalRows);
         }
         catch (Exception ex)
         {
            ExceptionManager.WriteException(ex);
            throw;
         }
      }

      private bool GetImageThumbnail(int imagePosition)
      {
         try
         {
            if (_images == null)
               LoadNasaImageList(_currentPage);

            if (imagePosition < 0)
               throw new Exception("Invalid image position.");
            if (_images == null || _images.UberNodes.Length < 0)
               throw new Exception("Image collection error. No images retrieved.");

            var imageId = _images.UberNodes[imagePosition].UberNodeId;

            var imageDatalUrl = string.Format("{0}{1}.json", NasaApiUrl, imageId);

            var currentImage = JsonHelper.DownloadImageData(imageDatalUrl);
            if (currentImage == null)
               throw new Exception(string.Format("Error retrieving image: {0}", imageDatalUrl));

            var fileName = currentImage.ImageData[0].FileName;

            var localFileSystemThumbNailDirectory = string.Format("{0}\\NASA\\PicOfTheDay\\Thumbnails", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

            if (!Directory.Exists(localFileSystemThumbNailDirectory))
            {
               Directory.CreateDirectory(localFileSystemThumbNailDirectory);
            }

            var localFileSystemImagePath = string.Format("{0}\\{1}", localFileSystemThumbNailDirectory, fileName);

            var thumbnailUrl = string.Format("{0}{1}", NasaImageBaseUrl, currentImage.ImageData[0].LrThumbnail);

            //If the thumbnail has been previously downloaded, no need to download it again, just use the current one
            if (!File.Exists(localFileSystemImagePath))
            {
               if (!DownloadHelper.DownloadImage(localFileSystemImagePath, thumbnailUrl))
                  throw new Exception("Error downloading image.");
            }

            picBoxCurrentImg.ImageLocation = localFileSystemImagePath;
            picBoxCurrentImg.SizeMode = PictureBoxSizeMode.Zoom;

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
