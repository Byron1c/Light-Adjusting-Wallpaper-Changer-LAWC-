namespace LAWC.Wizard
{
    partial class Page3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page3));
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAdjustments = new System.Windows.Forms.Label();
            this.cbResetImageOptions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(353, 165);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(168, 13);
            this.label9.TabIndex = 204;
            this.label9.Text = "Note: You can add more in LAWC";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(87, 346);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(411, 17);
            this.label8.TabIndex = 203;
            this.label8.Text = "You are now done setting up the main options in LAWC!";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 221);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(552, 52);
            this.label7.TabIndex = 202;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(300, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 23);
            this.button1.TabIndex = 197;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(267, 13);
            this.label3.TabIndex = 196;
            this.label3.Text = "Choose a folder to add your wallpaper images to LAWC";
            // 
            // lblAdjustments
            // 
            this.lblAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAdjustments.AutoSize = true;
            this.lblAdjustments.Location = new System.Drawing.Point(59, 50);
            this.lblAdjustments.Name = "lblAdjustments";
            this.lblAdjustments.Size = new System.Drawing.Size(225, 13);
            this.lblAdjustments.TabIndex = 205;
            this.lblAdjustments.Text = "Image Adjustment during the Dark / Night time";
            // 
            // cbResetImageOptions
            // 
            this.cbResetImageOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbResetImageOptions.FormattingEnabled = true;
            this.cbResetImageOptions.Items.AddRange(new object[] {
            "Please Choose (Optional)",
            "A Little Darker",
            "Darker",
            "Much Darker",
            "Very Dark",
            "None"});
            this.cbResetImageOptions.Location = new System.Drawing.Point(290, 47);
            this.cbResetImageOptions.Name = "cbResetImageOptions";
            this.cbResetImageOptions.Size = new System.Drawing.Size(165, 21);
            this.cbResetImageOptions.TabIndex = 206;
            this.cbResetImageOptions.Text = "Please Choose (Optional)";
            this.cbResetImageOptions.SelectedIndexChanged += new System.EventHandler(this.cbResetImageOptions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 39);
            this.label1.TabIndex = 207;
            this.label1.Text = "This will set how dark the wallpapers will be, during the Dark\r\ntime (usually set" +
    " to night time). You can tweak them later in the\r\nAdvanced Settings window.";
            // 
            // Page3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAdjustments);
            this.Controls.Add(this.cbResetImageOptions);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Name = "Page3";
            this.Size = new System.Drawing.Size(600, 419);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAdjustments;
        internal System.Windows.Forms.ComboBox cbResetImageOptions;
        private System.Windows.Forms.Label label1;
    }
}
