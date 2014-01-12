using System;
using System.IO;
using System.Windows.Forms;
using NasaPicOfDay.Properties;

namespace NasaPicOfDay
{
	public partial class ImagesForm : Form
	{
		private int _totalDocImageCount;
		private int _currentImagePosition;
		private int _totalNumberOfImages;
		private const string NasaLatestImagesUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=50&offset={0}&routes=1446";
		private const string NasaImageBaseUrl = "http://www.nasa.gov";
		private NasaImages _images;
		private int _currentOffset;

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
			if (_currentOffset == _totalDocImageCount - 1)
			{
				btnBackImage.Enabled = false;
				return;
			}

			if (_currentImagePosition < _totalNumberOfImages - 1)
			{
				_currentImagePosition++;
				_currentOffset++;
			}
			else if (_currentImagePosition == _totalNumberOfImages - 1)
			{
				//need to grab the next 50 images
				_currentOffset++;
				LoadNasaImageList(_currentOffset);
				_currentImagePosition = 0;
			}
			else if (_currentOffset == _totalDocImageCount - 1)
			{
				SetButtonEnabled();
				return;
			}

			SetButtonEnabled();
			GetImageThumbnail(_currentImagePosition);
			lblCount.Text = string.Format("{0} of {1} images", (_currentOffset + 1), _totalDocImageCount);

		}

		private void btnForwardImage_Click(object sender, EventArgs e)
		{
			if (_currentOffset == 0)
			{
				btnForwardImage.Enabled = false;
				return;
			}

			if (_currentImagePosition > 0)
			{
				_currentImagePosition--;
				_currentOffset--;
			}
			else if (_currentOffset % 50 == 0)
			{
				//need to grab the next 50 images
				_currentOffset--;
				LoadNasaImageList(_currentOffset - 50);
				_currentImagePosition = 49;
			}

			SetButtonEnabled();
			GetImageThumbnail(_currentImagePosition);
			lblCount.Text = string.Format("{0} of {1} images", (_currentOffset + 1), _totalDocImageCount);
		}

		private void SetButtonEnabled()
		{
			btnBackImage.Enabled = _currentOffset != _totalDocImageCount;

			btnForwardImage.Enabled = _currentOffset != 0;
		}

		private void btnCurrentImage_Click(object sender, EventArgs e)
		{
			_currentImagePosition = 0;
			_currentOffset = 0;
			LoadNasaImageList(0);
			SetButtonEnabled();
			GetImageThumbnail(_currentImagePosition);
			lblCount.Text = string.Format("{0} of {1} images", (_currentOffset + 1), _totalDocImageCount);
		}

		private void btnSetImage_Click(object sender, EventArgs e)
		{
			var bgChanger = new BackgroundChanger();
			GlobalVariables.NasaImage = bgChanger.GetImage(_currentOffset);
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
				btnForwardImage.Enabled = true;
				btnSetImage.Enabled = true;
				btnCurrentImage.Enabled = true;
			}

			if (!GetImageThumbnail(_currentImagePosition))
				MessageBox.Show(Resources.ImageRetrievalError, Resources.Oops, MessageBoxButtons.OK);

			lblCount.Text = string.Format("{0} of {1} images", (_currentOffset + 1), _totalDocImageCount);
		}

		private void LoadNasaImageList(int offset)
		{
			try
			{
				if (offset < 0)
				{
					offset = 0;
					_currentImagePosition = 49;
					_currentOffset = 49;
				}

				_images = JsonHelper.DownloadSerializedJsonData(string.Format(NasaLatestImagesUrl, offset));
				if (_images == null)
					throw new Exception("Unable to retrieve the image data");

				_totalDocImageCount = _images.Count;
				_totalNumberOfImages = _images.Nodes.Length;
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
					LoadNasaImageList(0);

				if (imagePosition < 0)
					throw new Exception("Invalid image position.");
				if (_images == null || _images.Nodes.Length < 0)
					throw new Exception("Image collection error. No images retrieved.");

				var imageNode = _images.Nodes[imagePosition].node;

				var imageUrl = imageNode.AutoImage466x248;
				var imageFullUrl = string.Format("{0}{1}", NasaImageBaseUrl, imageUrl);

				var thumbNailImagePath = string.Format("{0}\\NASA\\PicOfTheDay\\Thumbnails", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

				if (!Directory.Exists(thumbNailImagePath))
				{
					Directory.CreateDirectory(thumbNailImagePath);
				}

				string imageExtension = GetImageExtensionFromUrl(imageFullUrl);
				if (string.IsNullOrEmpty(imageExtension))
					throw new Exception("Invalid image extension from URL");

				//Retrieving the file name of the image
				int slashIdx = imageFullUrl.LastIndexOf("/");
				int extensionIdx = imageFullUrl.LastIndexOf(imageExtension);
				/*  All images, besides the master image,
				 * had ?<random letters> after the file extension. So i had to figure out what the extension was,
				 * substring from the last '/' up to the end of the extension.
				 * There might be better ways to do this, but i've been staring at it too long tonight.
				 */
				string imageName = imageFullUrl.Substring(slashIdx + 1, ((extensionIdx - 1) + imageExtension.Length) - slashIdx);

				var fullImagePath = string.Format("{0}\\thumb{1}", thumbNailImagePath, imageName);

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
