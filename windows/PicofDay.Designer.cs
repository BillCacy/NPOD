﻿namespace NasaPicOfDay
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
            this.txtimage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtimage
            // 
            this.txtimage.Enabled = false;
            this.txtimage.Location = new System.Drawing.Point(12, 12);
            this.txtimage.Multiline = true;
            this.txtimage.Name = "txtimage";
            this.txtimage.Size = new System.Drawing.Size(260, 238);
            this.txtimage.TabIndex = 0;
            // 
            // PicofDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.txtimage);
            this.Name = "PicofDay";
            this.Text = "PicOfDay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtimage;
    }
}

