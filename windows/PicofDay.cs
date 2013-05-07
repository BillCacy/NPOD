using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace NasaPicOfDay
{
	public partial class PicofDay : Form
	{
		private NotifyIcon notifyIcon1;
		private ContextMenu contextMenu1;
		private MenuItem exitMenuItem;
		private MenuItem detailsMenuItem;
        private MenuItem updateMenuItem;
		private System.Windows.Forms.Timer appTimer;

        //Added to ensure that only 1 instance of the application can be running at a time
        static Mutex mutex = new Mutex(true, "c6ed4943-2c8e-4382-af10-6455ec315896");

		[STAThread]
		static void Main()
		{
            //Checking to see if the application is running
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.Run(new PicofDay());
            }
            else
            {
                //Not doing anyting since the application is already running
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
				appTimer.Interval = 86400000; //milliseconds in 24 hours
				//appTimer.Interval = 60 * 1000; //testing only
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
		private void LoadApplicationContent()
		{
			try
			{
                this.components = new System.ComponentModel.Container();
                this.contextMenu1 = new ContextMenu();
                this.detailsMenuItem = new MenuItem();
                this.exitMenuItem = new MenuItem();
                this.updateMenuItem = new MenuItem();
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
				new MenuItem[] { this.detailsMenuItem, this.updateMenuItem, this.exitMenuItem });

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

				// Set up how the form should be displayed.
                this.TopMost = true;

				
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
		}

        private void UpdateContent()
        {
            BackgroundChanger changer = new BackgroundChanger();
            if (changer == null)
                throw new Exception("Error retrieving current image information.");

            BackgroundImage backgroundImage = changer.GetTodaysImage();
            changer.SetDesktopBackground(backgroundImage.DownloadedPath);

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.

            /* 4/28/2012 - Bill Cacy: Checking Image title for length > 64 characters.
             * Titles that are longer than 64 characters cause an error and the tray icon
             * does not load*/
            if (backgroundImage.ImageTitle.Length >= 63)
                this.notifyIcon1.Text = backgroundImage.ImageTitle.Substring(0, 63);
            else
                this.notifyIcon1.Text = backgroundImage.ImageTitle;

            this.notifyIcon1.Visible = true;
            this.txtImageDescr.Text = backgroundImage.ImageDescription;
            this.txtImageTitle.Text = backgroundImage.ImageTitle;
            this.lnkURL.Text = "Link to this Image";
            this.lnkURL.Links.Clear();
            this.lnkURL.Links.Add(0, this.lnkURL.Text.Length, backgroundImage.ImageUrl);

            this.txtDate.Text = backgroundImage.ImageDate.ToLongDateString();
        }

		void appTimer_Tick(object sender, EventArgs e)
		{
			//Reload the application content
            UpdateContent();
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
		}
		private void detailsMenuItem_Click(object Sender, EventArgs e)
		{
			// Activate the form.
			this.Visible = true;
		}
        private void updateMenuItem_Click(object sender, EventArgs e)
        {
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
	}
}
