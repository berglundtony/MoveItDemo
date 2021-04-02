using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoveItDemo.Models.ViewModels
{
    public class PriceSuggestion
    {
        public int Id{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Distance { get; set; }
        public Nullable<int> ResidenceArea { get; set; }
        public Nullable<int> WindBaseMentArea { get; set; }
        public Nullable<bool> PianoStatus { get; set; }
        public Nullable<bool> PackingStatus { get; set; }
    }
}