using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoveItDemo;
using MoveItDemo.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace MoveItDemo.Controllers
{
    public class PriceSuggestionsController : Controller
    {
        private static DataHandlerEntities db = new DataHandlerEntities();
        private static DefaultDataEntities defaultdb = new DefaultDataEntities();


        // GET: PriceSuggestions
        [Authorize]
        public ActionResult PriceOffers()
        {
            var CurrentUser = User.Identity.GetUserName();
            IQueryable<PriceOffert> _po = db.PriceOfferts.Where(l => l.UserName == CurrentUser);

            var _priceoffers = new List<PriceOffers>();
            foreach (var i in _po)
            {
                _priceoffers.Add(
                new PriceOffers
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    UserName = i.UserName,
                    FromName = i.FromName,
                    ToName = i.ToName,
                    Distance = i.Distance,
                    ResidenceArea = i.ResidenceArea,
                    WindBaseMentArea = i.WindBaseMentArea,
                    PianoStatus = i.PianoStatus,
                    PackingStatus = i.PackingStatus,
                    Price = i.Price
                }); ;
            }
            return View(_priceoffers);
        }
        [Authorize]
        // GET: PriceSuggestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
