using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace NasaPicOfDay
{
	public partial class SettingsForm : Form
	{
		private int _TotalDocImageCount = 0;
		private int _CurrentImagePosition = 0;
		private int _TotalNumberOfImages = 0;
		private string _NasaLatestImagesUrl = "http://www.nasa.gov/ws/image_gallery.jsonp?format_output=1&display_id=page_1&limit=50&offset={0}&routes=1446";
		private string _NasaImageBaseUrl = "http://www.nasa.gov";
		private NasaImages _Images;
		private int _CurrentOffset = 0;

		public SettingsForm()
		{
			InitializeComponent();
			SetButtonEnabled();
			chkBoxLogging.Checked = GlobalVariables.LoggingEnabled;
		}

		private void btnCloseSettings_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnBackImage_Click(object sender, EventArgs e)
		{
			if (_CurrentOffset == _TotalDocImageCount - 1)
			{
				btnBackImage.Enabled = false;
				return;
			}
			else
			{
				if (_CurrentImagePosition < _TotalNumberOfImages - 1)
				{
					_CurrentImagePosition++;
					_CurrentOffset++;
				}
				else if (_CurrentImagePosition == _TotalNumberOfImages - 1)
				{
					//need to grab the next 50 images
					_CurrentOffset++;
					LoadNasaImageList(_CurrentOffset);
					_CurrentImagePosition = 0;
				}
				else if (_CurrentOffset == _TotalDocImageCount - 1)
				{
					SetButtonEnabled();
					return;
				}

				SetButtonEnabled();
				GetImageThumbnail(_CurrentImagePosition);
				lblCount.Text = string.Format("{0} of {1} images", (_CurrentOffset + 1).ToString(), _TotalDocImageCount.ToString());
			}
		}

		private void btnForwardImage_Click(object sender, EventArgs e)
		{
			if (_CurrentOffset == 0)
			{
				btnForwardImage.Enabled = false;
				return;
			}
			else
			{
				if (_CurrentImagePosition > 0)
				{
					_CurrentImagePosition--;
					_CurrentOffset--;
				}
				else if (_CurrentOffset % 50 == 0)
				{
					//need to grab the next 50 images
					_CurrentOffset--;
					LoadNasaImageList(_CurrentOffset - 50);
					_CurrentImagePosition = 49;
				}
			}

			SetButtonEnabled();
			GetImageThumbnail(_CurrentImagePosition);
			lblCount.Text = string.Format("{0} of {1} images", (_CurrentOffset + 1).ToString(), _TotalDocImageCount.ToString());
		}

		private void SetButtonEnabled()
		{
			if (_CurrentOffset == _TotalDocImageCount)
			{
				btnBackImage.Enabled = false;
			}
			else
			{
				btnBackImage.Enabled = true;
			}

			if (_CurrentOffset == 0)
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
			_CurrentOffset = 0;
			LoadNasaImageList(0);
			SetButtonEnabled();
			GetImageThumbnail(_CurrentImagePosition);
			lblCount.Text = string.Format("{0} of {1} images", (_CurrentOffset + 1).ToString(), _TotalDocImageCount.ToString());
		}

		private void btnSetImage_Click(object sender, EventArgs e)
		{
			BackgroundChanger bgChanger = new BackgroundChanger();
			GlobalVariables.NasaImage = bgChanger.GetImage(_CurrentOffset);
			bgChanger.SetDesktopBackground(GlobalVariables.NasaImage.DownloadedPath);
			this.Close();
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			LoadNasaImageList(0);

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

			lblCount.Text = string.Format("{0} of {1} images", (_CurrentOffset + 1).ToString(), _TotalDocImageCount.ToString());
		}

		private void LoadNasaImageList(int offset)
		{
			try
			{
				if (offset < 0)
				{
					offset = 0;
					_CurrentImagePosition = 49;
					_CurrentOffset = 49;
				}

				_Images = JsonHelper.DownloadSerializedJsonData(string.Format(_NasaLatestImagesUrl, offset));
				if (_Images == null)
					throw new Exception("Unable to retrieve the image data");

				_TotalDocImageCount = _Images.Count;
				_TotalNumberOfImages = _Images.Nodes.Length;
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
					LoadNasaImageList(0);

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

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			//Save Settings
			Properties.Settings.Default.LoggingEnabled = chkBoxLogging.Checked;
			GlobalVariables.LoggingEnabled = chkBoxLogging.Checked;
			MessageBox.Show("Setting saved.", "Saved", MessageBoxButtons.OK);
		}
	}
}
