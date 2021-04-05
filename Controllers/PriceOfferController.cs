using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoveItDemo.Functions;
using MoveItDemo.Models.ViewModels;


namespace MoveItDemo.Controllers
{
    public class PriceOfferController : ApiController
    {
        private DataHandlingEntities db = new DataHandlingEntities();

        // GET: api/GetPriceOffer?id=5
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

        // POST: api/PriceOffers
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

        // POST: api/Order
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> Order([FromBody]OrderMove ordermove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderm= new Order 
            { 
                Date = DateTime.Now,
                OffertId = ordermove.Id,
                UserName = ordermove.UserName
            };
            db.Orders.Add(orderm);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderm.Id }, orderm);
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