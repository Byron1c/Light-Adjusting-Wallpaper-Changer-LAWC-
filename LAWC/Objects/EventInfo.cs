using System;
using System.Collections;
using System.Drawing;
using OpenHardwareMonitor.Hardware;
using System.Globalization;

namespace LAWC.Objects
{
    internal class EventInfo : IComparable
    {

        internal enum CheckActionType
        {
            None,
            GreaterThan,
            LessThan,
            Contains,
            EqualTo,
            NotEqualTo,
            DisplayAlways
        }

        private int orderPos;
        public int OrderPos
        {
            get { return orderPos; }
            set { orderPos = value; }
        }

        private String imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        private String message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        private int checkSeconds;
        public int CheckSeconds
        {
            get { return checkSeconds; }
            set { checkSeconds = value; }
        }
        
        private String nameOfEvent;
        public String SensorName
        {
            get { return nameOfEvent; }
            set { nameOfEvent = value; }
        }

        private String typeOfEvent;
        public String TypeOfEvent
        {
            get { return typeOfEvent; }
            set { typeOfEvent = value; }
        }

        private CheckActionType checkAction;
        public CheckActionType CheckAction
        {
            get { return checkAction; }
            set { checkAction = value; }
        }

        private Decimal checkValueDecimal;
        public Decimal CheckValueDecimal
        {
            get { return checkValueDecimal; }
            set { checkValueDecimal = value; }
        }

        private String checkValueString;
        public String CheckValueString
        {
            get { return checkValueString; }
            set { checkValueString = value; }
        }

        private Boolean overrideWallpaper;
        public Boolean OverrideWallpaper
        {
            get { return overrideWallpaper; }
            set { overrideWallpaper = value; }
        }

        private Boolean enabled;
        public Boolean Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        private Boolean displayed;
        public Boolean Displayed
        {
            get { return displayed; }
            set { displayed = value; }
        }

        private Boolean showNotification;
        public Boolean ShowNotification
        {
            get { return showNotification; }
            set { showNotification = value; }
        }

        private float fontSize;
        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        private Color fontColour;
        public Color FontColour
        {
            get { return fontColour; }
            set { fontColour = value; }
        }

        private DateTime lastRun;
        public DateTime LastRun
        {
            get { return lastRun; }
            set { lastRun = value; }
        }

        private int transparency;
        public int Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }


        public EventInfo()
        {
            // Defaults
            OrderPos = 0;
            ImagePath = string.Empty;
            Message = string.Empty;
            CheckSeconds = 300;
            TypeOfEvent = string.Empty;//EventType.None;
            SensorName = string.Empty;
            CheckAction = CheckActionType.None;
            CheckValueDecimal = 10;
            checkValueString = string.Empty;
            OverrideWallpaper = true;
            Enabled = true;
            Displayed = false;
            ShowNotification = false;
            FontSize = 1080 / 90f;
            FontColour = Color.White;
            LastRun = new DateTime(1980, 1, 1);
            Transparency = 20; // (20% transparent)
        }
        

        /// <summary>
        /// Convert the text to the appropriate CheckActionType
        /// </summary>
        /// <param name="vInput"></param>
        /// <returns></returns>
        internal static CheckActionType StringToCheckActionType(String vInput)
        {
            CheckActionType output = CheckActionType.None;

            switch (vInput)
            {
                case "None":
                    return CheckActionType.None;
                case "GreaterThan":
                    return CheckActionType.GreaterThan;
                case "LessThan":
                    return CheckActionType.LessThan;
                case "Contains":
                    return CheckActionType.Contains;
                case "EqualTo":
                    return CheckActionType.EqualTo;
                case "NotEqualTo":
                    return CheckActionType.NotEqualTo;
                case "DisplayAlways":
                    return CheckActionType.DisplayAlways;

                default:
                    break;
            }

            return output;
        }

        internal static SensorType StringToSensorType(String vInput)
        {
            switch (vInput)
            {
                case "Voltage":
                    return SensorType.Voltage;

                case "Clock":
                    return SensorType.Clock;

                case "Temperature":
                    return SensorType.Temperature;

                case "Load":
                    return SensorType.Load;

                case "Fan":
                    return SensorType.Fan;

                case "Flow":
                    return SensorType.Flow;

                case "Control":
                    return SensorType.Control;

                case "Level":
                    return SensorType.Level;

                case "Factor":
                    return SensorType.Factor;

                case "Power":
                    return SensorType.Power;

                case "Data":
                    return SensorType.Data;

                case "SmallData":
                    return SensorType.SmallData;

                default:
                    return SensorType.Temperature;

            }
        }
        

        /// <summary>
        /// Make sure the logic statements in this match those in isFormValid()
        /// </summary>
        /// <param name="vIndex"></param>
        /// <param name="vMessage"></param>
        /// <returns></returns>
        internal Boolean isValid(ref String vMessage)
        {
            Boolean output = true;
            vMessage = String.Empty;

            if (this.Enabled == true) // only validate these controls if the user wants the event to be Enabled
            {
                if (this.CheckAction == EventInfo.CheckActionType.None)
                {
                    output = false;
                    vMessage += " * Select an Action for this event.\n";
                }

                if (this.SensorName == null)
                {
                    output = false;
                    vMessage += " * Select the Type for this event.\n";
                }
                else if (this.SensorName == "None")
                {
                    output = false;
                    vMessage += " * Select the Type for this event.\n";
                }
                if (this.CheckSeconds < 2)
                {
                    output = false;
                    vMessage += " * Enter the number of seconds for how often you want this event to check (> 2).\n";
                }

                if (string.IsNullOrEmpty(this.ImagePath)
                    && string.IsNullOrEmpty(this.Message.Trim()))
                {
                    output = false;
                    vMessage += " * Please enter a Message, or select an Image for this event.\n";
                }

                // check the value if we are calculating with it, not if its set to Display Always
                if (this.CheckAction != EventInfo.CheckActionType.DisplayAlways)  // not 
                {
                    //ComboboxItemCustom selectedItem = (ComboboxItemCustom)cbSensor.SelectedItem;
                    if ((this.SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNRISE")
                            || this.SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("SUNSET"))
                            )
                    {
                        // if we have selected a sunrise/sunset sensor
                        // cant have a "blank" entry, and the control handles validation

                    }
                    else if (this.SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("CLOUDS")
                            || this.SensorName.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture).Contains("WINDDIRECTION")
                            )
                    {
                        if (string.IsNullOrEmpty(this.CheckValueString.ToString(CultureInfo.InvariantCulture)))
                        {
                            output = false;
                            vMessage += " * Please enter the text to be checked for this event.\n";
                        }
                    }
                    else
                    {
                        // default, anything not covered above
                        if (string.IsNullOrEmpty(this.CheckValueDecimal.ToString(CultureInfo.InvariantCulture)))
                        {
                            output = false;
                            vMessage += " * Please enter a value to be checked for this event.\n";
                        }

                    }

                }
            }
            return output;
        }
        

        public int CompareTo(object obj)
        {
            EventInfo otherInfo = (EventInfo)obj;
            //return this.message.CompareTo(otherInfo.Message);
            return String.Compare(this.Message, otherInfo.Message, StringComparison.InvariantCulture);
        }
               

        public static int CompareByMessage(EventInfo x, EventInfo y)
        {
            //return x.Message.CompareTo(y.Message);
            return String.Compare(x.Message, y.Message, StringComparison.InvariantCulture);
        }

        public static int CompareByMessageDESC(EventInfo x, EventInfo y)
        {
            string filenameX = System.IO.Path.GetFileName(x.Message);
            string filenameY = System.IO.Path.GetFileName(y.Message);

            //return filenameY.CompareTo(filenameX);
            return String.Compare(filenameY, filenameX, StringComparison.InvariantCulture);
        }

        public int CompareTo(EventInfo item2, EventInfoSort.ComparisonType comparisonType)
        {
            int returnValue;

            if (comparisonType == EventInfoSort.ComparisonType.Message)
            {
                if (this.Message == item2.Message)
                {
                    //returnValue = Message.CompareTo(item2.Message);
                    returnValue = String.Compare(Message, item2.Message, StringComparison.InvariantCulture);
                }
                else
                {
                    //returnValue = Message.CompareTo(item2.Message);
                    returnValue = String.Compare(Message, item2.Message, StringComparison.InvariantCulture);
                }
            }
            //else if (comparisonType == EventInfoSort.ComparisonType.Brightness)
            //{
            //    returnValue = AverageBrightness.CompareTo(item2.AverageBrightness);
            //}
            else
            {
                //returnValue = Message.CompareTo(item2.Message);
                returnValue = String.Compare(Message, item2.Message, StringComparison.InvariantCulture);
            }

            return returnValue;

        }

        //internal static SensorType Parse(string typeOfEvent)
        //{
        //    throw new NotImplementedException();
        //}

        public static int CompareByOrder(EventInfo x, EventInfo y)
        {
            return x.OrderPos.CompareTo(y.OrderPos);
        }

    } // END CLASS
       


    internal class EventInfoSort : IComparer
    {
        public enum ComparisonType
        {
            Message,            
        }

        public enum SortDirection
        {
            Asc,
            Desc
        }


        private ComparisonType compMethod;
        public ComparisonType ComparisonMethod
        {
            get { return compMethod; }
            set { compMethod = value; }
        }

        public EventInfoSort(ComparisonType vType)
        {
            ComparisonMethod = vType;
        }

        public int Compare(object x, object y)
        {
            ImageInfo item1 = (ImageInfo)x;
            ImageInfo item2 = (ImageInfo)y;

            return item1.CompareTo(item2);//, ComparisonMethod, item1);

        }       

    }




}
