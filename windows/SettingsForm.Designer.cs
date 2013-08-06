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
            this.btnBackImage = new System.Windows.Forms.Button();
            this.btnCurrentImage = new System.Windows.Forms.Button();
            this.btnForwardImage = new System.Windows.Forms.Button();
            this.btnSetImage = new System.Windows.Forms.Button();
            this.btnCloseSettings = new System.Windows.Forms.Button();
            this.picBoxCurrentImg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurrentImg)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBackImage
            // 
            this.btnBackImage.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackImage.Location = new System.Drawing.Point(159, 12);
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
            this.btnCurrentImage.Location = new System.Drawing.Point(197, 41);
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
            this.btnForwardImage.Location = new System.Drawing.Point(240, 12);
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
            this.btnSetImage.Location = new System.Drawing.Point(120, 336);
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
            this.btnCloseSettings.Location = new System.Drawing.Point(207, 376);
            this.btnCloseSettings.Name = "btnCloseSettings";
            this.btnCloseSettings.Size = new System.Drawing.Size(75, 23);
            this.btnCloseSettings.TabIndex = 4;
            this.btnCloseSettings.Text = "Close";
            this.btnCloseSettings.UseVisualStyleBackColor = true;
            this.btnCloseSettings.Click += new System.EventHandler(this.btnCloseSettings_Click);
            // 
            // picBoxCurrentImg
            // 
            this.picBoxCurrentImg.Location = new System.Drawing.Point(12, 70);
            this.picBoxCurrentImg.Name = "picBoxCurrentImg";
            this.picBoxCurrentImg.Size = new System.Drawing.Size(466, 248);
            this.picBoxCurrentImg.TabIndex = 5;
            this.picBoxCurrentImg.TabStop = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 411);
            this.Controls.Add(this.picBoxCurrentImg);
            this.Controls.Add(this.btnCloseSettings);
            this.Controls.Add(this.btnSetImage);
            this.Controls.Add(this.btnForwardImage);
            this.Controls.Add(this.btnCurrentImage);
            this.Controls.Add(this.btnBackImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPOD Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurrentImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBackImage;
        private System.Windows.Forms.Button btnCurrentImage;
        private System.Windows.Forms.Button btnForwardImage;
        private System.Windows.Forms.Button btnSetImage;
        private System.Windows.Forms.Button btnCloseSettings;
        private System.Windows.Forms.PictureBox picBoxCurrentImg;
    }
}