namespace NasaPicOfDay
{
	partial class SettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.chkBoxLogging = new System.Windows.Forms.CheckBox();
			this.btnCloseSettings = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSaveSettings.Location = new System.Drawing.Point(165, 19);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
			this.btnSaveSettings.TabIndex = 10;
			this.btnSaveSettings.Text = "Save";
			this.btnSaveSettings.UseVisualStyleBackColor = true;
			// 
			// chkBoxLogging
			// 
			this.chkBoxLogging.AutoSize = true;
			this.chkBoxLogging.Location = new System.Drawing.Point(44, 23);
			this.chkBoxLogging.Name = "chkBoxLogging";
			this.chkBoxLogging.Size = new System.Drawing.Size(115, 17);
			this.chkBoxLogging.TabIndex = 9;
			this.chkBoxLogging.Text = "Logging is enabled";
			this.chkBoxLogging.UseVisualStyleBackColor = true;
			// 
			// btnCloseSettings
			// 
			this.btnCloseSettings.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCloseSettings.Location = new System.Drawing.Point(197, 101);
			this.btnCloseSettings.Name = "btnCloseSettings";
			this.btnCloseSettings.Size = new System.Drawing.Size(75, 23);
			this.btnCloseSettings.TabIndex = 11;
			this.btnCloseSettings.Text = "Close";
			this.btnCloseSettings.UseVisualStyleBackColor = true;
			this.btnCloseSettings.Click += new System.EventHandler(this.btnCloseSettings_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 136);
			this.Controls.Add(this.btnCloseSettings);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.chkBoxLogging);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NPOD Settings";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.CheckBox chkBoxLogging;
		private System.Windows.Forms.Button btnCloseSettings;
	}
}