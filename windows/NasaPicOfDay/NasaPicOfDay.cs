using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;


namespace NasaPicOfDay
{
    public class nPOD : System.Windows.Forms.Form
    {
     [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
    private static UInt32 SPI_SETDESKWALLPAPER = 20;
    private static UInt32 SPIF_UPDATEINIFILE = 0x1;
    private static string imageFileName = "C:\\nasaimages\\NASA_ImageOfDay.bmp";

    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.ContextMenu contextMenu1;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.ComponentModel.IContainer components;

        [STAThread]
         static void Main()
        {
                        setImage(imageFileName);
            Application.Run(new nPOD());

        }
    public nPOD()
    {
        try
        {
            this.WindowState = FormWindowState.Minimized;

            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Notify Icon Example";

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = new Icon("npod_tray.ico");

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Nasa Pic of the day app";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
        }
        catch
        {
            //todo
        }

    }

    protected override void Dispose( bool disposing )
    {
        
        try
        {
            // Clean up any components being used.
        if( disposing )
            if (components != null)
                components.Dispose();            

        base.Dispose( disposing );
                }
        catch
        {
            //todo
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
            this.Activate();

        }
        catch
        {
            //todo
        }
    }

    private void menuItem1_Click(object Sender, EventArgs e) {
        // Close the form, which closes the application.
        this.Close();
    }

    public static void setImage( string filename )
      {
          try
          {
              SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
          }
          catch
          {
              //todo
          }
      } 
   
    }
 
}