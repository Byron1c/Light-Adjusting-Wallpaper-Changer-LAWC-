using BrightIdeasSoftware;
using LAWC.Common;
using LAWC.Objects;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Xml;

namespace LAWC
{
    public partial class frmWebsites : Form
    {
        private readonly FrmMain parentForm;

        private int SelectedIndex = 0;

        public frmWebsites(FrmMain vParent)
        {
            InitializeComponent();

            parentForm = vParent;

            olvWebsites.MouseDoubleClick += LvWebsites_MouseDoubleClick;
            olvWebsites.Click += LvWebsites_Click;
            olvWebsites.DragEnter += DragDropEnter;
            olvWebsites.DragDrop += DragDropURL;

            this.DragEnter += DragDropEnter;
            this.DragDrop += DragDropURL;

            olvWebsites.DrawColumnHeader += LvWebsites_DrawColumnHeader;
            olvWebsites.DrawSubItem += LvWebsites_DrawSubItem;
            olvWebsites.MouseUp += LvWebsites_MouseUp;

            
        }

        private void LvWebsites_MouseUp(object sender, MouseEventArgs e)
        {
            // show context menu
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                setWebsitePopupMenu();

                this.cmWebsites.Show(olvWebsites, e.Location);

            }
        }

        private void setWebsitePopupMenu()
        {
            editToolStripMenuItem.Visible = false;
            markAsDoneToolStripMenuItem.Visible = false;
            markAsNotDoneToolStripMenuItem.Visible = false;

            if (this.olvWebsites.SelectedItem != null) //.Count > 0)
            {
                editToolStripMenuItem.Visible = true;

                if (((WebsiteInfo)(this.olvWebsites.SelectedItem.RowObject)).Done == false)
                {
                    markAsDoneToolStripMenuItem.Visible = true;
                    
                }
                else
                {
                    markAsNotDoneToolStripMenuItem.Visible = true;
                }
            }
        }

        private void DragDropEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.Text)) //DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;


            parentForm.Focus();
            parentForm.BringToFront();
            this.Focus();
            this.BringToFront();
        }
        
        private void DragDropURL(object sender, DragEventArgs e)
        {
            //string[] s = (string[])e.Data.GetData(DataFormats.Text, false);
            string url = e.Data.GetData(DataFormats.Text, false).ToString();

            if (url.ToUpper(CultureInfo.InvariantCulture).Contains("HTTP") == false) return;

            // check if its already in the Websites list
            for (int i = 0; i < parentForm.settings.Websites.Count; i++)
            {

                if (parentForm.settings.Websites[i].URL.Contains(url) == true)
                {
                    // already in the list
                    MessageBox.Show("Website is already in the list.");
                    return;
                }

            }

            frmWebsiteEdit webEdit = new frmWebsiteEdit(parentForm)
            {
                FormMode = frmWebsiteEdit.EditMode.Add
            };

            webEdit.lblLastChecked.Text = DateTime.Now.ToShortDateString();
            webEdit.txtURL.Text = url;
            webEdit.Focus();
            webEdit.ShowDialog();


            webEdit.Dispose();

            DrawWebsiteList();
            //fillWWebsiteList();


        }






        private void lnkDesktopNexus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.desktopnexus.com/"); 
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://www.deviantart.com/
            System.Diagnostics.Process.Start("https://www.deviantart.com/");
        }

        private void lnkWallpaperUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://www.wallpaperup.com
            System.Diagnostics.Process.Start("https://www.wallpaperup.com");
        }

        private void lnkWallpaperAbyss_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://wall.alphacoders.com/
            System.Diagnostics.Process.Start("https://wall.alphacoders.com/");
        }

        private void lnkWallpapersCraft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://wallpaperscraft.com/
            System.Diagnostics.Process.Start("https://wallpaperscraft.com/");
        }

        private void lnkUnsplash_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://unsplash.com/wallpaper
            System.Diagnostics.Process.Start("https://unsplash.com/wallpaper");
        }

        private void lnkPexels_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://www.pexels.com/
            System.Diagnostics.Process.Start("https://www.pexels.com/");
        }

        private void lnkHDWallpapers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://www.hdwallpapers.in/
            System.Diagnostics.Process.Start("https://www.hdwallpapers.in/");
        }

        private void lnkWallpapersWide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //http://wallpaperswide.com/latest_wallpapers.html
            System.Diagnostics.Process.Start("http://wallpaperswide.com/latest_wallpapers.html");
        }

        private void lnkPlanWallpaper_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://www.planwallpaper.com
            System.Diagnostics.Process.Start("https://www.planwallpaper.com");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void LvWebsites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (olvWebsites.SelectedItem != null)// .Count > 0)
            {
                //System.Diagnostics.Process.Start(((WebsiteInfo)olvWebsites.SelectedItems[0].Tag).URL);
                System.Diagnostics.Process.Start(((WebsiteInfo)(this.olvWebsites.SelectedItem.RowObject)).URL);

                ((WebsiteInfo)(this.olvWebsites.SelectedItem.RowObject)).LastVisited = DateTime.Now;

                parentForm.SaveSettings(string.Empty, this.parentForm.settings);
            }
        }

        private void LvWebsites_Click(object sender, EventArgs e)
        {
            SelectWebsite();
        }

        private void SelectWebsite()
        {
            if (this.olvWebsites.SelectedItem == null) //ddolvWebsites.SelectedItems.Count == 0)
            {
                SelectedIndex = 0;
                setButtons();
                return;
            }

            // get index of entry for selected item
            for (int i = 0; i < parentForm.settings.Websites.Count; i++)
            {
                if (parentForm.settings.Websites[i].Name == ((WebsiteInfo)olvWebsites.SelectedItem.RowObject).Name)
                {
                    SelectedIndex = i;
                    break;
                }


            }

            setButtons();


        }

        private void setButtons()
        {
            if (SelectedIndex <= olvWebsites.Items.Count && olvWebsites.SelectedItem != null) // .Count > 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void Edit()
        {
            if (SelectedIndex <= olvWebsites.Items.Count && olvWebsites.SelectedItem != null) //s.Count > 0)
            {
                //// get index of entry for editing
                //for (int i = 0; i < lvWebsites.Items.Count; i++)
                //{
                //    if (((WebsiteInfo)lvWebsites.Items[i].Tag).Name == ((WebsiteInfo)lvWebsites.SelectedItems[0].Tag).Name)
                //    {
                //        SelectedIndex = i;
                //        break;
                //    }


                //}

                // fill in form
                frmWebsiteEdit frmEdit = new frmWebsiteEdit(parentForm)
                {
                    Editndex = SelectedIndex,
                    FormMode = frmWebsiteEdit.EditMode.Edit
                };
                frmEdit.txtName.Text = ((WebsiteInfo)olvWebsites.SelectedItem.RowObject).Name;
                frmEdit.txtURL.Text = ((WebsiteInfo)olvWebsites.SelectedItem.RowObject).URL;
                frmEdit.lblLastChecked.Text = ((WebsiteInfo)olvWebsites.SelectedItem.RowObject).LastVisited.ToShortDateString();
                frmEdit.cbDone.Checked = ((WebsiteInfo)olvWebsites.SelectedItem.RowObject).Done;
                frmEdit.Focus();
                frmEdit.ShowDialog();

                //Setting.SaveSettings(string.Empty, this.parentForm.settings);
                parentForm.SaveSettings(string.Empty, this.parentForm.settings);

                frmEdit.Dispose();

                DrawWebsiteList();
                //fillWWebsiteList();


            }
        }



        private void adjustMyObjectListViewHeader()
        {
            foreach (OLVColumn item in olvWebsites.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(parentForm.colourDarker);
                headerstyle.SetForeColor(parentForm.colourLightest);
                item.HeaderFormatStyle = headerstyle;
            }

            foreach (OLVColumn item in olvWebsites.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(parentForm.colourDarker);
                headerstyle.SetForeColor(parentForm.colourLightest);
                item.HeaderFormatStyle = headerstyle;
            }
        }

        internal void DrawWebsiteList()
        {

            adjustMyObjectListViewHeader();

            //olvFolders.AlternateRowBackColor = colourDarker;
            //olvFolders.BackColor = colourMedium;// colourDark;
            //olvFolders.ForeColor = colourLightest;
            olvWebsites.CheckBoxes = false;
            //this.olvImages.UseOverlays = true;
            //this.olvFolders.UseOverlays = true;

            parentForm.settings.Websites.Sort(WebsiteInfo.CompareByName);

            this.olvWebsites.SetObjects(parentForm.settings.Websites);

            //SetColumnWidthsFolders();

            //HideChangeWallpaperWorking(); // HideWorkingMessage();

        }

        internal void fillWWebsiteListOLD()
        {
            //Color currentBackColour = Color.Silver;
            //Color backColour1 = Color.Silver;
            //Color backColour2 = Color.White;

            System.Drawing.Color backColourDisabled = parentForm.colourDarkest; //GetCurrentInterfaceColour(InterfaceColours.darkest, screenStateCurrent, GetPercentThroughChange());// getScreenState(DateTime.Now)); //Color.LightGray;
            System.Drawing.Color backColour1 = parentForm.colourDarker; //GetCurrentInterfaceColour(InterfaceColours.darker, screenStateCurrent, GetPercentThroughChange());// getScreenState(DateTime.Now)); //Color.LightGray;
            System.Drawing.Color backColour2 = parentForm.colourDark; // GetCurrentInterfaceColour(InterfaceColours.dark, screenStateCurrent, GetPercentThroughChange());// getScreenState(DateTime.Now)); //Color.LightGray;
            System.Drawing.Color currentBackColour = backColour1;//Color.White;
            System.Drawing.Color currentForeColourEnabled = Color.GhostWhite; //getCurrentInterfaceColour(InterfaceColours.lightest, screenStateCurrent, getPercentThroughChange());// getScreenState(DateTime.Now));//Color.Black;
            System.Drawing.Color currentForeColourDisabled = parentForm.colourDark;//GetCurrentInterfaceColour(InterfaceColours.dark, screenStateCurrent, GetPercentThroughChange());// getScreenState(DateTime.Now));//Color.Black;
            System.Drawing.Color currentForeColour = parentForm.colourLightest; //Color.GhostWhite; //currentForeColourEnabled;

            if (parentForm.settings.AdjustInterfaceColour == false)
            {
                backColourDisabled = Color.Gray;
                backColour1 = Color.LightGray;
                backColour2 = Color.WhiteSmoke;
                currentBackColour = backColour1;

                currentForeColourEnabled = Color.Black;
                currentForeColourDisabled = Color.Silver;
                currentForeColour = currentForeColourEnabled;
            }



            olvWebsites.BeginUpdate();

            olvWebsites.Items.Clear();
            olvWebsites.Columns.Clear();
            olvWebsites.Columns.Add("Name", "Name", 200);
            olvWebsites.Columns.Add("LastVisit", "Last Visit", 140);
            olvWebsites.Columns.Add("Done", "Done", 40);


            string[] fullItemText = new string[3]; // 1 = number of columns

            /* sorting */
            parentForm.settings.Websites.Sort(WebsiteInfo.CompareByName);


            for (int i = 0; i < parentForm.settings.Websites.Count; i++)
            {

                fullItemText[0] = parentForm.settings.Websites[i].Name.ToString(CultureInfo.InvariantCulture);
                if (parentForm.settings.Websites[i].LastVisited != MainFunctions.DateNull)
                {
                    fullItemText[1] = parentForm.settings.Websites[i].LastVisited.ToString(CultureInfo.InvariantCulture);
                } else { fullItemText[1] = string.Empty; }
                if (parentForm.settings.Websites[i].Done)
                {
                    fullItemText[2] = "Yes"; // parentForm.settings.Websites[i].Done.ToString(CultureInfo.InvariantCulture);
                    fullItemText[0] = "[Done] " + fullItemText[0];
                } else {
                    fullItemText[2] = "...";
                }
                //if (parentForm.settings.Websites[i].LastVisited != MainFunctions.DateNull)
                //{
                //    fullItemText[1] = parentForm.settings.Websites[i].LastVisited.ToString(CultureInfo.InvariantCulture);
                //}
                //if (parentForm.settings.Websites[i].Done)
                //{
                //    fullItemText[2] = "Yes"; // parentForm.settings.Websites[i].Done.ToString(CultureInfo.InvariantCulture);
                //}

                //if (parentForm.settings.Websites[i].Done == true)
                //{
                //    fullItemText[0] = "[Done] " + fullItemText[0];
                //}



                ListViewItem item = new ListViewItem(fullItemText)
                {
                    ToolTipText = parentForm.settings.Websites[i].URL.ToString(CultureInfo.InvariantCulture),
                    Tag = (WebsiteInfo)parentForm.settings.Websites[i],

                    BackColor = currentBackColour
                };
                if (currentBackColour == backColour2)
                {
                    currentBackColour = backColour1;
                }
                else
                {
                    currentBackColour = backColour2;
                }
                
                this.olvWebsites.Items.Add(item);

            }


            olvWebsites.EndUpdate();

            //setColumnWidthsFolders();

            olvWebsites.Refresh();

        }

        private void lvWallpapers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectWebsite();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmWebsiteEdit webEdit = new frmWebsiteEdit(parentForm)
            {
                FormMode = frmWebsiteEdit.EditMode.Add
            };

            webEdit.lblLastChecked.Text = DateTime.Now.ToShortDateString();

            webEdit.ShowDialog();

            webEdit.Dispose();

            DrawWebsiteList();
            //fillWWebsiteList();
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedIndex <= olvWebsites.Items.Count && olvWebsites.SelectedItem != null) //.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this entry?" + System.Environment.NewLine + olvWebsites.Items[SelectedIndex].ToolTipText + "", "Delete Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.parentForm.settings.Websites.RemoveAt(SelectedIndex);

                    //Setting.SaveSettings(string.Empty, this.parentForm.settings);
                    parentForm.SaveSettings(string.Empty, this.parentForm.settings);

                    DrawWebsiteList();
                    //fillWWebsiteList();
                }
            }                
        }


        private String GetFolder()
        {
            String output = "";

            FolderBrowserDialog FolderBrowserDialog1 = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.Desktop
            };
            if (!string.IsNullOrEmpty(parentForm.settings.LastFolderPath)) FolderBrowserDialog1.SelectedPath = parentForm.settings.LastFolderPath;

            FolderBrowserDialog1.Description = "Select the Output folder";

            if (FolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    // ASSUME a UNC path
                    //txtSource.Text = parentForm.GetFullPath(FolderBrowserDialog1.SelectedPath.ToString(CultureInfo.InvariantCulture));
                    output = FolderBrowserDialog1.SelectedPath.ToString(CultureInfo.InvariantCulture);
                    parentForm.settings.LastFolderPath = FolderBrowserDialog1.SelectedPath;
                }
                catch (IOException)
                {
                    //output = FolderBrowserDialog1.SelectedPath.ToString(CultureInfo.InvariantCulture);
                    MessageBox.Show("Try a different folder");
                }

                if (output.Substring(output.Length - 1, 1) != "\\")
                {
                    output += "\\";
                }
            }

            return output;
        }


        private static String GetFile()
        {
            String output = "";

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog
            {

                //OpenFileDialog1 RootFolder = Environment.SpecialFolder.Desktop;


                Title = "Select the Wallpaper list XML file",
                Filter = "XML Files|*.xml"
            };

            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    // ASSUME a UNC path
                    //txtSource.Text = parentForm.GetFullPath(FolderBrowserDialog1.SelectedPath.ToString(CultureInfo.InvariantCulture));
                    output = OpenFileDialog1.FileName .ToString(CultureInfo.InvariantCulture);
                }
                catch (IOException)
                {
                    //output = FolderBrowserDialog1.SelectedPath.ToString(CultureInfo.InvariantCulture);
                    MessageBox.Show("Try a different file");
                }

                
            }

            return output;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

            String filePath = GetFolder() + MainFunctions.WallpaperWebsitesFilenameOnly;

            if (exportWebsitesToXML(filePath, parentForm.settings) == true)
            {
                MessageBox.Show("Website list successfully Exported to " + filePath);
            }
            else
            {
                MessageBox.Show("Problem Exporting to " + filePath);
            }

           
        }

        private Boolean exportWebsitesToXML(String vFilePath, Setting vSetting)
        {
            Boolean output;// = false;
            DataSet dsMain = new DataSet();
            DataTable dtWebsites = new DataTable();

            DataRow dr;

            dtWebsites.TableName = "Websites";
            dtWebsites.Columns.Add("URL", System.Type.GetType("System.String"));
            dtWebsites.Columns.Add("LastVisited", System.Type.GetType("System.String"));
            dtWebsites.Columns.Add("Done", System.Type.GetType("System.Boolean"));
            dtWebsites.Columns.Add("Name", System.Type.GetType("System.String"));

            dsMain.Tables.Add(dtWebsites);


            for (int i = 0; i < vSetting.Websites.Count; i++)
            {
                dr = dsMain.Tables["Websites"].NewRow();

                dr["URL"] = vSetting.Websites[i].URL;
                dr["LastVisited"] = vSetting.Websites[i].LastVisited.ToString(CultureInfo.InvariantCulture);
                dr["Done"] = vSetting.Websites[i].Done;
                dr["Name"] = vSetting.Websites[i].Name;

                dtWebsites.Rows.Add(dr);

            }

            //// WRITE TO FILE
            try
            {
                if (!Directory.Exists(vFilePath))
                {
                    string path = System.IO.Path.GetDirectoryName(vFilePath);
                    System.IO.Directory.CreateDirectory(path);
                }
                dsMain.WriteXml(vFilePath, XmlWriteMode.WriteSchema);
                output = true;

            }
            catch (IOException ex)
            {
                //MessageBox.Show("Error Exporting the file: \n\n" + ex.Message);
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Error exporting Websites \n\n" + ex.Message, vSetting.WriteToLog);
                ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.UnknownDataWrite, true, false, String.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
                output = false;
            }


            dtWebsites.Clear();
            dsMain.Tables.Clear();
            dsMain.Clear();
            dtWebsites.Dispose();
            dsMain.Dispose();

            return output;

        }


        internal void importWebsites(String vFilePath)
        {
            DataSet dsWebsites = ImportWebsites(vFilePath, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

            DataRow dr;

            try
            {

                parentForm.settings.Websites.Clear();

                if (dsWebsites.Tables["Websites"] != null)
                {

                    for (int i = 0; i < dsWebsites.Tables["Websites"].Rows.Count; i++)
                    {
                        dr = dsWebsites.Tables["Websites"].Rows[i];

                        String itemURL = dr["URL"].ToString();
                        DateTime itemDT = MainFunctions.DateNull; //DateTime.Parse(dr["LastVisited"].ToString());
                        Boolean itemDone = Boolean.Parse(dr["Done"].ToString());
                        String itemName = dr["Name"].ToString();

                        WebsiteInfo item = new WebsiteInfo
                        {
                            URL = itemURL,
                            LastVisited = itemDT, // DateTime.Now; 
                            Done = itemDone,
                            Name = itemName
                        };

                        if (parentForm.settings.Websites.Count == 0)
                        {
                            // No Entries - add the first one by default
                            parentForm.settings.Websites.Add(item);
                        }
                        else
                        {
                            for (int y = 0; y < parentForm.settings.Websites.Count; y++)
                            {
                                // if the url is already present then dont add it again
                                if (parentForm.settings.Websites[y].URL.Contains(item.URL) == false)
                                {
                                    parentForm.settings.Websites.Add(item);
                                    break;
                                }
                            }
                        }


                    }

                } // end if

            }
            catch (IOException ex)
            {
                ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.UnknownDataRead, true, false, string.Format(CultureInfo.InvariantCulture, ""), Setting.getSettingsFullPath(Properties.Settings.Default.Portable));

                //MessageBox.Show("Unknown Data Found: " + ex.Message);
                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Unknown data Found: " + ex.Message, true);

            }


        }

        internal void startImport()
        {

            String filePath = GetFile();// + MainFunctions.WallpaperWebsitesFilenameOnly;
            importWebsites(filePath);

            //Setting.SaveSettings(string.Empty, this.parentForm.settings);
            parentForm.SaveSettings(string.Empty, this.parentForm.settings);
            MessageBox.Show("Imported " + parentForm.settings.Websites.Count + " wallpaper Websites.");
            DrawWebsiteList();
            //fillWWebsiteList();

        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            startImport();
        }





        internal static DataSet ImportWebsites(String vFilePath, String vSettingsFullPath)
        {
            //String settingsFullPath = MainFunctions.GetSettingsFullPath(); // Application.StartupPath + "\\" + vSettingsFilename; // read file from app path
            DataSet output = new DataSet();

            if (System.IO.File.Exists(vFilePath))
            {
                try
                {
                    //XmlReadMode mode = output.ReadXml(vFilePath, XmlReadMode.ReadSchema);

                    TextReader tr = new StreamReader(vFilePath);
                    XmlTextReader reader = new XmlTextReader(tr) { DtdProcessing = DtdProcessing.Prohibit };

                    XmlReadMode mode = output.ReadXml(reader, XmlReadMode.ReadSchema);



                    if (mode != XmlReadMode.ReadSchema)
                    {
                        ErrorHandling.ProcessError(null, ErrorHandling.ErrorMessageType.XMLFileRead, true, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                    }
                }
                catch (IOException ex)
                {
                    //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Error 008: Website List file is corrupt.", true);
                    //MessageBox.Show("Unable to read the XML file.\n\nError: " + ex.Message, "Import Websites Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ErrorHandling.ProcessError(ex, ErrorHandling.ErrorMessageType.XMLFileRead, true, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                    //throw;
                }
            }
            else
            {
                ErrorHandling.ProcessError(null, ErrorHandling.ErrorMessageType.WebsiteListNotFound, true, false, string.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);

                //FileFunctions.WriteToLog(MainFunctions.GetErrorLogFullPath(Properties.Settings.Default.Portable), "Warning 001: Website List file not found", true);
                //MessageBox.Show("This is your first time running LAWC.\n\n  A new settings file will be created (" + settingsFullPath + ").", "Settings File Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return output;

        }

        private void frmWebsites_Load(object sender, EventArgs e)
        {
            
        }

        private void LvWebsites_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            parentForm.DrawSubItem(sender, e);
        }

        private void LvWebsites_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            parentForm.DrawColumnHeader(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void markAsDoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (olvWebsites.SelectedItem != null) //s.Count == 0) return;

            ((WebsiteInfo)(olvWebsites.SelectedItem.RowObject)).Done = true;
            ((WebsiteInfo)(olvWebsites.SelectedItem.RowObject)).LastVisited = DateTime.Now;
            setWebsitePopupMenu();
            DrawWebsiteList();
            //fillWWebsiteList();
        }

        private void markAsNotDoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (olvWebsites.SelectedItem != null) //s.Count == 0) return;
            ((WebsiteInfo)(olvWebsites.SelectedItem.RowObject)).Done = false;
            ((WebsiteInfo)(olvWebsites.SelectedItem.RowObject)).LastVisited = MainFunctions.DateNull;
            setWebsitePopupMenu();
            DrawWebsiteList();
            //fillWWebsiteList();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }


}
