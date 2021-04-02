using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoveItDemo.Models
{
    public class Geolocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Message { get; set; }
    }

    public class Adress
    {
        public string Address { get; set; }
        public string City { get; set; }
    }
}