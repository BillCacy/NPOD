using System;
using System.IO;
using System.Windows.Forms;
using NasaPicOfDay.Properties;

namespace NasaPicOfDay
{
	public partial class SettingsForm : Form
	{
		private int _totalDocImageCount;
		private int _currentImagePosition;
		private int _totalNumberOfImages;
		private const string NasaLatestImagesUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=50&offset={0}&routes=1446";
		private const string NasaImageBaseUrl = "http://www.nasa.gov";
		private NasaImages _images;
		private int _currentOffset;

		public SettingsForm()
		{
			InitializeComponent();
			SetButtonEnabled();
			chkBoxLogging.Checked = GlobalVariables.LoggingEnabled;
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

				var slashIdx = imageUrl.LastIndexOf("/", StringComparison.Ordinal);
				var extensionIdx = imageUrl.LastIndexOf(".jpg", StringComparison.Ordinal);
				var imageName = imageUrl.Substring(slashIdx + 1, (extensionIdx - slashIdx) + 3);

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

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			//Save Settings
			Settings.Default.LoggingEnabled = chkBoxLogging.Checked;
			GlobalVariables.LoggingEnabled = chkBoxLogging.Checked;
			MessageBox.Show(Resources.SettingSaved, Resources.Saved, MessageBoxButtons.OK);
		}
	}
}
