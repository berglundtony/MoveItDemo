using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoveItDemo.Functions;
using MoveItDemo.Models.ViewModels;
using System.Web;
using System.Security.Claims;
using MoveItDemo.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading;
using Auth0.AuthenticationApi;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MoveItDemo.Controllers
{
    public class PriceOfferController : ApiController
    {
        PriceOfferController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        private DataHandlingEntities db = new DataHandlingEntities();
        private DefaultConnectionEntities default_db = new DefaultConnectionEntities();

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }



        public PriceOffert GetOffer([FromBody] PriceSuggestion pricesuggestion)
        {
            try
            {
                var suggestion = new PriceOffert();
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
                return suggestion;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                return null;
            }
        }

        // GET: api/PriceOfferts
        public IQueryable<PriceOffert> GetPriceOffers()
        {
            return db.PriceOfferts;
        }

        // GET: api/PriceOffert/5
        [ResponseType(typeof(PriceOffert))]
        public async Task<IHttpActionResult>GetLatestPriceOffer()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            // Retrieve the access_token claim which we saved in the OnTokenValidated event
            var CurrentUser = default_db.AspNetUsers.Select(x => x.UserName).ToString();
            object httpContext;
            ActionContext.Request.Properties.TryGetValue("MS_HttpContext", out httpContext);

            string userName;
            string userId;
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                userName = HttpContext.Current.User.Identity.Name;
                userId = HttpContext.Current.User.Identity.GetUserId();
            }
    
            string currentUserId = User.Identity.GetUserId();
            int  latestid =  db.PriceOfferts.Where(l => l.UserName == CurrentUser).Max(i => i.Id);

            return (Ok(latestid));
        }

        // GET: api/PriceOfferts/5
        [ResponseType(typeof(PriceOffert))]
        public async Task<IHttpActionResult> GetPriceOffer(int id)
        {
            PriceOffert priceOffert = await db.PriceOfferts.FindAsync(id);
            if (priceOffert == null)
            {
                return NotFound();
            }

            return Ok(priceOffert);
        }

        // PUT: api/PriceOfferts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPriceOffer(int id, PriceOffert priceOffer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceOffer.Id)
            {
                return BadRequest();
            }

            db.Entry(priceOffer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceOfferExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PriceOfferts
        [ResponseType(typeof(PriceOffert))]
        public async Task<IHttpActionResult> PostPriceOffer(PriceOffert priceOffer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var priceRequest = new PriceOffert();
            priceRequest = priceOffer;
            decimal distance = PriceFunctions.ReplaceKilometerFromDistance(priceRequest.Distance);
            priceRequest.Distance = distance.ToString();
            db.PriceOfferts.Add(priceRequest);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = priceRequest.Id }, priceRequest);
        }

        // DELETE: api/PriceOfferts/5
        [ResponseType(typeof(PriceOffert))]
        public async Task<IHttpActionResult> DeletePriceOffer(int id)
        {
            PriceOffert priceOffer = await db.PriceOfferts.FindAsync(id);
            if (priceOffer == null)
            {
                return NotFound();
            }

            db.PriceOfferts.Remove(priceOffer);
            await db.SaveChangesAsync();

            return Ok(priceOffer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        
        }

        private bool PriceOfferExists(int id)
        {
            return db.PriceOfferts.Count(e => e.Id == id) > 0;
        }
    }
}