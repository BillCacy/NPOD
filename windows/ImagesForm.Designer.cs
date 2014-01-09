namespace NasaPicOfDay
{
    partial class ImagesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagesForm));
			this.btnBackImage = new System.Windows.Forms.Button();
			this.btnCurrentImage = new System.Windows.Forms.Button();
			this.btnForwardImage = new System.Windows.Forms.Button();
			this.btnSetImage = new System.Windows.Forms.Button();
			this.btnCloseSettings = new System.Windows.Forms.Button();
			this.picBoxCurrentImg = new System.Windows.Forms.PictureBox();
			this.lblCount = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picBoxCurrentImg)).BeginInit();
			this.SuspendLayout();
			// 
			// btnBackImage
			// 
			this.btnBackImage.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBackImage.Location = new System.Drawing.Point(120, 35);
			this.btnBackImage.Name = "btnBackImage";
			this.btnBackImage.Size = new System.Drawing.Size(75, 23);
			this.btnBackImage.TabIndex = 0;
			this.btnBackImage.Text = "<< Back";
			this.btnBackImage.UseVisualStyleBackColor = true;
			this.btnBackImage.Click += new System.EventHandler(this.btnBackImage_Click);
			// 
			// btnCurrentImage
			// 
			this.btnCurrentImage.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCurrentImage.Location = new System.Drawing.Point(201, 35);
			this.btnCurrentImage.Name = "btnCurrentImage";
			this.btnCurrentImage.Size = new System.Drawing.Size(75, 23);
			this.btnCurrentImage.TabIndex = 1;
			this.btnCurrentImage.Text = "Today\'s";
			this.btnCurrentImage.UseVisualStyleBackColor = true;
			this.btnCurrentImage.Click += new System.EventHandler(this.btnCurrentImage_Click);
			// 
			// btnForwardImage
			// 
			this.btnForwardImage.Enabled = false;
			this.btnForwardImage.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnForwardImage.Location = new System.Drawing.Point(279, 35);
			this.btnForwardImage.Name = "btnForwardImage";
			this.btnForwardImage.Size = new System.Drawing.Size(89, 23);
			this.btnForwardImage.TabIndex = 2;
			this.btnForwardImage.Text = "Forward >>";
			this.btnForwardImage.UseVisualStyleBackColor = true;
			this.btnForwardImage.Click += new System.EventHandler(this.btnForwardImage_Click);
			// 
			// btnSetImage
			// 
			this.btnSetImage.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetImage.Location = new System.Drawing.Point(120, 370);
			this.btnSetImage.Name = "btnSetImage";
			this.btnSetImage.Size = new System.Drawing.Size(248, 23);
			this.btnSetImage.TabIndex = 3;
			this.btnSetImage.Text = "Set this image as desktop backround";
			this.btnSetImage.UseVisualStyleBackColor = true;
			this.btnSetImage.Click += new System.EventHandler(this.btnSetImage_Click);
			// 
			// btnCloseSettings
			// 
			this.btnCloseSettings.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCloseSettings.Location = new System.Drawing.Point(201, 399);
			this.btnCloseSettings.Name = "btnCloseSettings";
			this.btnCloseSettings.Size = new System.Drawing.Size(75, 23);
			this.btnCloseSettings.TabIndex = 4;
			this.btnCloseSettings.Text = "Close";
			this.btnCloseSettings.UseVisualStyleBackColor = true;
			this.btnCloseSettings.Click += new System.EventHandler(this.btnCloseSettings_Click);
			// 
			// picBoxCurrentImg
			// 
			this.picBoxCurrentImg.Location = new System.Drawing.Point(11, 105);
			this.picBoxCurrentImg.Name = "picBoxCurrentImg";
			this.picBoxCurrentImg.Size = new System.Drawing.Size(466, 248);
			this.picBoxCurrentImg.TabIndex = 5;
			this.picBoxCurrentImg.TabStop = false;
			// 
			// lblCount
			// 
			this.lblCount.AutoSize = true;
			this.lblCount.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCount.Location = new System.Drawing.Point(198, 75);
			this.lblCount.Name = "lblCount";
			this.lblCount.Size = new System.Drawing.Size(39, 16);
			this.lblCount.TabIndex = 6;
			this.lblCount.Text = "label1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(152, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 16);
			this.label1.TabIndex = 9;
			this.label1.Text = "Application Version:";
			// 
			// versionLabel
			// 
			this.versionLabel.AutoSize = true;
			this.versionLabel.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.versionLabel.Location = new System.Drawing.Point(266, 9);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size(0, 16);
			this.versionLabel.TabIndex = 10;
			// 
			// ImagesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(489, 432);
			this.Controls.Add(this.versionLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblCount);
			this.Controls.Add(this.picBoxCurrentImg);
			this.Controls.Add(this.btnCloseSettings);
			this.Controls.Add(this.btnSetImage);
			this.Controls.Add(this.btnForwardImage);
			this.Controls.Add(this.btnCurrentImage);
			this.Controls.Add(this.btnBackImage);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImagesForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NPOD Images";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.picBoxCurrentImg)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBackImage;
        private System.Windows.Forms.Button btnCurrentImage;
        private System.Windows.Forms.Button btnForwardImage;
        private System.Windows.Forms.Button btnSetImage;
        private System.Windows.Forms.Button btnCloseSettings;
        private System.Windows.Forms.PictureBox picBoxCurrentImg;
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label versionLabel;
    }
}