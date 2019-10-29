using System;


namespace LAWC
{
    internal class Constants
    {

        //TODO: Set these values for your own email account and OpenWeather API Key
        // Note: Email set to use SSL 
        internal const String EmailAccountUser = "{email account username}"; //eg myemail@here.com
        internal const String EmailAccountPass = "{email account password}"; //eg abcdef1234!@# 
        internal const String EmailSMTP = "{SMTP Server Address}"; //eg smtp.gmail.com
        internal const int EmailSMTPPort = 587;

        internal const String ErrorFromEmail = "{From email address}"; //eg myemail@here.com
        internal const String ErrorToEmail = "{To email address}"; //eg myemail@here.com

        internal static String OpenWeatherAPIKey = "{your OpenWeather API Key}"; //get from https://openweathermap.org/appid



    }
}
