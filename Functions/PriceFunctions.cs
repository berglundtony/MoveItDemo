using MoveItDemo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Web;

namespace MoveItDemo.Functions
{
    /// <summary>
    /// We get the price depending on the business rules about distance and pianostatus.
    /// </summary>
    public static class PriceFunctions
    {
        private static string message = "";
        
        public static decimal?  GetPrice(string distance, int? residencearea, int? windbasementarea, bool? pianoStatus, bool? packingstatus)
        {
            
            int dist = 0;
            decimal result = 0m;
            decimal? price = 0m;
            decimal todecimal = 0m;

            todecimal = ReplaceKilometerFromDistance(distance);
     
            result = Math.Round(todecimal, 0);
            dist = Convert.ToInt32(result);

            if (dist < 50)
            {
                price = 1000 + (dist * 10);
            }
            else if (dist > 50 && dist < 100)
            {
                price = 5000 + (dist * 8);
            }
            else
            {
                price = 10000 + (dist * 7);
            }

            price = PriceByResideceAndWindBasementArea(residencearea, windbasementarea, price);

            if (pianoStatus == true)
            {
                price += 5000;
            }
            if (packingstatus == true)
            {
                message = "Har ingen uppgift på vad detta skulle kosta med packning, antar att det beror på antal timmar och hur stor yta det gäller.";
                //price += 2000;
                Message(message);
            }
            return price;
        }

        /// <summary>
        /// Here we calculate the Price depending on residence area and windbasementarea
        /// </summary>
        /// <param name="residencearea"></param>
        /// <param name="windbasementarea"></param>
        /// <returns></returns>

        private static decimal? PriceByResideceAndWindBasementArea(int? residencearea, int? windbasementarea, decimal? price)
        {
            int car = 1;
            int? area = 0;

            area = CalculateWindBasementArea(windbasementarea);
            area += residencearea;

            if (area < 50)
            {
                price = car * price;
            }
            else if (area >= 50 && area < 100)
            {
                car = 2;
                price = car * price;
            }
            else if (area >= 100 && area < 150)
            {
                car = 2;
                price = car * price;
            }
            else if (area <= 100 && area < 150)
            {
                car = 3;
                price = car * price;
            }
            else if (area >= 150 && area < 200)
            {
                car = 4;
                price = car * price;
            }
            else if (area >= 200 && area < 250)
            {
                car = 5;
                price = car * price;
            }
            else if (area >= 250 && area < 300)
            {
                car = 6;
                price = car * price;
            }
            else if (area >= 300 && area < 350)
            {
                car = 7;
                price = car * price;
            }
            else if (area >= 350 && area < 400)
            {
                car = 8;
                price = car * price;
            }
            else if (area >= 400 && area < 450)
            {
                car = 9;
                price = car * price;
            }
            else if (area >= 450 && area < 500)
            {
                car = 10;
                price = car * price;
            }
            else
            {
                message = "vi har inte så många bilar så du får göra en till beställning en annan dag";
            }

            Message(message);

            return price;
        }

        internal static string Message(string message)
        {
            HttpContext.Current.Session["Message"] = message;
            return message;
        }

        private static int? CalculateWindBasementArea(int? windbasementarea)
        {
            int? area = windbasementarea * 2;
            return area;
        }
        internal static decimal ReplaceKilometerFromDistance(string distance)
        {
            string strdecimal = distance.Replace("km", string.Empty);
            decimal todecimal = Decimal.Parse(strdecimal);

            return todecimal;
        }
    }
}