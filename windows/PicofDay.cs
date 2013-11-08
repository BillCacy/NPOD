using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace NasaPicOfDay
{
	public partial class PicofDay : Form
	{
		private bool internetAvailable = true;
		private NotifyIcon notifyIcon1;
		private ContextMenu contextMenu1;
		private MenuItem exitMenuItem;
		private MenuItem detailsMenuItem;
		private MenuItem updateMenuItem;
		private MenuItem settingsMenuItem;
		private System.Windows.Forms.Timer appTimer;

		//Added to ensure that only 1 instance of the application can be running at a time
		static Mutex mutex = new Mutex(true, "c6ed4943-2c8e-4382-af10-6455ec315896");

		[STAThread]
		static void Main()
		{
			//Checking to see if the application is running
			if (mutex.WaitOne(TimeSpan.Zero, true))
			{
				if (!NetworkHelper.InternetAccessIsAvailable())
				{
					MessageBox.Show("NASA Pic of the Day requires an internet connection to retrieve images.\r\nPlease check your internet connection and try again.", "Internet Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
					Environment.Exit(Environment.ExitCode);
				}
				else
				{
					//Application is not currently running and there is an available internet connection
					Application.Run(new PicofDay());
				}
			}
			else
			{
				//If application is already running and there is no internet connection available, close the application
				if (!NetworkHelper.InternetAccessIsAvailable())
				{
					MessageBox.Show("NASA Pic of the Day requires an internet connection to retrieve images.\r\nPlease check your internet connection and try again.", "Internet Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
					Application.ExitThread();
				}
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			this.Visible = false;
			this.ShowInTaskbar = false;
			base.OnLoad(e);
		}
		public PicofDay()
		{
			InitializeComponent();

			try
			{
				//Creating a timer to retrieve the latest image once a day.
				//We did it this way to avoid setting up a scheduled task on the user's machine.
				appTimer = new System.Windows.Forms.Timer();

				//Checking for updates at 10:30 a.m. EST (GMT-5) everyday
				DateTime utcTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 30, 0);

				//Get the offset for the current time zone
				TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(utcTime);

				//create the current time with the GMT Offset
				DateTime currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (int)(utcTime.Hour + utcOffset.Hours), (int)(utcTime.Minute + utcOffset.Minutes), 0);

				//Get the amount of time between now and 10:30 a.m. EST
				TimeSpan timeUntilEST1030 = ((TimeSpan)(currentTime - DateTime.Now.ToUniversalTime()));

				if (timeUntilEST1030.Milliseconds <= 0)
				{
					//set the date to tomorrow by adding 24 hours
					currentTime = currentTime.AddHours(24);
					//get the number of milliseconds between currentTime and tomorrow at 10:30 a.m. EST
					timeUntilEST1030 = ((TimeSpan)(currentTime - DateTime.Now.ToUniversalTime()));
				}

				//set the interval for the timer
				appTimer.Interval = (int)timeUntilEST1030.TotalMilliseconds;
				appTimer.Tick += new EventHandler(appTimer_Tick);

				//Launch the main part of the data retrieval
				LoadApplicationContent();
				//Start the application timer
				appTimer.Start();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
		}

		void appTimer_Tick(object sender, EventArgs e)
		{
			//Reload the application content
			UpdateContent();
			//Update the timer to fire 24 hours from now
			appTimer.Interval = 86400000; //milliseconds in 24 hours
		}

		private void LoadApplicationContent()
		{
			try
			{
				this.components = new System.ComponentModel.Container();
				this.contextMenu1 = new ContextMenu();
				this.detailsMenuItem = new MenuItem();
				this.exitMenuItem = new MenuItem();
				this.updateMenuItem = new MenuItem();
				this.settingsMenuItem = new MenuItem();
				// Create the NotifyIcon.
				this.notifyIcon1 = new NotifyIcon(this.components);

				// The Icon property sets the icon that will appear
				// in the systray for this application.
				notifyIcon1.Icon = new Icon("world.ico");

				// The ContextMenu property sets the menu that will
				// appear when the systray icon is right clicked.
				notifyIcon1.ContextMenu = this.contextMenu1;

				// Handle the DoubleClick event to activate the form.
				notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);

				UpdateContent();

				// Initialize contextMenu1
				this.contextMenu1.MenuItems.AddRange(
				new MenuItem[] { this.detailsMenuItem, this.updateMenuItem, this.settingsMenuItem, this.exitMenuItem });

				// Initialize exitMenuItem
				this.exitMenuItem.Index = 0;
				this.exitMenuItem.Text = "E&xit";
				this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
				// Initialize detailsMenuItem
				this.detailsMenuItem.Index = 0;
				this.detailsMenuItem.Text = "&See Details";
				this.detailsMenuItem.Click += new System.EventHandler(this.detailsMenuItem_Click);
				// Initialize updateMenuItem
				this.updateMenuItem.Index = 0;
				this.updateMenuItem.Text = "Update";
				this.updateMenuItem.Click += new EventHandler(updateMenuItem_Click);
				//Initialize settingsMenuItme
				this.settingsMenuItem.Index = 0;
				this.settingsMenuItem.Text = "Settings";
				this.settingsMenuItem.Click += new EventHandler(settingsMenuItem_Click);

				// Set up how the form should be displayed.
				this.TopMost = true;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
		}

		//Launches the Settings form to allow the user to select previous images from the feed
		//when the form closes, the main controls will be updated with the new image's information.
		void settingsMenuItem_Click(object sender, EventArgs e)
		{
			ProcessHelper processHelper = new ProcessHelper();
			processHelper.BackgroundLoading(TestInternetConnection);
			processHelper.Start();
			processHelper = null;

			//Present the message that an internet connection is required
			if (!internetAvailable)
			{
				MessageBox.Show("NASA Pic of the Day requires an internet connection to retrieve images.\r\nPlease check your internet connection and try again.", "Internet Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			else
			{
				SettingsForm settingForm = new SettingsForm();
				settingForm.ShowDialog();
				UpdateControlContent();
			}
		}

		private void UpdateContent()
		{
			BackgroundChanger changer = new BackgroundChanger();
			GlobalVariables.NasaImage = changer.GetImage();
			changer.SetDesktopBackground(GlobalVariables.NasaImage.DownloadedPath);

			UpdateControlContent();
		}

		private void UpdateControlContent()
		{
			// The Text property sets the text that will be displayed,
			// in a tooltip, when the mouse hovers over the systray icon.
			// note: if the title is >= 63 characters, the notify icon will not load unless the text is substringed.
			if (GlobalVariables.NasaImage.ImageTitle.Length >= 63)
				this.notifyIcon1.Text = GlobalVariables.NasaImage.ImageTitle.Substring(0, 63);
			else
				this.notifyIcon1.Text = GlobalVariables.NasaImage.ImageTitle;

			this.notifyIcon1.Visible = true;
			this.txtImageDescr.Text = GlobalVariables.NasaImage.ImageDescription;
			this.txtImageTitle.Text = GlobalVariables.NasaImage.ImageTitle;
			this.lnkURL.Text = "Link to this Image";
			this.lnkURL.Links.Clear();
			this.lnkURL.Links.Add(0, this.lnkURL.Text.Length, GlobalVariables.NasaImage.ImageUrl);

			this.txtDate.Text = GlobalVariables.NasaImage.ImageDate.ToLongDateString();
		}


		private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
		{
			try
			{
				// Show the form when the user double clicks on the notify icon.
				// Set the WindowState to normal if the form is minimized.
				if (this.WindowState == FormWindowState.Minimized)
					this.WindowState = FormWindowState.Normal;

				// Activate the form.
				this.Visible = true;
				this.Activate();

			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
		}
		private void exitMenuItem_Click(object Sender, EventArgs e)
		{
			// Close the form, which closes the application.
			appTimer.Stop();
			appTimer = null;
			this.Close();
			Environment.Exit(Environment.ExitCode);
		}
		private void detailsMenuItem_Click(object Sender, EventArgs e)
		{
			// Activate the form.
			this.Visible = true;
		}
		private void updateMenuItem_Click(object sender, EventArgs e)
		{
			ProcessHelper processHelper = new ProcessHelper();
			processHelper.BackgroundLoading(TestInternetConnection);
			processHelper.Start();
			processHelper = null;

			if (!internetAvailable)
			{
				MessageBox.Show("NASA Pic of the Day requires an internet connection to retrieve images.\r\nPlease check your internet connection and try again.", "Internet Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			UpdateContent();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			//make the form invisible
			this.Visible = false;
		}
		private void lnkURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}

		private void TestInternetConnection()
		{
			internetAvailable = NetworkHelper.InternetAccessIsAvailable();
		}
	}
}
