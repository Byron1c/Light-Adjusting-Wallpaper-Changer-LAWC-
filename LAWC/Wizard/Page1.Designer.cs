namespace LAWC.Wizard
{
    partial class Page1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numChangeFrequencyMinutes = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbShowToolTips = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChangeFrequencyMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(144, 45);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(430, 96);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(10, 13);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(128, 128);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 15;
            this.pbLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(144, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "LAWC - Light Adjusting Wallpaper Changer - Setup Wizard";
            // 
            // numChangeFrequencyMinutes
            // 
            this.numChangeFrequencyMinutes.Location = new System.Drawing.Point(325, 342);
            this.numChangeFrequencyMinutes.Name = "numChangeFrequencyMinutes";
            this.numChangeFrequencyMinutes.Size = new System.Drawing.Size(50, 20);
            this.numChangeFrequencyMinutes.TabIndex = 19;
            this.numChangeFrequencyMinutes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numChangeFrequencyMinutes.ValueChanged += new System.EventHandler(this.numChangeFrequencyMinutes_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 344);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(308, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "How many minutes would you like between wallpaper changes?";
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAutoStart.Location = new System.Drawing.Point(10, 319);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(339, 17);
            this.cbAutoStart.TabIndex = 17;
            this.cbAutoStart.Text = "Would you like LAWC to automatically start when Windows starts?";
            this.cbAutoStart.UseVisualStyleBackColor = true;
            this.cbAutoStart.CheckedChanged += new System.EventHandler(this.cbAutoStart_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(10, 148);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(564, 151);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // cbShowToolTips
            // 
            this.cbShowToolTips.AutoSize = true;
            this.cbShowToolTips.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbShowToolTips.Checked = true;
            this.cbShowToolTips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowToolTips.Location = new System.Drawing.Point(10, 369);
            this.cbShowToolTips.Name = "cbShowToolTips";
            this.cbShowToolTips.Size = new System.Drawing.Size(324, 30);
            this.cbShowToolTips.TabIndex = 155;
            this.cbShowToolTips.Text = "HELP: Show Tool Tips when holding the cursor over a control. \r\nNew Users should t" +
    "urn this on.";
            this.cbShowToolTips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbShowToolTips.UseVisualStyleBackColor = true;
            this.cbShowToolTips.CheckedChanged += new System.EventHandler(this.cbShowToolTips_CheckedChanged);
            // 
            // Page1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbShowToolTips);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numChangeFrequencyMinutes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbAutoStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Page1";
            this.Size = new System.Drawing.Size(600, 419);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChangeFrequencyMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numChangeFrequencyMinutes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cbShowToolTips;
    }
}
