using System;
using System.IO;
using System.Windows.Forms;
using NasaPicOfDay.Properties;

namespace NasaPicOfDay
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
			chkBoxLogging.Checked = GlobalVariables.LoggingEnabled;
		}

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			//Save Settings
			Settings.Default.LoggingEnabled = chkBoxLogging.Checked;
			GlobalVariables.LoggingEnabled = chkBoxLogging.Checked;
			MessageBox.Show(Resources.SettingSaved, Resources.Saved, MessageBoxButtons.OK);
		}

		private void btnCloseSettings_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
