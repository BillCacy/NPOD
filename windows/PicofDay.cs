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

namespace NasaPicOfDay
{
	public partial class PicofDay : Form
	{
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		[STAThread]
		static void Main()
		{
			Application.Run(new PicofDay());
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

			BackgroundChanger changer = new BackgroundChanger();
			BackgroundImage backgroundImage = changer.GetTodaysImage();
			changer.SetDesktopBackground(backgroundImage.DownloadedPath);

			try
			{

				this.components = new System.ComponentModel.Container();
				this.contextMenu1 = new System.Windows.Forms.ContextMenu();
				this.menuItem2 = new System.Windows.Forms.MenuItem();
				this.menuItem1 = new System.Windows.Forms.MenuItem();


				// Initialize contextMenu1
				this.contextMenu1.MenuItems.AddRange(
				new System.Windows.Forms.MenuItem[] { this.menuItem2, this.menuItem1 });

				// Initialize menuItem1
				this.menuItem1.Index = 0;
				this.menuItem1.Text = "E&xit";
				this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
				// Initialize menuItem2
				this.menuItem2.Index = 0;
				this.menuItem2.Text = "&See Details";
				this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

				// Set up how the form should be displayed.
				//this.ClientSize = new System.Drawing.Size(300,400);
				//this.Text = backgroundImage.ImageTitle;

				// Create the NotifyIcon.
				this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

				// The Icon property sets the icon that will appear
				// in the systray for this application.
				notifyIcon1.Icon = new Icon("trayIcon.ico");

				// The ContextMenu property sets the menu that will
				// appear when the systray icon is right clicked.
				notifyIcon1.ContextMenu = this.contextMenu1;

				// The Text property sets the text that will be displayed,
				// in a tooltip, when the mouse hovers over the systray icon.
				notifyIcon1.Text = backgroundImage.ImageTitle;
				notifyIcon1.Visible = true;
				this.txtImageDescr.Text = backgroundImage.ImageDescription;
				this.txtImageTitle.Text = backgroundImage.ImageTitle;
				this.lnkURL.Text = "Link to this Image";
				this.lnkURL.Links.Add(0, this.lnkURL.Text.Length, backgroundImage.ImageUrl);

				this.txtDate.Text = backgroundImage.ImageDate.ToLongDateString();

				// Handle the DoubleClick event to activate the form.
				notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteException(ex);
			}
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

		private void menuItem1_Click(object Sender, EventArgs e)
		{
			// Close the form, which closes the application.
			this.Close();
		}
		private void menuItem2_Click(object Sender, EventArgs e)
		{
			// Activate the form.
			this.Visible = true;
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
