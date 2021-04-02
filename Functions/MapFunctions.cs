using MoveItDemo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MoveItDemo.Functions
{
    public static class MapFunctions
    {
        /// <summary>
        /// This method is to getting the longitude and latitude for the address object
        /// </summary>
        /// <param name="query"></param>
        /// <returns>longitude and latitude</returns>
        /// 
        private const string APIKEY = "AIzaSyCLB9sMk0s-Jz9TE84ii1lQg2U3FS5PFKQ";
        private static readonly System.Net.Http.HttpClient _httpClient = new HttpClient();

        public static Geolocation Locate(string query)
        {
            string json = string.Empty;
            string error = string.Empty;
            double latitude = 0.0;
            double longitude = 0.0;
            string fulladdress = string.Empty;
            string city = string.Empty;

            // We use WebClient to get the information we need from the google map API
            using (WebClient web = new WebClient())
            {
                try
                {
                    web.Headers["Content-type"] = "application/json";
                    web.Encoding = Encoding.UTF8;
                    var url = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + query + @"&key=" + APIKEY + @"&sensor=true";

                    using (Stream strm = web.OpenRead(url))
                    {
                        StreamReader sr = new StreamReader(strm);
                        json = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                // We dezerilize the root object to get values to the model "RootObjectAddressComponent"
                JavaScriptSerializer ser = new JavaScriptSerializer();
                RootObjectAddressComponent root = ser.Deserialize<RootObjectAddressComponent>(json);

                latitude = root.results[0].geometry.location.lat;
                longitude = root.results[0].geometry.location.lng;
                fulladdress = root.results[0].formatted_address;
                city = root.results[0].address_components[1].short_name;
            }
            // After setting values from the json object we can return a Geolocation object with values.
            return new Geolocation
            {
                Address = fulladdress,
                City = city,
                Latitude = latitude,
                Longitude = longitude
            };
        }
        public static Adress GetAdress(object latitud, object longitud)
        {
            string json = string.Empty;
            string error = string.Empty;
            string lat = string.Empty;
            string lng = string.Empty;
            string fulladdress = string.Empty;
            string city = string.Empty;

            double lati = Convert.ToDouble(latitud);
            double longi = Convert.ToDouble(longitud);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");
            lat = lati.ToString();
            lng = longi.ToString();
   

            // We use WebClient to get the information we need from the google map API
            using (WebClient web = new WebClient())
            {
                try
                {
                    web.Headers["Content-type"] = "application/json";
                    web.Encoding = Encoding.UTF8;
                    var url = @"https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + @"&key=" + APIKEY + @"&sensor=true";

                    using (Stream strm = web.OpenRead(url))
                    {
                        StreamReader sr = new StreamReader(strm);
                        json = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                // We dezerilize the root object to get values to the model "RootObjectAddressComponent"
                JavaScriptSerializer ser = new JavaScriptSerializer();
                RootObjectAddressComponent root = ser.Deserialize<RootObjectAddressComponent>(json);

                fulladdress = root.results[0].formatted_address;
                city = root.results[0].address_components[1].short_name;
            }
            // After setting values from the json object we can return a Geolocation object with values.
            return new Adress
            {
                Address = fulladdress,
                City = city,
            };
        }
    }
}