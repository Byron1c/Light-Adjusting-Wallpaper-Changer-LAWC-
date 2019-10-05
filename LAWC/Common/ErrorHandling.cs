using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Windows;


namespace LAWC.Common
{
    internal static class ErrorHandling
    {
        // from here: https://stackoverflow.com/questions/14973642/how-using-try-catch-for-exception-handling-is-best-practice

        //https://docs.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions


        internal enum ErrorMessageType
        {
            AdjustDesktopImageThread,
            DeletingImage,
            SensorValueUpdate,
            Initialisation,
            LoadingSettings,
            SettingFormValues,
            SelectingImage,
            InitialisingSensors,
            GetSensorValue,
            ActionResult,
            FiringEvent,
            AdjustDesktopImage,
            FilenameEmpty,
            FileDoesntExist,
            FileProblem,
            ImageScalingUnknown,
            AddingImage,
            SettingWallpaper,
            ThreadUpdate,
            ImportingXML,
            UpdatingMonitorInfo,
            WizardFinished,
            ProcessingFoundImages,
            SensorUnknown,
            HDDNotFound,
            CheckForUpdate,
            GetWebData,
            Search,
            FindLocation,
            OrderNotFound,
            NoImages,
            NoImagesFound,
            LoopBreak,
            XMLFileRead,
            WebsiteListNotFound,
            UnknownDataRead,
            ReadingSettingsXML,
            SetImageOpacity,
            SettingsNotFound,
            EventActionNotSet,
            SavingSettings,
            LoadingDSTablesToArray,
            LowestViewCountNotFound,
            WriteTextFile,
            WizardStart,
            SavingWallpaper,
            GraphicsGDI,
            StartUp,
            LoadingDSToSettings,
            ChangeWallpaper,
            UnknownDataWrite,
            ImageData,
            DeletingFiles,
            Crash,
        }

        internal static Dictionary<int, string> ErrorDescriptionList = new Dictionary<int, string>()
        {
            { (int)ErrorMessageType.ActionResult, "There was a problem checking a Sensor Event. Check your internet connection, and edit the Event to see what is wrong." },
            { (int)ErrorMessageType.AddingImage, "Unable to add an image to the wallpaper." },
            { (int)ErrorMessageType.AdjustDesktopImage, "There was a problem trying to adjust or change the wallpaper." },
            { (int)ErrorMessageType.AdjustDesktopImageThread, "There was a problem trying to start adjusting the wallpaper." },
            { (int)ErrorMessageType.SetImageOpacity, "Unable to create the bitmap for Image Opacity." },
            { (int)ErrorMessageType.CheckForUpdate, "There was a problem checking for updates." },
            { (int)ErrorMessageType.DeletingImage, "Unable to delete the file." },
            { (int)ErrorMessageType.DeletingFiles, "Unable to delete the old wallpaper files." },            
            { (int)ErrorMessageType.EventActionNotSet, "This is not a valid Event Action." },
            { (int)ErrorMessageType.FileDoesntExist, "The selected file does not exist." },
            { (int)ErrorMessageType.FilenameEmpty, "A filename was not supplied." },
            { (int)ErrorMessageType.FileProblem, "There was a problem loading the file." },
            { (int)ErrorMessageType.FindLocation, "Unable to find the chosen location" },
            { (int)ErrorMessageType.FiringEvent, "The Event did not work correctly." },
            { (int)ErrorMessageType.GetSensorValue, "There was a problem getting the Sensor value." },
            { (int)ErrorMessageType.GetWebData, "Unable to get data from the web." },
            { (int)ErrorMessageType.GraphicsGDI, "GDI+ Error trying to rotate/flip the image." },
            { (int)ErrorMessageType.HDDNotFound, "The Hard Drive selected in Events cannot be found." },
            { (int)ErrorMessageType.ImageData, "Unable to read the wallpaper Meta Data." },
            { (int)ErrorMessageType.ImageScalingUnknown, "The Image Scaling Mode selected is not valid." },
            { (int)ErrorMessageType.ImportingXML, "Unable to import the XML file." },
            { (int)ErrorMessageType.Initialisation, "There was a problem initialising LAWC." },
            { (int)ErrorMessageType.InitialisingSensors, "Initialising Sensors found a problem" },
            { (int)ErrorMessageType.LoadingSettings, "There was a problem loading the settings." },
            { (int)ErrorMessageType.LoadingDSTablesToArray, "Unable to load the data into the Settings" },
            { (int)ErrorMessageType.LoadingDSToSettings, "Unable to load the data into the Settings" },
            { (int)ErrorMessageType.LoopBreak, "Unable to get the next images from the Settings." },
            { (int)ErrorMessageType.LowestViewCountNotFound, "Unable to find the image with the lowest View Count." },
            { (int)ErrorMessageType.ChangeWallpaper, "Unable to change the wallpaper properly." },
            { (int)ErrorMessageType.NoImages, "No images are found in settings. Try adding a Folder." },
            { (int)ErrorMessageType.NoImagesFound, "No images are found in settings." },
            { (int)ErrorMessageType.OrderNotFound, "An unknown display Order was selected." },
            { (int)ErrorMessageType.ReadingSettingsXML, "There is a problem with the settings file. Unable to read the XML information." },
            { (int)ErrorMessageType.ProcessingFoundImages, "There was a problem processing the images found in the last scan." },
            { (int)ErrorMessageType.SavingSettings, "There was a problem saving the settings to a file." },
            { (int)ErrorMessageType.SavingWallpaper, "There was a problem saving the wallpaper to a file." },
            { (int)ErrorMessageType.Search, "Error when trying to search the list of wallpapers." },
            { (int)ErrorMessageType.SelectingImage, "There was a problem selecting the desired image." },
            { (int)ErrorMessageType.SensorUnknown, "The Sensor chosen is unknown." },
            { (int)ErrorMessageType.SensorValueUpdate, "Unable to update the Sensor value." },
            { (int)ErrorMessageType.SettingFormValues, "Initialising the Settings and Advanced Settings had a problem." },
            { (int)ErrorMessageType.SettingsNotFound, "The settings.xml file does not exist." },
            { (int)ErrorMessageType.SettingWallpaper, "Unable to set the wallpaper in Windows." },
            { (int)ErrorMessageType.StartUp, "Unable to finish starting up." },
            { (int)ErrorMessageType.ThreadUpdate, "There was a problem updating the information for the current Thread." },
            { (int)ErrorMessageType.UnknownDataRead, "Unable to read XML data correctly." },
            { (int)ErrorMessageType.UnknownDataWrite, "Unable to write the XML data correctly." },
            { (int)ErrorMessageType.UpdatingMonitorInfo, "There was a problem updating the monitor information." },
            { (int)ErrorMessageType.WebsiteListNotFound, "The list of websites was not found." },
            { (int)ErrorMessageType.WizardFinished, "There was a problem finishing the Wizard." },
            { (int)ErrorMessageType.WizardStart, "There was a problem starting the Wizard." },
            { (int)ErrorMessageType.WriteTextFile, "Unable to write TEXT file." },
            { (int)ErrorMessageType.XMLFileRead, "Unable to read the XML file." },            

        };

        // Extension methods for Error Handling
        internal static Exception Log(this Exception ex, ref Boolean vIsLogged, String vSettingsFullPath)
        {
            //File.AppendAllText("CaughtExceptions" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ".log", DateTime.Now.ToString("HH:mm:ss") + ": " + ex.Message + "\n" + ex.ToString(CultureInfo.InvariantCulture) + "\n");
            if (!vIsLogged)
            {
                string stackTrace = ex.StackTrace;
                string lawcStack = string.Empty;

                if (stackTrace != null)
                {
                    using (StringReader reader = new StringReader(stackTrace))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains("at LAWC"))
                            {
                                lawcStack += line;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(lawcStack))
                {
                    WriteError(
                        "************************************************************\n"
                        + ErrorHandling.getCurrentDateTimeLog()
                        + "  Exception: " + ex.Message + "\n" + lawcStack, true, vSettingsFullPath); //// ex.ToString(CultureInfo.InvariantCulture), true);
                }
                else
                {
                    WriteError(
                        "************************************************************\n"
                        + ErrorHandling.getCurrentDateTimeLog()
                        + "  Exception: " + ex.Message, true, vSettingsFullPath); //// ex.ToString(CultureInfo.InvariantCulture), true);
                }

                vIsLogged = true;
            }            

            return ex;
        }

        internal static Exception Display(this Exception ex, ref Boolean vIsDisplayed, string msg = null) //, MessageBoxImage img = MessageBoxImage.Error, FrmMain vParentForm = null)
        {
            //MessageBox.Show(msg ?? ex.Message, "", MessageBoxButton.OK, img);
            
            if (!vIsDisplayed)
            {
                frmShowText showInfo = new frmShowText
                {
                    Text = "Unexpected Error"
                };
                showInfo.lblHeading.Text = "There was an Error with LAWC";
                showInfo.lblDetails.Text = "Sorry, there was a problem. Please have a look at the details below, and add any extra information "
                    + "that might be relevant. Eg. What you were doing at the time. \nPlease only write above the horizontal line. Then press [Send Error].\n "
                    + "Thank you :)";
                showInfo.richText.Text = "\n\n___________________^ Please only reply in the area above this line ^___________________\n\n"
                    + (msg ?? ex.Message);
                showInfo.richText.ReadOnly = false;
                showInfo.btnCancel.Visible = false;
                showInfo.btnSendError.Location = showInfo.btnCancel.Location;
                showInfo.btnSendError.Visible = true;
                showInfo.btnOK.Text = "Close";
                showInfo.ShowDialog();

                vIsDisplayed = true;

                showInfo.Dispose();
                //DialogResult result = showInfo.DialogResult;

                //if (result == DialogResult.Yes) // send error
                //{
                //    SendSMTPMail("@gmail.com", "me@here.com", "LAWC Error Received!", showInfo.richText.Text, MailPriority.High); 
                //}
            }

            return ex;
        }

        internal static String getCurrentDateTimeLog()
        {

            return String.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2} {3}:{4}:{5}",
                        DateTime.Now.Year.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Month.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Day.ToString("D2", CultureInfo.InvariantCulture),

                        DateTime.Now.Hour.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Minute.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Second.ToString("D2", CultureInfo.InvariantCulture)
                     );
        }

        public static void WriteText(String vMessage, String vFilePath)
        {
            String path = vFilePath;
            if (String.IsNullOrEmpty(path)) path = @".\Debug.log";

            ErrorHandling.WriteToLog(
                    path,
                    vMessage,
                    true,//vWriteToLog, 
                    path); // settings.WriteToLog);

        }

        internal static void WriteError(String vMessage, Boolean vWriteToLog, String vSettingsFullPath)
        {

            ErrorHandling.WriteToLog(
                    Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, vSettingsFullPath), 
                    vMessage, 
                    vWriteToLog, vSettingsFullPath); // settings.WriteToLog);

        }

        internal static void WriteErrorDatetime(String vMessage, Boolean vWriteToLog, String vSettingsFullPath)
        {

            ErrorHandling.WriteToLog(
                    Setting.GetErrorLogFullPath(Properties.Settings.Default.Portable, vSettingsFullPath),
                    String.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2} {3}:{4}:{5}: {6}",
                        DateTime.Now.Year.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Month.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Day.ToString("D2", CultureInfo.InvariantCulture),

                        DateTime.Now.Hour.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Minute.ToString("D2", CultureInfo.InvariantCulture),
                        DateTime.Now.Second.ToString("D2", CultureInfo.InvariantCulture),

                        vMessage

                     ), vWriteToLog, vSettingsFullPath); // settings.WriteToLog);

        }

        /// <summary>
        /// Write a line entry to the end of the existing log file, or create a new one if there is no existing file
        /// </summary>
        /// <param name="vLogPath"></param>
        /// <param name="vMessage"></param>
        /// <param name="vWriteToLogAllowed"></param>
        /// <returns></returns>
        public static Boolean WriteToLog(String vLogPath, String vMessage, Boolean vWriteToLogAllowed, String vSettingsFullPath)
        {
            if (vWriteToLogAllowed)
            {
                try
                {

                    using (StreamWriter outfile = new StreamWriter(vLogPath, true))
                    {
                        outfile.WriteLine(vMessage);
                        outfile.Close();
                    }

                    return true;
                }
                catch (IOException ex)
                {
                    ProcessError(ex, ErrorMessageType.WriteTextFile, false, false, String.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                    return false;
                }
            }
            return false;
        }


        internal static void ProcessError(Exception ex, ErrorMessageType vMessageType, Boolean vDisplayMessage, Boolean vThrow, String vExtraInfo, String vSettingsFullPath)
        {
            string message = string.Empty;
            string exceptionDetails = string.Empty;

            message += "Error Type: " + vMessageType.ToString() + "\n";
            int i = (int)vMessageType;

            if (ErrorDescriptionList.ContainsKey(i))
            {
                message += ErrorDescriptionList[i] + "\n\n";
                message += "------------------------------------\n";
                //value = ErrorDescriptionList[key];
            }

            if (ex != null)
            {
                exceptionDetails = "\tException: " + ex.ToString(); // + "\n";
            }
            //else
            //{
            //    // exception is null / empty
            //    exceptionDetails = string.Empty;
            //}

            if (!string.IsNullOrEmpty(vExtraInfo.Trim()))
            {
                message += "\tExtra Info: \n  " + vExtraInfo + "\n";
            }

            Boolean isDisplayed = false;
            Boolean isLogged = false;

            // NOTE: always log the error
            if (vThrow && vDisplayMessage)
            {
                if (ex != null) throw new ApplicationException(message + exceptionDetails, ex).Log(ref isLogged, vSettingsFullPath).Display(ref isDisplayed, null);//,  MessageBoxImage.Error);
                else throw new ApplicationException(message + exceptionDetails, null).Log(ref isLogged, vSettingsFullPath).Display(ref isDisplayed, null);//,  MessageBoxImage.Error);
            }
            else if (!vThrow && vDisplayMessage)
            {
                if (ex != null) new ApplicationException(message + exceptionDetails, ex).Log(ref isLogged, vSettingsFullPath).Display(ref isDisplayed, null);//, MessageBoxImage.Error);
                else new ApplicationException(message + exceptionDetails, null).Log(ref isLogged, vSettingsFullPath).Display(ref isDisplayed, null);//, MessageBoxImage.Error);
            }
            else if (vThrow && !vDisplayMessage)
            {
                if (ex != null) throw new ApplicationException(message + exceptionDetails, ex).Log(ref isLogged, vSettingsFullPath);
                else throw new ApplicationException(message + exceptionDetails, null).Log(ref isLogged, vSettingsFullPath);
            }
            else if (!vThrow && !vDisplayMessage)
            {
                if (ex != null) new ApplicationException(message + exceptionDetails, ex).Log(ref isLogged, vSettingsFullPath);
                else new ApplicationException(message + exceptionDetails, null).Log(ref isLogged, vSettingsFullPath);
            }


        }

        internal static Boolean SendSMTPMail(String vFrom, String vTo, String vSubject, String vBody, string vSettingsFullPath, MailPriority vPriority = MailPriority.Normal, String vAttachmentPath = "")
        {
            Boolean output;

            MailMessage mail = new MailMessage(vFrom, vTo);
            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = Constants.EmailSMTP, //"smtp.gmail.com",
                Port = Constants.EmailSMTPPort, //587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(Constants.EmailAccountUser, Constants.EmailAccountPass) 
            };

            mail.Subject = vSubject;
            mail.Body = vBody;
            mail.IsBodyHtml = true;
            mail.Priority = vPriority; // MailPriority.High;

            //Attachment
            if (vAttachmentPath != string.Empty && System.IO.File.Exists(vAttachmentPath))
            {
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                contentType.Name = System.IO.Path.GetFileName(vAttachmentPath);
                mail.Attachments.Add(new Attachment(vAttachmentPath, contentType));
            }

            try
            {
                client.Send(mail);
                output = true;
            }
            catch (SmtpException  ex)
            {
                ErrorHandling.ProcessError(ex, ErrorMessageType.StartUp, true, false, String.Format(CultureInfo.InvariantCulture, ""), vSettingsFullPath);
                output = false;
            }

            mail.Dispose();
            client.Dispose();

            return output;
        }


    }
}
