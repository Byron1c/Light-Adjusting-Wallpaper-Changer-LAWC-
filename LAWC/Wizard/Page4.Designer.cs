namespace LAWC.Wizard
{
    partial class Page4
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page4));
            this.label8 = new System.Windows.Forms.Label();
            this.pbDonate = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblExtraInfo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbDonate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(103, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(389, 34);
            this.label8.TabIndex = 188;
            this.label8.Text = "If you like this application, please \r\nconsider donating something to support fur" +
    "ther development";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbDonate
            // 
            this.pbDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDonate.Image = ((System.Drawing.Image)(resources.GetObject("pbDonate.Image")));
            this.pbDonate.InitialImage = null;
            this.pbDonate.Location = new System.Drawing.Point(241, 358);
            this.pbDonate.Name = "pbDonate";
            this.pbDonate.Size = new System.Drawing.Size(100, 50);
            this.pbDonate.TabIndex = 189;
            this.pbDonate.TabStop = false;
            this.pbDonate.Click += new System.EventHandler(this.pbDonate_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(156, 196);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(288, 186);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 190;
            this.pictureBox1.TabStop = false;
            // 
            // lblExtraInfo
            // 
            this.lblExtraInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtraInfo.Location = new System.Drawing.Point(15, 38);
            this.lblExtraInfo.Name = "lblExtraInfo";
            this.lblExtraInfo.Size = new System.Drawing.Size(568, 116);
            this.lblExtraInfo.TabIndex = 191;
            this.lblExtraInfo.Text = resources.GetString("lblExtraInfo.Text");
            this.lblExtraInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(210, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(177, 17);
            this.lblTitle.TabIndex = 192;
            this.lblTitle.Text = "Wizard Setup Complete";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblExtraInfo);
            this.Controls.Add(this.pbDonate);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label8);
            this.Name = "Page4";
            this.Size = new System.Drawing.Size(600, 419);
            ((System.ComponentModel.ISupportInitialize)(this.pbDonate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pbDonate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblExtraInfo;
        private System.Windows.Forms.Label lblTitle;
    }
}
