using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoveItDemo.Models.ViewModels
{
    public class OrderMove
    {
        public int Id { get; set; }
        public int OffertId { get; set; }
        public string UserName { get; set; }
        public DateTime BookingDate { get; set; }
    }
}