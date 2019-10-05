using OpenHardwareMonitor.Hardware;
using System;
using System.Globalization;
using static OpenWeatherAPI.OpenWeatherAPI;

namespace LAWC.Objects
{
    internal class Sensor
    {
        internal enum SensorSource
        {
            None,
            Hardware,
            Weather,
            HDD,
            Power,
            Location,
            Internet,
        }

        /// <summary>
        /// Identifier and value of each sensor is stored in this object
        /// </summary>
        internal struct SensorSummary
        {
            /// <summary>
            /// The category of the data that the sensor returns. eg. Temp is a value, Load is a percent, and so on
            /// </summary>
            internal SensorType DataType;
            internal string Name;
            //internal float? Value;
            internal object Value;
            /// <summary>
            /// The sensor's category. Used to group logically related sensors together
            /// </summary>
            internal SensorSource Category;
        }



        internal static WeatherSensors getWeatherSensorName(String vSourceName)
        {
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Humidity")) return WeatherSensors.Humidity;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Pressure")) return WeatherSensors.Pressure;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("CelsiusCurrent")) return WeatherSensors.CelsiusCurrent;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Rain")) return WeatherSensors.Rain;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Snow")) return WeatherSensors.Snow;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Clouds")) return WeatherSensors.Clouds;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Sunrise")) return WeatherSensors.Sunrise;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Sunset")) return WeatherSensors.Sunset;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Visibility")) return WeatherSensors.Visibility;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindDegree")) return WeatherSensors.WindDegree;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindDirection")) return WeatherSensors.WindDirection;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindGust")) return WeatherSensors.WindGust;
            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindSpeedMPS")) return WeatherSensors.WindSpeedMPS;
            return WeatherSensors.CelsiusCurrent;
        }

        internal static SensorSource getSensorSource(string vSourceName)
        {
            SensorSource output;// = SensorSource.Hardware; // default

            if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Humidity")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Pressure")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("CelsiusCurrent")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Temperature")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Rain")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Snow")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Clouds")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Visibility")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindDegree")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindDirection")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindGust")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("WindSpeedMPS")
                )
            {
                output = SensorSource.Weather;
            }
            else if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("CPU")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("GPU")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Bus Speed")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Memory")
                )
            {
                output = SensorSource.Hardware;
            }
            else if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Power")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Battery")
                )
            {
                output = SensorSource.Power;
            }
            else if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Sunrise")
                || vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Sunset"))
            {
                output = SensorSource.Location;
            }
            else if (vSourceName.ToString(CultureInfo.InvariantCulture).Contains("Internet"))
            {
                output = SensorSource.Internet;
            }
            else
            {
                output = SensorSource.HDD;
            }
            return output;
        }


    }
}
