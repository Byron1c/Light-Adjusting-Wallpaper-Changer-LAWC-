namespace LAWC
{
    partial class frmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCancel = new System.Windows.Forms.Label();
            this.pnlCancel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentStep.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentStep.Location = new System.Drawing.Point(12, 229);
            this.lblCurrentStep.Name = "lblCurrentStep";
            this.lblCurrentStep.Padding = new System.Windows.Forms.Padding(2);
            this.lblCurrentStep.Size = new System.Drawing.Size(307, 31);
            this.lblCurrentStep.TabIndex = 1;
            this.lblCurrentStep.Text = "Current Step";
            this.lblCurrentStep.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCurrentStep.UseWaitCursor = true;
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(84, 59);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(162, 155);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 2;
            this.pbLogo.TabStop = false;
            this.pbLogo.UseWaitCursor = true;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(242, 7);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version ";
            this.lblVersion.UseWaitCursor = true;
            // 
            // lblCancel
            // 
            this.lblCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCancel.AutoSize = true;
            this.lblCancel.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.lblCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancel.ForeColor = System.Drawing.Color.Black;
            this.lblCancel.Location = new System.Drawing.Point(280, 249);
            this.lblCancel.Name = "lblCancel";
            this.lblCancel.Size = new System.Drawing.Size(39, 13);
            this.lblCancel.TabIndex = 5;
            this.lblCancel.Text = "cancel";
            this.lblCancel.UseWaitCursor = true;
            this.lblCancel.Visible = false;
            this.lblCancel.Click += new System.EventHandler(this.lblCancel_Click);
            // 
            // pnlCancel
            // 
            this.pnlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCancel.BackColor = System.Drawing.Color.Transparent;
            this.pnlCancel.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.pnlCancel.Location = new System.Drawing.Point(268, 243);
            this.pnlCancel.Name = "pnlCancel";
            this.pnlCancel.Size = new System.Drawing.Size(63, 28);
            this.pnlCancel.TabIndex = 6;
            this.pnlCancel.UseWaitCursor = true;
            this.pnlCancel.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(2);
            this.lblTitle.Size = new System.Drawing.Size(307, 31);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "LAWC";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTitle.UseWaitCursor = true;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(331, 271);
            this.ControlBox = false;
            this.Controls.Add(this.lblCancel);
            this.Controls.Add(this.pnlCancel);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lblCurrentStep);
            this.Controls.Add(this.lblTitle);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplash";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.frmSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.PictureBox pbLogo;
        internal System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCancel;
        private System.Windows.Forms.Panel pnlCancel;
        internal System.Windows.Forms.Label lblTitle;
    }
}