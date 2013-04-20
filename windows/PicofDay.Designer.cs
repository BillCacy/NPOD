namespace NasaPicOfDay
{
    partial class PicofDay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicofDay));
            this.txtImageDescr = new System.Windows.Forms.TextBox();
            this.txtImageTitle = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.lnkURL = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // txtImageDescr
            // 
            this.txtImageDescr.Location = new System.Drawing.Point(12, 45);
            this.txtImageDescr.Multiline = true;
            this.txtImageDescr.Name = "txtImageDescr";
            this.txtImageDescr.ReadOnly = true;
            this.txtImageDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtImageDescr.Size = new System.Drawing.Size(291, 205);
            this.txtImageDescr.TabIndex = 0;
            // 
            // txtImageTitle
            // 
            this.txtImageTitle.Location = new System.Drawing.Point(12, 18);
            this.txtImageTitle.Name = "txtImageTitle";
            this.txtImageTitle.ReadOnly = true;
            this.txtImageTitle.Size = new System.Drawing.Size(265, 20);
            this.txtImageTitle.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(81, 326);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(136, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close Details";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(12, 291);
            this.txtDate.Name = "txtDate";
            this.txtDate.ReadOnly = true;
            this.txtDate.Size = new System.Drawing.Size(265, 20);
            this.txtDate.TabIndex = 4;
            // 
            // lnkURL
            // 
            this.lnkURL.AutoSize = true;
            this.lnkURL.Location = new System.Drawing.Point(12, 262);
            this.lnkURL.Name = "lnkURL";
            this.lnkURL.Size = new System.Drawing.Size(55, 13);
            this.lnkURL.TabIndex = 5;
            this.lnkURL.TabStop = true;
            this.lnkURL.Text = "linkLabel1";
            this.lnkURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkURL_LinkClicked);
            // 
            // PicofDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 361);
            this.Controls.Add(this.lnkURL);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtImageTitle);
            this.Controls.Add(this.txtImageDescr);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PicofDay";
            this.Text = "NASA Pic Of Day Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtImageDescr;
        private System.Windows.Forms.TextBox txtImageTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.LinkLabel lnkURL;
    }
}

