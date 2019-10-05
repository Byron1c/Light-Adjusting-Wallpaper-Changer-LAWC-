using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherAPI
{
    public class OpenWeatherAPI
    {

        public enum WeatherSensors
        {
            Humidity,
            Pressure,
            CelsiusCurrent,
            Rain,
            Snow,
            Clouds,
            Sunrise,
            Sunset,
            Visibility,
            WindDegree,
            WindDirection,
            WindGust,
            WindSpeedMPS,
        }

        private string openWeatherAPIKey;

        public OpenWeatherAPI(string apiKey)
        {
            openWeatherAPIKey = apiKey;
        }

        public void UpdateAPIKey(string apiKey)
        {
            openWeatherAPIKey = apiKey;
        }

        //Returns null if invalid request
        public Query Query(string queryStr)
        {
            Query newQuery = new Query(openWeatherAPIKey, queryStr);
            if (newQuery.ValidRequest)
                return newQuery;
            return null;
        }

        public Query Query(double vLat, double vLong)
        {
            Query newQuery = new Query(openWeatherAPIKey, vLat, vLong);
            if (newQuery.ValidRequest)
                return newQuery;
            return null;
        }

        public Query FindByName(String vName)
        {
            Query newQuery = new Query(openWeatherAPIKey, vName);
            if (newQuery.ValidRequest)
                return newQuery;
            return null;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public static object GetWeatherValue(double vLat, double vLong, OpenWeatherAPI.WeatherSensors vWeatherSensor, String vWeatherAPIKey)
        {            
            Query newQuery = new Query(vWeatherAPIKey, vLat, vLong);
            if (newQuery.ValidRequest)
            {
                switch (vWeatherSensor)
                {
                    case WeatherSensors.Humidity:
                        return newQuery.Main.Humdity;
                        
                    case WeatherSensors.Pressure:
                        return newQuery.Main.Pressure;

                    case WeatherSensors.CelsiusCurrent:
                        return newQuery.Main.Temperature.CelsiusCurrent;

                    case WeatherSensors.Rain:
                        if (newQuery.Rain != null)
                            return newQuery.Rain.H3;
                        else
                            return 0;

                    case WeatherSensors.Snow:
                        if (newQuery.Snow != null)
                            return newQuery.Snow.H3;
                        else
                            return 0;
                        
                    case WeatherSensors.Clouds:
                        return newQuery.Weathers[0].Main;
                        //return newQuery.Clouds.All;

                    case WeatherSensors.Sunrise:
                        return newQuery.Sys.Sunrise;

                    case WeatherSensors.Sunset:
                        return newQuery.Sys.Sunset;

                    case WeatherSensors.Visibility:
                        return newQuery.Visibility;

                    case WeatherSensors.WindDegree:
                        return newQuery.Wind.Degree;

                    case WeatherSensors.WindDirection:
                        return newQuery.Wind.Direction;
                        
                    case WeatherSensors.WindGust:
                        return newQuery.Wind.Gust;

                    case WeatherSensors.WindSpeedMPS:
                        return newQuery.Wind.SpeedMetersPerSecond;
                        
                    default:
                        return 0;
                }

                
            }
            return 0;
        }




    }
}
