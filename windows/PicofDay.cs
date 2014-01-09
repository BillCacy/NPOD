using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NasaPicOfDay.Properties;
using Timer = System.Windows.Forms.Timer;

namespace NasaPicOfDay
{
	public partial class PicofDay : Form
	{
		#region Private Properties
		private bool _internetAvailable = true;
		private NotifyIcon _notifyIcon1;
		private ContextMenu _contextMenu1;
		private MenuItem _exitMenuItem;
		private MenuItem _detailsMenuItem;
		private MenuItem _updateMenuItem;
		private MenuItem _imagesMenuItem;
		private MenuItem _settingsMenuItem;
		private Timer _appTimer;
		#endregion

		//Added to ensure that only 1 instance of the application can be running at a time
		static readonly Mutex Mutex = new Mutex(true, "c6ed4943-2c8e-4382-af10-6455ec315896");

		[STAThread]
		static void Main()
		{
			//Checking to see if logging is enabled.
			GlobalVariables.LoggingEnabled = Settings.Default.LoggingEnabled;
			GlobalVariables.GetApplicationVersion();

			if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Application starting.");

			//Checking to see if the application is running
			if (Mutex.WaitOne(TimeSpan.Zero, true))
			{
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Checking internet connection.");
				if (!NetworkHelper.InternetAccessIsAvailable())
				{
					MessageBox.Show(Resources.InternetRequiredMessage, Resources.InternetRequiredTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("No internet connection. Application exiting.");
					Environment.Exit(Environment.ExitCode);
				}
				else
				{
					//Application is not currently running and there is an available internet connection

					//Check for updates
					if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Checking for application update.");
					if (ApplicationUpdater.UpdateIsAvailable())
					{
						//Kick off the updater
						var dlgResult = MessageBox.Show(Resources.UpdateQuestion, Resources.AnUpdateIsAvailable,
							MessageBoxButtons.YesNo);
						if (dlgResult == DialogResult.Yes)
						{
							//Need the 'runas' verb to allow admin privileges for update
							var startInfo = new ProcessStartInfo("Updater.exe") { Verb = "runas" };
							Process.Start(startInfo);
						}
					}

					Application.Run(new PicofDay());
				}
			}
			else
			{
				//If application is already running and there is no internet connection available, close the application
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Application is already running. But there is not internet connection. Exiting the application.");
				if (!NetworkHelper.InternetAccessIsAvailable())
				{
					MessageBox.Show(Resources.InternetRequiredMessage, Resources.InternetRequiredTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					Application.ExitThread();
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			Visible = false;
			ShowInTaskbar = false;
			base.OnLoad(e);
		}

		public PicofDay()
		{
			InitializeComponent();

			try
			{
				//Creating a timer to retrieve the latest image once a day.
				//We did it this way to avoid setting up a scheduled task on the user's machine.
				_appTimer = new Timer();

				//Checking for updates at 10:30 a.m. EST (GMT-5) everyday
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Checking the system time for 10:30 update.");

				var utcTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0);

				//Get the offset for the current time zone
				var utcOffset = TimeZoneInfo.Local.GetUtcOffset(utcTime);
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("UTC Offset is {0} in milliseconds.", utcOffset.TotalMilliseconds));

				//create the current time with the GMT Offset
				var currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, utcTime.Hour + utcOffset.Hours, utcTime.Minute + utcOffset.Minutes, 0);
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Current GMT configured time is {0}", currentTime));

				//Get the amount of time between now and 10:30 a.m. EST
				var timeUntilEst1030 = currentTime - DateTime.Now.ToUniversalTime();
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Time until 10:30 [{0}:{1}:{2}]", timeUntilEst1030.Hours, timeUntilEst1030.Minutes, timeUntilEst1030.Seconds));

				if (timeUntilEst1030.Milliseconds <= 0)
				{
					//set the date to tomorrow by adding 24 hours
					currentTime = currentTime.AddHours(24);
					//get the number of milliseconds between currentTime and tomorrow at 10:30 a.m. EST
					timeUntilEst1030 = currentTime - DateTime.Now.ToUniversalTime();
					if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Time until 10:30 [{0}:{1}:{2}]", timeUntilEst1030.Hours, timeUntilEst1030.Minutes, timeUntilEst1030.Seconds));
				}

				//set the interval for the timer
				_appTimer.Interval = (int)timeUntilEst1030.TotalMilliseconds;
				if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation(string.Format("Next update will occur in [{0}:{1}:{2}]", timeUntilEst1030.Hours, timeUntilEst1030.Minutes, timeUntilEst1030.Seconds));
				_appTimer.Tick += appTimer_Tick;

				//Launch the main part of the data retrieval
				LoadApplicationContent();
				//Start the application timer
				_appTimer.Start();
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
			_appTimer.Interval = 86400000; //milliseconds in 24 hours
		}

		private void LoadApplicationContent()
		{
			try
			{
				components = new Container();
				_contextMenu1 = new ContextMenu();
				_detailsMenuItem = new MenuItem();
				_exitMenuItem = new MenuItem();
				_updateMenuItem = new MenuItem();
				_imagesMenuItem = new MenuItem();
				_settingsMenuItem = new MenuItem();
				// Create the NotifyIcon.
				_notifyIcon1 = new NotifyIcon(components) { Icon = new Icon("world.ico"), ContextMenu = _contextMenu1 };

				// Handle the DoubleClick event to activate the form.
				_notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;

				UpdateContent();

				// Initialize contextMenu1
				_contextMenu1.MenuItems.AddRange(
				new[] { _imagesMenuItem, _updateMenuItem, _detailsMenuItem, _settingsMenuItem, _exitMenuItem });

				//Initialize imagesMenuItme
				_imagesMenuItem.Index = 0;
				_imagesMenuItem.Text = Resources.ImagesLabel;
				_imagesMenuItem.Click += imagesMenuItem_Click;
				// Initialize updateMenuItem
				_updateMenuItem.Index = 1;
				_updateMenuItem.Text = Resources.UpdateLabel;
				_updateMenuItem.Click += updateMenuItem_Click;
				// Initialize detailsMenuItem
				_detailsMenuItem.Index = 2;
				_detailsMenuItem.Text = Resources.SeeDetailsLabel;
				_detailsMenuItem.Click += detailsMenuItem_Click;
				//initialize settingsMenuItem
				_settingsMenuItem.Index = 3;
				_settingsMenuItem.Text = Resources.SettingsLabel;
				_settingsMenuItem.Click += settingsMenuItem_Click;
				// Initialize exitMenuItem
				_exitMenuItem.Index = 4;
				_exitMenuItem.Text = Resources.ExitLabel;
				_exitMenuItem.Click += exitMenuItem_Click;

				// Set up how the form should be displayed.
				TopMost = true;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
		}

		private void UpdateContent()
		{
			var changer = new BackgroundChanger();
			GlobalVariables.NasaImage = changer.GetImage();
			changer.SetDesktopBackground(GlobalVariables.NasaImage.DownloadedPath);

			UpdateControlContent();
		}

		private void UpdateControlContent()
		{
			// The Text property sets the text that will be displayed,
			// in a tooltip, when the mouse hovers over the systray icon.
			// note: if the title is >= 63 characters, the notify icon will not load unless the text is substringed.
			_notifyIcon1.Text = GlobalVariables.NasaImage.ImageTitle.Length >= 63 ? GlobalVariables.NasaImage.ImageTitle.Substring(0, 63) : GlobalVariables.NasaImage.ImageTitle;

			_notifyIcon1.Visible = true;
			txtImageDescr.Text = GlobalVariables.NasaImage.ImageDescription;
			txtImageTitle.Text = GlobalVariables.NasaImage.ImageTitle;
			lnkURL.Text = Resources.LinkToThisImage;
			lnkURL.Links.Clear();
			lnkURL.Links.Add(0, lnkURL.Text.Length, GlobalVariables.NasaImage.ImageUrl);

			txtDate.Text = GlobalVariables.NasaImage.ImageDate.ToLongDateString();
		}

		private void imagesMenuItem_Click(object sender, EventArgs e)
		{
			if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Images option selected.");
			var processHelper = new ProcessHelper();
			processHelper.BackgroundLoading(TestInternetConnection);
			processHelper.Start();

			if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Checking internet connectivity");
			//Present the message that an internet connection is required
			if (!_internetAvailable)
			{
				MessageBox.Show(Resources.InternetRequiredMessage, Resources.InternetRequiredTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var imagesForm = new ImagesForm();
			imagesForm.ShowDialog();
			UpdateControlContent();
		}
		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				// Show the form when the user double clicks on the notify icon.
				// Set the WindowState to normal if the form is minimized.
				if (WindowState == FormWindowState.Minimized)
					WindowState = FormWindowState.Normal;

				// Activate the form.
				Visible = true;
				Activate();

			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
		}
		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			// Close the form, which closes the application.
			_appTimer.Stop();
			_appTimer = null;
			Close();
			Environment.Exit(Environment.ExitCode);
		}
		private void detailsMenuItem_Click(object sender, EventArgs e)
		{
			// Activate the form.
			Visible = true;
		}
		private void updateMenuItem_Click(object sender, EventArgs e)
		{
			if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Update has been requested.");
			var processHelper = new ProcessHelper();
			processHelper.BackgroundLoading(TestInternetConnection);
			processHelper.Start();

			if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Checking internet connectivity");
			if (!_internetAvailable)
			{
				MessageBox.Show(Resources.InternetRequiredMessage, Resources.InternetRequiredTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			UpdateContent();
		}
		private void settingsMenuItem_Click(object sender, EventArgs e)
		{
			if (GlobalVariables.LoggingEnabled) ExceptionManager.WriteInformation("Settings option selected.");

			var settingForm = new SettingsForm();
			settingForm.ShowDialog();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			//make the form invisible
			Visible = false;
		}
		private void lnkURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(e.Link.LinkData.ToString());
		}
		private void TestInternetConnection()
		{
			_internetAvailable = NetworkHelper.InternetAccessIsAvailable();
		}
	}
}
