using LAWC.Common;
using LAWC.Objects;
using OpenHardwareMonitor.Hardware;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using static LAWC.Common.ErrorHandling;
using static LAWC.FrmMain;
using static LAWC.Objects.EventInfo;
using static LAWC.Objects.Sensor;

namespace LAWC
{
    public partial class frmEvent : Form
    {

        private readonly frmSettingsAdvanced parentForm;

        internal enum FormMode
        {
            Add,
            Edit,
        }
        internal FormMode formMode = FormMode.Add;

        internal int EventIndex = -1;

        private String imagePath;
        public String ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        internal DateTime lastRun = new DateTime(1980, 1, 1);
        internal Boolean applyingSettings = false;


        internal frmEvent(frmSettingsAdvanced vParent, FormMode vMode)
        {
            InitializeComponent();
            formMode = vMode;
            parentForm = vParent;
            EventIndex = -1;
            ImagePath = string.Empty;
        }

        // For Editing:
        internal frmEvent(frmSettingsAdvanced vParent, FormMode vMode, int vIndex)
        {
            InitializeComponent();
            formMode = vMode;
            parentForm = vParent;
            EventIndex = vIndex;
            ImagePath = string.Empty;
        }


        internal int getLowestOrderPos()
        {            
            int index = 0;

            for (int i = 0; i <= parentForm.parentForm.settings.Events.Count(); i++)
            {
                index = parentForm.parentForm.settings.Events.FindIndex(r =>  i == r.OrderPos);
                if (index == -1)
                {
                    // first available pos
                    return i;
                }
            }

            return 0;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEvent_Load(object sender, EventArgs e)
        {
            //this.formMode = FormMode.Add; // default mode

            //ImagePath = string.Empty;

            pbImage.MouseClick += PbImage_MouseClick;

            //pnlFontColour.MouseDoubleClick += PnlFontColour_MouseDoubleClick;
            pnlFontColour.MouseUp += PnlFontColour_MouseUp;

            numCheckSeconds.KeyUp += NumCheckSeconds_KeyUp;
            numCheckSeconds.LostFocus += NumCheckSeconds_LostFocus;

            lblCheckSecondsWarning.MouseUp += LblCheckSecondsWarning_MouseUp;
            btnAddSpecialText.MouseUp += BtnAddSpecialText_MouseUp;

            setLabels(!string.IsNullOrEmpty(this.ImagePath));

            setColours();
            setCheckSeconds(true);
        }

        private void setColours()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name.Contains("Colour") == false)
                {
                    c.BackColor = parentForm.parentForm.colourDarker; // colourDarkest;
                    c.ForeColor = parentForm.parentForm.colourLightest;
                }
            }
            this.BackColor = parentForm.parentForm.colourDarker; // colourDarkest;
            this.ForeColor = parentForm.parentForm.colourLightest;
        }

        private void BtnAddSpecialText_MouseUp(object sender, MouseEventArgs e)
        {
            // show context menu
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (cbSensor.SelectedItem != null)
                {
                    this.cmSpecialCharacters.Show(btnAddSpecialText, e.Location);
                }
                else
                {
                    this.cmSelectSensor.Show(btnAddSpecialText, e.Location);
                }                
            }
        }

        private void LblCheckSecondsWarning_MouseUp(object sender, MouseEventArgs e)
        {
            // show context menu
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.cmCheckSeconds.Show(lblCheckSecondsWarning, e.Location);

            }
        }

        private void NumCheckSeconds_LostFocus(object sender, EventArgs e)
        {
            setCheckSeconds(true);
        }


        private void NumCheckSeconds_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setCheckSeconds(true);
            }
            else
            {
                setCheckSeconds(false);
            }

        }


        private void PnlFontColour_MouseUp(object sender, MouseEventArgs e)
        {
            PickFontColour();
        }


        internal void PickFontColour()
        {
            System.Windows.Forms.ColorDialog colorPicker = new ColorDialog
            {
                // Allows the user to get help. (The default is false.)
                ShowHelp = true,
                // Sets the initial color select to the current text color.
                Color = pnlFontColour.BackColor
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                pnlFontColour.BackColor = colorPicker.Color;

            }
            colorPicker.Dispose();
        }


        private void PbImage_MouseClick(object sender, MouseEventArgs e)
        {
            GetEventImage();
        }

        private void GetEventImage()
        {
            ImagePath = getImagePath();
            loadEventImage(ImagePath);

        }

        /// <summary>
        /// Loads an image into the picturebox, and adjusts the display of the labels accordingly
        /// </summary>
        /// <param name="vImagePath"></param>
        internal void loadEventImage(String vImagePath)
        {

            if (string.IsNullOrEmpty(vImagePath))
            {
                pbImage.InitialImage = null;
                lblClickImage.Visible = true;
                lblClearImage.Visible = false;

                cbOverride.Checked = false;
                cbOverride.Enabled = false;
                return;
            }

            Boolean result = ImageFunctions.loadImageToPictureBox(pbImage, vImagePath);

            setLabels(result);

            //if (ImageFunctions.loadImageToPictureBox(pbImage, vImagePath))
            //{
            //    lblClearImage.Visible = true;
            //    lblClickImage.Visible = false;

            //    cbOverride.Checked = true;
            //    cbOverride.Enabled = true;
            //}
            //else
            //{
            //    lblClearImage.Visible = false;
            //    lblClickImage.Visible = true;

            //    cbOverride.Checked = false;
            //    cbOverride.Enabled = false;
            //}

        }

        internal void setLabels(Boolean vImageIsSet)
        {
            //if (vImagePath == string.Empty)
            //{
            //    pbImage.InitialImage = null;
            //    lblClickImage.Visible = true;
            //    lblClearImage.Visible = false;

            //    cbOverride.Checked = false;
            //    cbOverride.Enabled = false;
            //    return;
            //}

            if (vImageIsSet == true)
            {
                lblClearImage.Visible = true;
                lblClickImage.Visible = false;
                lblDisplayAlways.Visible = false;
                cbOverride.Checked = true;
                cbOverride.Enabled = true;
            }
            else
            {
                if (cbCheckAction.SelectedItem != null && EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString()) == CheckActionType.DisplayAlways)
                {
                    // display always
                    lblClearImage.Visible = false;
                    lblClickImage.Visible = false;
                    lblDisplayAlways.Visible = true;
                    cbOverride.Checked = false;
                    cbOverride.Enabled = false;
                } else
                {
                    lblClearImage.Visible = false;
                    lblClickImage.Visible = true;
                    lblDisplayAlways.Visible = false;
                    cbOverride.Checked = false;
                    cbOverride.Enabled = false;
                }
                
            }
        }


        private void btnOkay_Click(object sender, EventArgs e)
        {
            Boolean success = false;

            switch (formMode)
            {
                case FormMode.Add:
                    success = addEvent();
                    break;
                case FormMode.Edit:
                    success = saveEvent();
                    break;
                default:
                    break;
            }            

            // fire all events to re-set messages after the changes above (add/save)
            parentForm.parentForm.ProcessAllEvents();

            //refresh the event list (lastrun time may have changed)
            parentForm.DrawEventList();// ShowEventList();

            if (success == true) this.Hide();
        }

        internal void fillSensorList(String vName, String vType, string vSelected)
        {
            applyingSettings = true;

            cbSensor.BeginUpdate();

            cbSensor.Items.Clear();
            ComboboxItemCustom cbNone = new ComboboxItemCustom
            {
                Name = "None",
                Text = "None",
                Value = -1
            };
            cbSensor.Items.Add(cbNone);
            if (vSelected == "None")
            {
                cbSensor.SelectedItem = cbNone;
            }

            foreach (SensorSummary s in parentForm.parentForm.HWSensors)
            {
                int index = 0;
                ComboboxItemCustom cb = new ComboboxItemCustom();
                String name = s.Name.Replace(@":\", "");
                cb.Text = s.Category + " - " + name;
               
                //if (s.Type.ToString(CultureInfo.InvariantCulture).Contains("Temperature"))
                //{
                //    cb.Text += " (" + s.Type + ")";
                //}
                cb.Name = s.Name;
                cb.Value = s.DataType;
                
                if (s.Category == SensorSource.Hardware)
                {
                    cb.Text += " (" + s.DataType + ")";
                }

                index = cbSensor.Items.Add(cb);
                if (!string.IsNullOrEmpty(vName)&& !string.IsNullOrEmpty(vType))
                {
                    if (cb.Name == vName && cb.Value.ToString() == vType)
                    {
                        cbSensor.SelectedItem = cb;
                    }
                }
            }

            cbSensor.EndUpdate();
            applyingSettings = false;
            
        }

        ///// <summary>
        ///// Make sure the logic statements in this match those in isFormValid()
        ///// </summary>
        ///// <param name="vIndex"></param>
        ///// <param name="vMessage"></param>
        ///// <returns></returns>
        //private Boolean isEventValid(int vIndex, ref String vMessage)
        //{
        //    Boolean output = true;
        //    vMessage = String.Empty;

        //    if (parentForm.parentForm.settings.Events[vIndex].Enabled == true) // only validate these controls if the user wants the event to be Enabled
        //    {
        //        if (parentForm.parentForm.settings.Events[vIndex].CheckAction == EventInfo.CheckActionType.None)
        //        {
        //            output = false;
        //            vMessage += " * Select an Action for this event.\n";
        //        }
        //        //else if (cbCheckAction.SelectedItem.ToString(CultureInfo.InvariantCulture) == "None")
        //        //{
        //        //    output = false;
        //        //    vMessage += " * Select an Action for this event.\n";
        //        //}



        //        if (parentForm.parentForm.settings.Events[vIndex].SensorName == null)
        //        {
        //            output = false;
        //            vMessage += " * Select the Type for this event.\n";
        //        }
        //        else if (parentForm.parentForm.settings.Events[vIndex].SensorName == "None")
        //        {
        //            output = false;
        //            vMessage += " * Select the Type for this event.\n";
        //        }
        //        if (parentForm.parentForm.settings.Events[vIndex].CheckSeconds < 2)
        //        {
        //            output = false;
        //            vMessage += " * Enter the number of seconds for how often you want this event to check (> 2).\n";
        //        }

        //        if (parentForm.parentForm.settings.Events[vIndex].ImagePath == string.Empty 
        //            && parentForm.parentForm.settings.Events[vIndex].Message.Trim() == string.Empty)
        //        {
        //            output = false;
        //            vMessage += " * Please enter a Message, or select an Image for this event.\n";
        //        }

        //        // check the value if we are calculating with it, not if its set to Display Always
        //        if (parentForm.parentForm.settings.Events[vIndex].CheckAction != EventInfo.CheckActionType.DisplayAlways)  // not 
        //        {
        //            ComboboxItemCustom selectedItem = (ComboboxItemCustom)cbSensor.SelectedItem;
        //            if ((SensorType)selectedItem.Value == SensorType.SmallData
        //                    && (selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper() == "SUNRISE"
        //                    || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper() == "SUNSET"
        //                    || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper() == "CLOUDS"
        //                    || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper() == "WINDDIRECTION")
        //                    )
        //            {
        //                // if we have selected a sunrise/sunset sensor
        //                // cant have a "blank" entry, and the control handles validation

        //            }
        //            else
        //            {
        //                // default, anything not covered above
        //                if (this.numValue.ToString(CultureInfo.InvariantCulture) == string.Empty)
        //                {
        //                    output = false;
        //                    vMessage += " * Please enter a value to be checked for this event.\n";
        //                }

        //            }

        //        }                 
        //    }
        //    return output;
        //}


        /// <summary>
        /// Make sure the logic statements in this match those in isEventValid()
        /// </summary>
        /// <param name="vMessage"></param>
        /// <returns></returns>
        private Boolean isFormValid(ref String vMessage)
        {
            Boolean output = true;
            vMessage = String.Empty;

            //if (EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString(CultureInfo.InvariantCulture)) == EventInfo.CheckActionType.None)
            //{
            //    output = false;
            //    vMessage += " * Select an Action for this event.\n";
            //}

            if (cbEnabled.Checked == true) // only validate these controls if the user wants the event to be Enabled
            {
                if (cbCheckAction.Text == null) //selecteditem   // || cbCheckAction.SelectedText == string.Empty)
                {
                    output = false;
                    vMessage += " * Select an Action for this event.\n";
                }
                else if (cbCheckAction.Text.ToString(CultureInfo.InvariantCulture) == "None")  //selecteditem
                {
                    output = false;
                    vMessage += " * Select an Action for this event.\n";
                }
                //(int)numCheckSeconds.Value
                if (cbSensor.SelectedItem == null)
                {
                    output = false;
                    vMessage += " * Select the Sensor for this event.\n";
                }
                //else if (cbSensor.SelectedItem.ToString() == "None")
                //{
                //    output = false;
                //    vMessage += " * Select the Type for this event.\n";
                //}

                if ((int)numCheckSeconds.Value < 5)
                {
                    output = false;
                    vMessage += " * Enter the number of seconds for how often you want this event to check (> 5).\n";
                }

                if (string.IsNullOrEmpty(this.ImagePath) && string.IsNullOrEmpty(txtMessage.Text.Trim()))
                {
                    output = false;
                    vMessage += " * Please enter a Message, or select an Image for this event.\n";
                }


                if (cbCheckAction.SelectedItem != null)
                {
                    // check the value if we are calculating with it, not if its set to Display Always
                    if (EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString()) != EventInfo.CheckActionType.DisplayAlways)  // not equal
                    {
                        ComboboxItemCustom selectedItem = (ComboboxItemCustom)cbSensor.SelectedItem;
                        if ((SensorType)selectedItem.Value == SensorType.SmallData
                                && (selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                                || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                                || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                                || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION")
                                ))
                        {
                            // if we have selected a sunrise/sunset sensor
                            // cant have a "blank" entry, and the control handles validation

                        }
                        else
                        {
                            // default, anything not covered above
                            if (string.IsNullOrEmpty(this.numValue.ToString()))
                            {
                                output = false;
                                vMessage += " * Please enter a value to be checked for this event.\n";
                            }
                        }
                    }

                }
                //else
                //{
                //    output = false;
                //    vMessage += " * Please select an Action for this Event.\n";
                //}

                

                //numValue.Value;
                //cbEnabled.Checked;
                //if (this.ImagePath != null) 
                //{
                //    if (this.ImagePath == string.Empty) {
                //        output = false;
                //        vMessage += " * Select an image for this event. Click on the picture box to choose one.\n";
                //    }

                //} else
                //{
                //    //output = false;
                //    //vMessage += " * Select an image for this event. Click on the picture box to choose one.\n";
                //}
                //if (txtMessage.Text.ToString(CultureInfo.InvariantCulture) == string.Empty)
                //{
                //    output = false;
                //    vMessage += " * .\n";
                //}

                //cbOverride.Checked; 
            }


            return output;

        }

        private Boolean saveEvent()
        {
            String message = string.Empty;
            Boolean output = false;

            if (EventIndex == -1)
            {
                MessageBox.Show("There was a problem trying to save the edited event.");
                return output;
            }

            if (isFormValid(ref message) == false)
            {
                MessageBox.Show("There were problems saving this edited Event:\n" + message + "\n");
                return output;
            }

            if (EventIndex < parentForm.parentForm.settings.Events.Count())
            {
                if (cbCheckAction.SelectedItem != null)
                    parentForm.parentForm.settings.Events[EventIndex].CheckAction = EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString());
                parentForm.parentForm.settings.Events[EventIndex].CheckSeconds = (int)numCheckSeconds.Value;
                parentForm.parentForm.settings.Events[EventIndex].CheckValueDecimal = numValue.Value;
                parentForm.parentForm.settings.Events[EventIndex].CheckValueString = txtValue.Text;
                parentForm.parentForm.settings.Events[EventIndex].Enabled = cbEnabled.Checked;
                parentForm.parentForm.settings.Events[EventIndex].ImagePath = this.ImagePath;
                parentForm.parentForm.settings.Events[EventIndex].Message = txtMessage.Text.ToString(CultureInfo.InvariantCulture);
                parentForm.parentForm.settings.Events[EventIndex].OverrideWallpaper = cbOverride.Checked;
                parentForm.parentForm.settings.Events[EventIndex].ShowNotification = cbShowNotification.Checked;
                parentForm.parentForm.settings.Events[EventIndex].FontSize = (float)numFontSize.Value;
                parentForm.parentForm.settings.Events[EventIndex].FontColour = pnlFontColour.BackColor;
                parentForm.parentForm.settings.Events[EventIndex].LastRun = lastRun;
                //parentForm.parentForm.settings.Events[EventIndex].Displayed = false;
                parentForm.parentForm.settings.Events[EventIndex].OrderPos = (int)numOrderPos.Value;
                parentForm.parentForm.settings.Events[EventIndex].Transparency = (int)numTransparent.Value;
                
                if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
                {
                    parentForm.parentForm.settings.Events[EventIndex].CheckValueString = cbAvailable.Checked.ToString(CultureInfo.InvariantCulture);
                }                
                if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
                {
                    parentForm.parentForm.settings.Events[EventIndex].CheckValueString = txtValue.Text.ToString(CultureInfo.InvariantCulture);
                    parentForm.parentForm.settings.Events[EventIndex].CheckValueDecimal = numValue.Value; // (cbAvailable.Checked ? 1 : 0);
                }

                ComboboxItemCustom cb = (ComboboxItemCustom)cbSensor.SelectedItem;
                if (cbSensor.SelectedItem != null)
                {
                    parentForm.parentForm.settings.Events[EventIndex].TypeOfEvent = cb.Value.ToString(); //  EventInfo.StringToCheckEventType(cbSensor.SelectedItem.ToString(CultureInfo.InvariantCulture));
                    parentForm.parentForm.settings.Events[EventIndex].SensorName = cb.Name.ToString(CultureInfo.InvariantCulture);
                }

                // clean up the ordering incase the user has changed it
                parentForm.reSortOrderValues(EventIndex);

                // edit the timer with the new values
                //parentForm.parentForm.editEventTimer(parentForm.parentForm.settings.Events[EventIndex]);

                // re-create the event timers to make sure everything is up to date
                parentForm.parentForm.CreateEventTimers();

                // fire all events to reset/update all messages
                // processed in the parent function
                //parentForm.parentForm.ProcessAllEvents();

                output = true;


                // clean up the order values for all events, 
                // as the user may have changed this one to be infront of another (both have same value at this point) 

            }            

            parentForm.parentForm.SaveSettings(string.Empty, this.parentForm.parentForm.settings);

            return output;
        }

        //internal void reSortOrderValues(int vIndexToKeepPos)
        //{
        //    //return;
        //    int valToKeep = parentForm.parentForm.settings.Events[vIndexToKeepPos].OrderPos;

        //    //sort by the new order
        //    parentForm.parentForm.settings.Events.Sort(EventInfo.CompareByOrder);

        //    //for (int x = 1; x < parentForm.parentForm.settings.Events.Count; x++)
        //    {
        //        for (int y = 0; y < parentForm.parentForm.settings.Events.Count; y++)
        //        {
        //            if (y + 1 < parentForm.parentForm.settings.Events.Count)
        //            {
        //                if (parentForm.parentForm.settings.Events[y].OrderPos == parentForm.parentForm.settings.Events[y + 1].OrderPos)
        //                {
        //                    // the current and last entries have same position. one needs to change
        //                    if (y != vIndexToKeepPos)
        //                    {
        //                        // x - 1 is the value to change
        //                        parentForm.parentForm.settings.Events[y].OrderPos += 1;
        //                    } else
        //                    {

        //                    }
        //                }
        //            }

        //        }
                
        //    }
            

        //    //sort by the new order
        //    parentForm.parentForm.settings.Events.Sort(EventInfo.CompareByOrder);
        //}

        private Boolean addEvent()
        {
            Boolean output = false;

            String message = string.Empty;
            if (isFormValid(ref message) == false)
            {
                MessageBox.Show("There were problems adding this Event:\n" + message + "\n");
                return output;
            }


            EventInfo newEvent = new EventInfo();

            if (cbCheckAction.SelectedItem != null)
                newEvent.CheckAction = EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString());
            newEvent.CheckSeconds = (int)numCheckSeconds.Value;
            newEvent.CheckValueString = txtValue.Text;
            newEvent.CheckValueDecimal = numValue.Value;
            newEvent.Enabled = cbEnabled.Checked;
            newEvent.ImagePath = this.imagePath;
            newEvent.Message = txtMessage.Text.ToString(CultureInfo.InvariantCulture);
            newEvent.OverrideWallpaper = cbOverride.Checked;
            newEvent.Displayed = false;
            newEvent.ShowNotification = cbShowNotification.Checked;
            newEvent.FontSize = (float)numFontSize.Value;
            newEvent.FontColour = pnlFontColour.BackColor;
            newEvent.LastRun = lastRun;
            newEvent.OrderPos = (int)numOrderPos.Value;
            newEvent.Transparency = (int)numTransparent.Value; //(int)numTransparent.Value;

            if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
            {
                newEvent.CheckValueString = cbAvailable.Checked.ToString(CultureInfo.InvariantCulture);
            }
            if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
            {
                newEvent.CheckValueString = txtValue.Text.ToString(CultureInfo.InvariantCulture);
                //newEvent.CheckValueDecimal = (cbAvailable.Checked ? 1 : 0);
            }
            
            //if (cbSensor.SelectedItem.ToString(CultureInfo.InvariantCulture).ToUpper().Contains("SUNRISE")
            //            || cbSensor.SelectedItem.ToString(CultureInfo.InvariantCulture).ToUpper().Contains("SUNSET"))
            //{
            //    DateTime t = new DateTime(1980, 1, 1, dtValue.Value.Hour, dtValue.Value.Minute, 0);
            //    newEvent.CheckValueDecimal = t.Ticks;
            //}

            ComboboxItemCustom cb = (ComboboxItemCustom)cbSensor.SelectedItem;
            if (cbSensor.SelectedItem != null)
            {
                newEvent.SensorName = cb.Name.ToString(CultureInfo.InvariantCulture);//EventInfo.StringToCheckEventType(cbSensor.SelectedItem.ToString(CultureInfo.InvariantCulture));
                newEvent.TypeOfEvent = cb.Value.ToString();
            }

            parentForm.parentForm.settings.Events.Add(newEvent);

            // clean up the ordering incase the user has changed it
            parentForm.reSortOrderValues(parentForm.parentForm.settings.Events.Count - 1);

            // add a timer for the new event
            parentForm.parentForm.addEventTimer(newEvent);

            // re-create the event timers to make sure everything is up to date
            //parentForm.parentForm.CreateEventTimers();

            parentForm.parentForm.SaveSettings(string.Empty, this.parentForm.parentForm.settings);

            // fire all events to reset/update all messages
            // processing is handled in parent form
            //parentForm.parentForm.ProcessAllEvents();

            output = true;
            return output;
        }


        internal void updateLogicText()
        {
            string text;// = string.Empty;

            text = "Checking every " + numCheckSeconds.Value.ToString(CultureInfo.InvariantCulture) + " seconds:" + System.Environment.NewLine;

            // EVENT TYPE
            if (cbSensor.SelectedItem != null)
            {
                ComboboxItemCustom cb = (ComboboxItemCustom)cbSensor.SelectedItem;
                if (cb != null) //EventInfo.StringToCheckEventType(cbSensor.SelectedItem.ToString(CultureInfo.InvariantCulture)) != EventInfo.EventType.None)
                {
                    if (cb.Name != "None")
                    {
                        text += "If the " + cbSensor.SelectedItem.ToString() + " ";
                    } else
                    {
                        text += "If no sensor is selected ";
                    }
                }
            }
            else
            {
                text += "If the [unknown] ";
                // Found "none"
            }

            // ACTION
            if (cbCheckAction.SelectedItem != null)
            {
                if (EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString()) != EventInfo.CheckActionType.None)
                {
                    string actionText = string.Empty;
                    switch (EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString()))
                    {
                        case EventInfo.CheckActionType.None:
                            actionText = "is [unknown]";
                            break;
                        case EventInfo.CheckActionType.GreaterThan:
                            actionText = "is Greater Than";
                            break;
                        case EventInfo.CheckActionType.LessThan:
                            actionText = "is Less Than";
                            break;
                        case EventInfo.CheckActionType.Contains:
                            actionText = "Contains";
                            break;
                        case EventInfo.CheckActionType.EqualTo:
                            actionText = "is Equal To";
                            break;
                        case EventInfo.CheckActionType.NotEqualTo:
                            actionText = "is Not Equal To";
                            break;
                        case EventInfo.CheckActionType.DisplayAlways:
                            actionText = "is to be Displayed Always,";
                            break;
                        default:
                            break;
                    }
                    if (cbSensor.SelectedItem.ToString() == "None")
                    {
                        actionText = ", and " + actionText;
                    }
                    text += "" + actionText;

                    String selectedItem = cbCheckAction.SelectedItem.ToString();
                    CheckActionType actionType = StringToCheckActionType(selectedItem);

                    if (cbSensor.SelectedItem != null)
                    {
                        if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                        || cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET"))
                        {
                            text += " " + dtValue.Value.ToShortTimeString() + ", ";//  numValue.Value.ToString(CultureInfo.InvariantCulture) + ", ";
                        }
                        else if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                            || cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION"))
                        {
                            text += " \"" + txtValue.Text.ToString(CultureInfo.InvariantCulture) + "\", ";//  numValue.Value.ToString(CultureInfo.InvariantCulture) + ", ";
                        }
                        else
                        {
                            if (actionType != CheckActionType.DisplayAlways)
                            {
                                // 
                                text += " " + numValue.Value.ToString(CultureInfo.InvariantCulture) + ", ";
                            }                            
                        }

                        //else if (actionType != CheckActionType.DisplayAlways       //EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString(CultureInfo.InvariantCulture)) != EventInfo.CheckActionType.DisplayAlways
                        //&& actionType != CheckActionType.None)     // EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString(CultureInfo.InvariantCulture)) != EventInfo.CheckActionType.None)
                        //{
                        //    text += " " + numValue.Value.ToString(CultureInfo.InvariantCulture) + ", ";
                        //}
                        //else
                        //{
                        //    int g = 1; // we shouldnt ever land here
                        //}
                    }
                }
            }
            else
            {
                // Found "none"
                text += "[unknown] ";
            }

            //text += numValue.Value.ToString(CultureInfo.InvariantCulture) + "";
            text += " then show";
            if ((!string.IsNullOrEmpty(this.ImagePath) && cbOverride.Checked) && !string.IsNullOrEmpty(this.txtMessage.Text.Trim()) )
            {
                text += " the Wallpaper and Message";
            }
            else if ((!string.IsNullOrEmpty(this.ImagePath) && cbOverride.Checked) && string.IsNullOrEmpty(this.txtMessage.Text.Trim()) )
            {
                text += " the Wallpaper";
            }
            else if (string.IsNullOrEmpty(this.ImagePath) && !string.IsNullOrEmpty(this.txtMessage.Text.Trim()) )
            {
                text += " the Message";
            }
            else
            {
                text += " (no Wallpaper or Message)";
            }

            if (cbShowNotification.Checked)
            {
                text += ", and pop a new Notification";
            } else
            {
                text += ".";
            }
            

            lblLogicText.Text = text;
        }


        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            updateLogicText();
        }


        private void cbEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            updateLogicText();
            showTheValueToolStripMenuItem.Enabled = true;
            if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("NONE"))
            {
                showTheValueToolStripMenuItem.Enabled = false;
            }
            CheckSensorMinimumTime();
            if (!this.applyingSettings)
                updateCurrentSensorValue(txtValue.Text.ToString(CultureInfo.InvariantCulture));
            setValueControl();


        }

        private void CheckSensorMinimumTime()
        {
            numCheckSeconds.Minimum = FrmMain.MinimumSensorUpdateSeconds; // default minimum
            if (numCheckSeconds.Value < numCheckSeconds.Minimum)
            {
                numCheckSeconds.Value = numCheckSeconds.Minimum;
            }

            if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("WEATHER"))
            {
                //numCheckSeconds.Minimum = Setting.MinimumWeatherUpdateSeconds;
                
                // if user has own key
                if (parentForm.parentForm.settings.HasOwnWeatherKey())//(!String.IsNullOrEmpty(parentForm.parentForm.settings.OpenWeatherAPIKey.Trim()))
                {
                    numCheckSeconds.Minimum = Setting.MinimumWeatherUpdateSecondsHasKey;
                    if (numCheckSeconds.Value < Setting.MinimumWeatherUpdateSecondsHasKey)
                    {
                        numCheckSeconds.Value = Setting.MinimumWeatherUpdateSecondsHasKey;
                    }
                } else
                {
                    numCheckSeconds.Minimum = Setting.MinimumWeatherUpdateSeconds;
                    if (numCheckSeconds.Value < Setting.MinimumWeatherUpdateSeconds)
                    {
                        numCheckSeconds.Value = Setting.MinimumWeatherUpdateSeconds;
                    }
                }
                
                
            }
        }


        internal void setValueControl()
        {
            ComboboxItemCustom selectedItem = (ComboboxItemCustom)cbSensor.SelectedItem;

            // hide all to start
            numValue.Visible = false;
            dtValue.Visible = false;
            txtValue.Visible = false;
            cbAvailable.Visible = false;
            txtValue.Size = new System.Drawing.Size(95, txtValue.Size.Height);
            lblValue.Text = "Value";
            lblValue2.Visible = false;

            numValue.Minimum = -1000000;
            numValue.Maximum = 1000000;
            numValue.Location = new System.Drawing.Point(117, 208);
            numValue.DecimalPlaces = 2;

            if ((SensorType)selectedItem.Value == SensorType.SmallData
                    && (selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                    || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                    ) )
            {
                // Sunrise / Sunset 
                dtValue.Location = numValue.Location;
                dtValue.Visible = true;
                //numValue.Visible = false;
                //txtValue.Visible = false;
            }
            else if (selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                    || selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION"))
            {
                // Clouds / WindDirection
                txtValue.Location = numValue.Location;
                txtValue.Visible = true;
                //numValue.Visible = false;
                //dtValue.Visible = false;

            }
            else if (selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
            {                
                // Internet Connection
                cbAvailable.Checked = MainFunctions.CheckForInternetConnection();
                cbAvailable.Location = numValue.Location;
                cbAvailable.Visible = true;
                //numValue.Visible = false;
                //dtValue.Visible = false;
                //txtValue.Visible = false;
            }
            else if (selectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE"))
            {
                // Internet Website
                //mtxtAddress.Text  = MainFunctions.CheckForInternetConnection();
                lblValue.Text = "Website";
                lblValue2.Visible = true;
                lblValue2.Text = "Ping";

                txtValue.Location = new System.Drawing.Point(numValue.Location.X, numValue.Location.Y);
                txtValue.Visible = true;
                txtValue.Size = new System.Drawing.Size(150, txtValue.Size.Height);
                txtValue.Enabled = true;

                numValue.Minimum = 0;
                numValue.Maximum = 15000;
                numValue.DecimalPlaces = 0;
                numValue.Visible = true;
                //numValue.Value = 0; // MainFunctions.CheckForWebsite(txtValue.Text, pingTimeout);
                numValue.Location = new System.Drawing.Point(numValue.Location.X, txtValue.Location.Y + txtValue.Height + 4);
                numValue.Enabled = true;
                //cbAvailable.Checked = (MainFunctions.CheckForWebsite(txtValue.Text, pingTimeout) > 0);
                //cbAvailable.Location = new System.Drawing.Point(txtValue.Location.X, cbAvailable.Location.Y);
                //cbAvailable.Visible = true;
            }
            else
            {
                // Defaults here
                //dtValue.Location = numValue.Location;
                //dtValue.Visible = false;
                numValue.Visible = true;
                //txtValue.Visible = false;
            }
            
            setActionDropDown(selectedItem);

        }


        private void setActionDropDown(ComboboxItemCustom vSelectedItem)
        {
            // defaults
            cbCheckAction.Items.Clear();
            cbCheckAction.Items.Add("GreaterThan");
            cbCheckAction.Items.Add("LessThan");
            cbCheckAction.Items.Add("EqualTo");
            cbCheckAction.Items.Add("NotEqualTo");
            cbCheckAction.Items.Add("DisplayAlways");
            cbCheckAction.SelectedIndex = cbCheckAction.Items.Count - 1; 

            if ((SensorType)vSelectedItem.Value == SensorType.SmallData
                    && (vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                    || vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET")
                    )
                    )
            {
                // Sunrise / Sunset 
                // use defaults because times are converted to (long)ticks
                
            }
            else if (vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                    || vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION"))
            {
                // Clouds / WindDirection - TEXT
                cbCheckAction.Items.Clear();
                cbCheckAction.Items.Add("Contains");
                cbCheckAction.Items.Add("EqualTo");
                cbCheckAction.Items.Add("NotEqualTo");
                cbCheckAction.Items.Add("DisplayAlways");
            }
            else if (vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
            {
                // checkbox
                cbCheckAction.Items.Clear();
                cbCheckAction.Items.Add("EqualTo");
                cbCheckAction.Items.Add("NotEqualTo");
                cbCheckAction.Items.Add("DisplayAlways");
            }
            //else if (vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Equals("NONE"))
            else if (String.Equals(vSelectedItem.Name.ToUpperInvariant(), "NONE", StringComparison.InvariantCulture))
            {
                // checkbox
                cbCheckAction.Items.Clear();
                cbCheckAction.Items.Add("DisplayAlways");
                cbCheckAction.SelectedItem = "DisplayAlways";
            }

            //else if (vSelectedItem.Name.ToString(CultureInfo.InvariantCulture).ToUpper().Contains("INTERNETWEBSITE"))
            //{
            //    cbCheckAction.Items.Clear();
            //    cbCheckAction.Items.Add("EqualTo");
            //    cbCheckAction.Items.Add("NotEqualTo");
            //    cbCheckAction.Items.Add("DisplayAlways");
            //}
            else
            {
                // Defaults here
                // already set by default on form creation
            }
        }



        internal void updateCurrentSensorValue(Object vCheckValue)
        {
            object value;// = string.Empty;
                         // = string.Empty;
                         // = DateTime.Now;

            try
            {


                if (cbSensor.SelectedItem != null)
                {
                    ComboboxItemCustom selectedItem = (ComboboxItemCustom)cbSensor.SelectedItem;
                    value = parentForm.parentForm.getSensorValue(selectedItem.Name, (SensorType)selectedItem.Value, vCheckValue.ToString(), out string textData); // txtValue.Text

                    if (Boolean.TryParse(textData.ToString(CultureInfo.InvariantCulture), out Boolean internetAvailable) == true)
                    {
                        // Boolean
                        value = internetAvailable;
                    }
                    else if (DateTime.TryParse(textData.ToString(CultureInfo.InvariantCulture), out DateTime dateData) == true)
                    {
                        // date data
                        value = dateData.ToString("HH:mm", CultureInfo.CurrentCulture);
                    }
                    else if (value.ToString() == "-999999")
                    {
                        // text data
                        value = textData;
                    }
                    else if (String.IsNullOrEmpty(textData) && (float)value == 0)
                    {
                        // BLANK / NULL DATA
                        value = "[Run LAWC as Administrator to get this value]";
                    }
                    else
                    {
                        // float numeric value
                        value = Math.Round((float)value, 2);
                    }

                    //if (value.ToString(CultureInfo.InvariantCulture) == "-999999")
                    //{
                    //    // text data
                    //    value = textData;
                    //}
                    //else if (DateTime.TryParse(value.ToString(CultureInfo.InvariantCulture), out dateData) == true)
                    //{
                    //    // date data
                    //    value = dateData;
                    //}
                    //else
                    //{
                    //    // float numeric value
                    //    value = Math.Round((float)value, 2);
                    //}

                    lblCurrentValue.Text = "Current Value: "
                        + value.ToString() + " ";
                    //+ " (" + selectedItem.Value.ToString(CultureInfo.InvariantCulture) + ")";
                }
            }
            catch (FormatException ex)
            {
                //throw new ApplicationException(string.Format("I cannot write the file {0} to {1}", fileName, directoryName), ex);
                //throw new ApplicationException(string.Format("Problem updating the Sensor Value {0} {1}", value.ToString(CultureInfo.InvariantCulture), textData.ToString(CultureInfo.InvariantCulture)), ex);
                ErrorHandling.ProcessError(ex, ErrorMessageType.SensorValueUpdate, false, false, String.Empty, Setting.getSettingsFullPath(Properties.Settings.Default.Portable));
            }

        }


        private void cbCheckAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventInfo.StringToCheckActionType(cbCheckAction.SelectedItem.ToString()) == EventInfo.CheckActionType.DisplayAlways)
            {
                numValue.Enabled = false;
                cbShowNotification.Checked = false;
                cbShowNotification.Enabled = false;
                dtValue.Enabled = false;
                txtValue.Enabled = false;
                cbAvailable.Enabled = false;
                
                //lblClickImage.Enabled = false;
                //lblClickImage.Visible = false;
                pbImage.Enabled = false;
                lblDisplayAlways.Visible = true;                
                clearImage();
            }
            else
            {
                numValue.Enabled = true;
                cbShowNotification.Enabled = true;
                dtValue.Enabled = true;
                txtValue.Enabled = true;                
                pbImage.Enabled = false;
                lblDisplayAlways.Visible = false;
                cbAvailable.Enabled = true;

                //lblClickImage.Visible = true;
                //lblClickImage.Enabled = true;
            }
            if (cbSensor.SelectedItem != null)
            {
                if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETWEBSITE")
                //|| cbSensor.SelectedItem.ToString(CultureInfo.InvariantCulture).ToUpper().Contains("INTERNETCONNECTION")
                )
                {
                    txtValue.Enabled = true;
                    txtValue.Visible = true;
                    //cbAvailable.Enabled = true;
                    //numValue.Enabled = true;
                    //numValue.Visible = true;
                }

                if (cbSensor.SelectedItem.ToString().ToUpper(CultureInfo.InvariantCulture).Contains("INTERNETCONNECTION"))
                {
                    //txtValue.Enabled = true;
                    //txtValue.Visible = true;
                    //         cbAvailable.Enabled = true;
                    //numValue.Enabled = true;
                    //numValue.Visible = true;
                }
            } 
            

            setLabels(!string.IsNullOrEmpty(this.ImagePath)); 

            updateLogicText();

        }

        private void numValue_ValueChanged(object sender, EventArgs e)
        {
            updateLogicText();
        }

        /// <summary>
        /// Opens a dialog to select and return the path to an image file
        /// </summary>
        /// <returns></returns>
        internal static String getImagePath()
        {
            DialogResult result;// = DialogResult.OK;
            String output;// = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.SpecialFolder.MyComputer.ToString(), //Environment.SpecialFolder.MyPictures;
                CheckFileExists = true,
            };

            result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            { output = dialog.FileName; }
            else
            { output = string.Empty; }

            dialog.Dispose();

            return output;
            
        }

        private void lblClickImage_Click(object sender, EventArgs e)
        {
            GetEventImage();
        }

        private void lblClearImage_Click(object sender, EventArgs e)
        {
            clearImage();
        }

        internal void clearImage()
        {
            ImagePath = string.Empty;
            pbImage.Image = null;
            pbImage.InitialImage = null;
            loadEventImage(ImagePath);
            updateLogicText();

            cbOverride.Checked = false;
            //cbCheckAction.Enabled = false;
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbOverride_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void setCheckSeconds(Boolean vFinishedEntering)
        {
            //int checkSecs = 300; // default
            if (int.TryParse(numCheckSeconds.Value.ToString(CultureInfo.InvariantCulture), out int checkSecs))
            {
                if (vFinishedEntering) showFastEventsWarning(checkSecs);
                updateLogicText();
            }
        }

        private void showFastEventsWarning(int vSeconds)
        {
            if (vSeconds < 20)
            {
                lblCheckSecondsWarning.Visible = true;
                lblCheckSecondsWarning.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                // Over 20 seconds left
                lblCheckSecondsWarning.Visible = false;
            }
        }

        private void lblCheckSecondsWarning_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Warning: \n\nSetting Events to check too often can slow down older pcs. \n\nUse fast values with caution.", "Use fast Event checking with Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnAddSpecialText_Click(object sender, EventArgs e)
        {
            
        }  

        private void showTheValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<Value>>");
        }

        private void degreesSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("°C");            
        }

        private void numCheckSeconds_ValueChanged(object sender, EventArgs e)
        {
            setCheckSeconds(false);
        }
        
        private void currentTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<Time>>");
        }

        private void currentDateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<DateTime>>");
        }

        private void currentDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<Date>>");
        }

        private void cbShowNotification_CheckedChanged(object sender, EventArgs e)
        {
            updateLogicText();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            //GetEventImage();
        }

        private void wallpaperFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<Filename>>");
        }

        private void titleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<meta_title>>");
        }

        private void insertText(String vTag)
        {
            int selectionIndex = txtMessage.SelectionStart;
            txtMessage.Text = txtMessage.Text.Insert(selectionIndex, vTag);
            txtMessage.SelectionStart = selectionIndex + vTag.Length; // restore cursor position to end of added text

            txtMessage.Focus();
        }

        private void subjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<meta_subject>>");
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<meta_description>>");
        }

        private void commentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<meta_comment>>");
        }

        private void numTransparent_ValueChanged(object sender, EventArgs e)
        {
            // value saved when OK button is pressed
        }

        private void numFontSize_ValueChanged(object sender, EventArgs e)
        {
            // value saved when OK button is pressed
        }

        private void cbAvailable_CheckedChanged(object sender, EventArgs e)
        {
            updateCurrentSensorValue(txtValue.Text.ToString(CultureInfo.InvariantCulture));
            parentForm.parentForm.AdjustDesktopImages();            
        }

        private void BtnRandomColour_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            pnlFontColour.BackColor = randomColor;
        }

        private void wallpaperCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<Category>>");
        }

        private void category2LevelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertText("<<Category2>>");
        }
    }
}
