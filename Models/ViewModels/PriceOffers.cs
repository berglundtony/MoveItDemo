using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoveItDemo.Models.ViewModels
{
    public class PriceOffers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Distance { get; set; }
        public int? ResidenceArea { get; set; }
        public int? WindBaseMentArea { get; set; }
        public bool? PianoStatus { get; set; }
        public bool? PackingStatus { get; set; }
        public decimal? Price { get; set; }

    }
}