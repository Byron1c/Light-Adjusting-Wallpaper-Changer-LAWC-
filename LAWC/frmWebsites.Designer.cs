namespace LAWC
{
    partial class frmWebsites
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebsites));
            this.lnkDesktopNexus = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.lnkWallpaperUp = new System.Windows.Forms.LinkLabel();
            this.lnkWallpaperAbyss = new System.Windows.Forms.LinkLabel();
            this.lnkWallpapersCraft = new System.Windows.Forms.LinkLabel();
            this.lnkUnsplash = new System.Windows.Forms.LinkLabel();
            this.lnkPexels = new System.Windows.Forms.LinkLabel();
            this.lnkHDWallpapers = new System.Windows.Forms.LinkLabel();
            this.lnkWallpapersWide = new System.Windows.Forms.LinkLabel();
            this.lnkPlanWallpaper = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.olvWebsites = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnLastVisit = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDone = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cmWebsites = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsDoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsNotDoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.olvWebsites)).BeginInit();
            this.cmWebsites.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkDesktopNexus
            // 
            this.lnkDesktopNexus.AutoSize = true;
            this.lnkDesktopNexus.Location = new System.Drawing.Point(26, 22);
            this.lnkDesktopNexus.Name = "lnkDesktopNexus";
            this.lnkDesktopNexus.Size = new System.Drawing.Size(80, 13);
            this.lnkDesktopNexus.TabIndex = 0;
            this.lnkDesktopNexus.TabStop = true;
            this.lnkDesktopNexus.Text = "Desktop Nexus";
            this.lnkDesktopNexus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDesktopNexus_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(26, 51);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(57, 13);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "DeviantArt";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // lnkWallpaperUp
            // 
            this.lnkWallpaperUp.AutoSize = true;
            this.lnkWallpaperUp.Location = new System.Drawing.Point(26, 80);
            this.lnkWallpaperUp.Name = "lnkWallpaperUp";
            this.lnkWallpaperUp.Size = new System.Drawing.Size(69, 13);
            this.lnkWallpaperUp.TabIndex = 2;
            this.lnkWallpaperUp.TabStop = true;
            this.lnkWallpaperUp.Text = "WallpaperUp";
            this.lnkWallpaperUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWallpaperUp_LinkClicked);
            // 
            // lnkWallpaperAbyss
            // 
            this.lnkWallpaperAbyss.AutoSize = true;
            this.lnkWallpaperAbyss.Location = new System.Drawing.Point(26, 109);
            this.lnkWallpaperAbyss.Name = "lnkWallpaperAbyss";
            this.lnkWallpaperAbyss.Size = new System.Drawing.Size(86, 13);
            this.lnkWallpaperAbyss.TabIndex = 3;
            this.lnkWallpaperAbyss.TabStop = true;
            this.lnkWallpaperAbyss.Text = "Wallpaper Abyss";
            this.lnkWallpaperAbyss.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWallpaperAbyss_LinkClicked);
            // 
            // lnkWallpapersCraft
            // 
            this.lnkWallpapersCraft.AutoSize = true;
            this.lnkWallpapersCraft.Location = new System.Drawing.Point(26, 138);
            this.lnkWallpapersCraft.Name = "lnkWallpapersCraft";
            this.lnkWallpapersCraft.Size = new System.Drawing.Size(85, 13);
            this.lnkWallpapersCraft.TabIndex = 4;
            this.lnkWallpapersCraft.TabStop = true;
            this.lnkWallpapersCraft.Text = "Wallpapers Craft";
            this.lnkWallpapersCraft.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWallpapersCraft_LinkClicked);
            // 
            // lnkUnsplash
            // 
            this.lnkUnsplash.AutoSize = true;
            this.lnkUnsplash.Location = new System.Drawing.Point(26, 167);
            this.lnkUnsplash.Name = "lnkUnsplash";
            this.lnkUnsplash.Size = new System.Drawing.Size(51, 13);
            this.lnkUnsplash.TabIndex = 5;
            this.lnkUnsplash.TabStop = true;
            this.lnkUnsplash.Text = "Unsplash";
            this.lnkUnsplash.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUnsplash_LinkClicked);
            // 
            // lnkPexels
            // 
            this.lnkPexels.AutoSize = true;
            this.lnkPexels.Location = new System.Drawing.Point(26, 196);
            this.lnkPexels.Name = "lnkPexels";
            this.lnkPexels.Size = new System.Drawing.Size(38, 13);
            this.lnkPexels.TabIndex = 6;
            this.lnkPexels.TabStop = true;
            this.lnkPexels.Text = "Pexels";
            this.lnkPexels.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPexels_LinkClicked);
            // 
            // lnkHDWallpapers
            // 
            this.lnkHDWallpapers.AutoSize = true;
            this.lnkHDWallpapers.Location = new System.Drawing.Point(26, 225);
            this.lnkHDWallpapers.Name = "lnkHDWallpapers";
            this.lnkHDWallpapers.Size = new System.Drawing.Size(79, 13);
            this.lnkHDWallpapers.TabIndex = 7;
            this.lnkHDWallpapers.TabStop = true;
            this.lnkHDWallpapers.Text = "HD Wallpapers";
            this.lnkHDWallpapers.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHDWallpapers_LinkClicked);
            // 
            // lnkWallpapersWide
            // 
            this.lnkWallpapersWide.AutoSize = true;
            this.lnkWallpapersWide.Location = new System.Drawing.Point(26, 254);
            this.lnkWallpapersWide.Name = "lnkWallpapersWide";
            this.lnkWallpapersWide.Size = new System.Drawing.Size(85, 13);
            this.lnkWallpapersWide.TabIndex = 8;
            this.lnkWallpapersWide.TabStop = true;
            this.lnkWallpapersWide.Text = "WallpapersWide";
            this.lnkWallpapersWide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWallpapersWide_LinkClicked);
            // 
            // lnkPlanWallpaper
            // 
            this.lnkPlanWallpaper.AutoSize = true;
            this.lnkPlanWallpaper.Location = new System.Drawing.Point(26, 283);
            this.lnkPlanWallpaper.Name = "lnkPlanWallpaper";
            this.lnkPlanWallpaper.Size = new System.Drawing.Size(79, 13);
            this.lnkPlanWallpaper.TabIndex = 9;
            this.lnkPlanWallpaper.TabStop = true;
            this.lnkPlanWallpaper.Text = "Plan Wallpaper";
            this.lnkPlanWallpaper.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPlanWallpaper_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(410, 311);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // olvWebsites
            // 
            this.olvWebsites.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.olvWebsites.AllColumns.Add(this.olvColumnName);
            this.olvWebsites.AllColumns.Add(this.olvColumnLastVisit);
            this.olvWebsites.AllColumns.Add(this.olvColumnDone);
            this.olvWebsites.AllowDrop = true;
            this.olvWebsites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvWebsites.CellEditUseWholeCell = false;
            this.olvWebsites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName,
            this.olvColumnLastVisit,
            this.olvColumnDone});
            this.olvWebsites.ContextMenuStrip = this.cmWebsites;
            this.olvWebsites.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvWebsites.FullRowSelect = true;
            this.olvWebsites.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.olvWebsites.Location = new System.Drawing.Point(12, 12);
            this.olvWebsites.MultiSelect = false;
            this.olvWebsites.Name = "olvWebsites";
            this.olvWebsites.ShowGroups = false;
            this.olvWebsites.Size = new System.Drawing.Size(392, 322);
            this.olvWebsites.TabIndex = 11;
            this.olvWebsites.UseCompatibleStateImageBehavior = false;
            this.olvWebsites.View = System.Windows.Forms.View.Details;
            this.olvWebsites.VirtualMode = true;
            this.olvWebsites.SelectedIndexChanged += new System.EventHandler(this.lvWallpapers_SelectedIndexChanged);
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.FillsFreeSpace = true;
            this.olvColumnName.Text = "Website Name";
            this.olvColumnName.Width = 270;
            // 
            // olvColumnLastVisit
            // 
            this.olvColumnLastVisit.AspectName = "LastVisit";
            this.olvColumnLastVisit.Text = "Last Visit";
            this.olvColumnLastVisit.Width = 90;
            // 
            // olvColumnDone
            // 
            this.olvColumnDone.AspectName = "Done";
            this.olvColumnDone.Text = "Done";
            // 
            // cmWebsites
            // 
            this.cmWebsites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.markAsDoneToolStripMenuItem,
            this.markAsNotDoneToolStripMenuItem});
            this.cmWebsites.Name = "cmWebsites";
            this.cmWebsites.Size = new System.Drawing.Size(170, 70);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // markAsDoneToolStripMenuItem
            // 
            this.markAsDoneToolStripMenuItem.Name = "markAsDoneToolStripMenuItem";
            this.markAsDoneToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.markAsDoneToolStripMenuItem.Text = "Mark as &Done";
            this.markAsDoneToolStripMenuItem.Click += new System.EventHandler(this.markAsDoneToolStripMenuItem_Click);
            // 
            // markAsNotDoneToolStripMenuItem
            // 
            this.markAsNotDoneToolStripMenuItem.Name = "markAsNotDoneToolStripMenuItem";
            this.markAsNotDoneToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.markAsNotDoneToolStripMenuItem.Text = "Mark as &Not Done";
            this.markAsNotDoneToolStripMenuItem.Click += new System.EventHandler(this.markAsNotDoneToolStripMenuItem_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(410, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(410, 70);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Enabled = false;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Location = new System.Drawing.Point(410, 41);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 14;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(410, 216);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 15;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Location = new System.Drawing.Point(411, 187);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 16;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // frmWebsites
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 347);
            this.Controls.Add(this.olvWebsites);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lnkPlanWallpaper);
            this.Controls.Add(this.lnkWallpapersWide);
            this.Controls.Add(this.lnkHDWallpapers);
            this.Controls.Add(this.lnkPexels);
            this.Controls.Add(this.lnkUnsplash);
            this.Controls.Add(this.lnkWallpapersCraft);
            this.Controls.Add(this.lnkWallpaperAbyss);
            this.Controls.Add(this.lnkWallpaperUp);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.lnkDesktopNexus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmWebsites";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wallpaper Websites";
            this.Load += new System.EventHandler(this.frmWebsites_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvWebsites)).EndInit();
            this.cmWebsites.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkDesktopNexus;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel lnkWallpaperUp;
        private System.Windows.Forms.LinkLabel lnkWallpaperAbyss;
        private System.Windows.Forms.LinkLabel lnkWallpapersCraft;
        private System.Windows.Forms.LinkLabel lnkUnsplash;
        private System.Windows.Forms.LinkLabel lnkPexels;
        private System.Windows.Forms.LinkLabel lnkHDWallpapers;
        private System.Windows.Forms.LinkLabel lnkWallpapersWide;
        private System.Windows.Forms.LinkLabel lnkPlanWallpaper;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ContextMenuStrip cmWebsites;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsDoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsNotDoneToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private BrightIdeasSoftware.OLVColumn olvColumnLastVisit;
        private BrightIdeasSoftware.OLVColumn olvColumnDone;
        internal BrightIdeasSoftware.FastObjectListView olvWebsites;
    }
}