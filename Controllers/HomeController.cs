using MoveItDemo.Functions;
using MoveItDemo.Models;
using MoveItDemo.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MoveItDemo.Controllers
{
    public class HomeController : Controller
    {
        [System.Web.Http.Authorize]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// The main thing is to calculate the price for the moving depending of the business rules.
        /// </summary>
        /// <param name="pricesuggestion"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public JsonResult GetOffert([FromBody] PriceSuggestion pricesuggestion)
        {
            try
            {
                var suggestion = new PriceOffert();
                suggestion.Id = pricesuggestion.Id;
                suggestion.FirstName = pricesuggestion.FirstName;
                suggestion.LastName = pricesuggestion.LastName;
                suggestion.UserName = pricesuggestion.UserName;
                suggestion.ToName = pricesuggestion.ToName;
                suggestion.FromName = pricesuggestion.FromName;
                suggestion.Distance = pricesuggestion.Distance;
                suggestion.ResidenceArea = pricesuggestion.ResidenceArea;
                suggestion.WindBaseMentArea = pricesuggestion.WindBaseMentArea;
                suggestion.PianoStatus = pricesuggestion.PianoStatus;
                suggestion.PackingStatus = pricesuggestion.PackingStatus;
                suggestion.Price = PriceFunctions.GetPrice(suggestion.Distance, suggestion.ResidenceArea, suggestion.WindBaseMentArea, suggestion.PianoStatus, suggestion.PackingStatus);
                return Json(suggestion, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                ModelState.AddModelError("Error", ex.Message);

                return null;
            }

        }

        /// <summary>
        /// The method returns the coordinates the adress and the city we got from the  daparture destinition and the arriving destination.
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="toaddress"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult GetCoordinates(string fromaddress, string toaddress)
        {
            List<Geolocation> _geo = new List<Geolocation>();

            if (fromaddress != "" && toaddress != "")
            {
                _geo.Add(MapFunctions.Locate(fromaddress));
                _geo.Add(MapFunctions.Locate(toaddress));
            }
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(_geo);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public JsonResult GetAdress([FromBody] IEnumerable<double> location)
        {
            Adress address = new Adress();

            IEnumerable<object> lat = location.Cast<object>();
            var latitude = lat.First();
            var longitude = lat.Last();

                address = (MapFunctions.GetAdress(latitude, longitude));
  
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(address);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
