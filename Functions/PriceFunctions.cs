using MoveItDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static string msgpiano = "";
        private static string msgpackingstatus = "";

        public static decimal?  GetPrice(string distance, int? residencearea, int? windbasementarea, bool? pianoStatus, bool? packingstatus)
        {

            message = "";
            Message(message);

            msgpiano = "";
            MessagePiano(msgpiano);

            msgpackingstatus = "";
            MessagePackingStatus(msgpackingstatus);

            decimal todecimal = ReplaceKilometerFromDistance(distance);
            decimal? price;
            decimal result = Math.Round(todecimal, 0);
            int dist = Convert.ToInt32(result);

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
                if(price > 0)
                {
                    price += 5000;

                    msgpiano = "";
                    MessagePiano(msgpiano);

                }
                else
                {
                    price = 0;
                    msgpiano = "Your other requests are not confirmed, so you have to make a new try to move the piano.";
  
                    MessagePiano(msgpiano);
                }
      
            }
            if (packingstatus == true)
            {
                msgpackingstatus = "Have no information of what this will cost with packing, please contact MoveIt for information.";
                //price += 2000;
                MessagePackingStatus(msgpackingstatus);
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
            else if (area >= 450 && area < 501)
            {
                car = 10;
                price = car * price;
            }
            else
            {
                message = "The limit for the basementarea is 500 kvm.";
                Message(message);
                price = 0;
            }
            return price;
        }

        internal static string Message(string message)
        {
            HttpContext.Current.Session["Message"] = message;
            return message;
        }

        internal static string MessagePiano(string msgpiano)
        {
            HttpContext.Current.Session["MessagePiano"] = msgpiano;
            return msgpiano;
        }

        internal static string MessagePackingStatus(string msgpackingstatus)
        {
            HttpContext.Current.Session["MessagePackingStatus"] = msgpackingstatus;
            return msgpackingstatus;
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