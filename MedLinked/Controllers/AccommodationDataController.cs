using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MedLinked.Models;
using System.Diagnostics;

namespace MedLinked.Controllers
{
    public class AccommodationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Accommodations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Accommodations in the database, including their associated doctor.
        /// </returns>
        /// <example>
        /// GET: api/AccommodationData/ListAccommodations
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AccommodationDto))]
        public IHttpActionResult ListAccommodations()
        {
            List<Accommodation> Accommodations = db.Accommodations.ToList();
            List<AccommodationDto> AccommodationDtos = new List<AccommodationDto>();

            Accommodations.ForEach(k => AccommodationDtos.Add(new AccommodationDto()
            {
                AccommodationID = k.AccommodationID,
                AccommodationName = k.AccommodationName,
                Address = k.Address,
                Destination = k.Destination,
                Departure = k.Departure,
                Arrival = k.Arrival,
                AccommodationCost = k.AccommodationCost,

            }));

            return Ok(AccommodationDtos);
        }


        /// <summary>
        /// Returns all Accommodations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Accommodation in the system matching up to the Accommodation ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Accommodation</param>
        /// <example>
        /// GET: api/AccommodationData/FindAccommodation/5
        /// </example>
        [ResponseType(typeof(AccommodationDto))]
        [HttpGet]
        public IHttpActionResult FindAccommodation(int id)
        {
            Accommodation Accommodation = db.Accommodations.Find(id);
            AccommodationDto AccommodationDto = new AccommodationDto()
            {
                AccommodationID = Accommodation.AccommodationID,
                AccommodationName = Accommodation.AccommodationName,
                Address = Accommodation.Address,
                Destination = Accommodation.Destination,
                Departure = Accommodation.Departure,
                Arrival = Accommodation.Arrival,
                AccommodationCost = Accommodation.AccommodationCost,
            };
            if (Accommodation == null)
            {
                return NotFound();
            }

            return Ok(AccommodationDto);
        }

        /// <summary>
        /// Updates a particular Accommodation in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Accommodation ID primary key</param>
        /// <param name="Accommodation">JSON FORM DATA of an Accommodation</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/AccommodationData/UpdateAccommodation/5
        /// FORM DATA: Accommodation JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAccommodation(int id, Accommodation Accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Accommodation.AccommodationID)
            {

                return BadRequest();
            }

            db.Entry(Accommodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(id))
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

        /// <summary>
        /// Adds an Accommodation to the system
        /// </summary>
        /// <param name="Accommodation">JSON FORM DATA of an Accommodation</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Accommodation ID, Accommodation Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/AccommodationData/AddAccommodation
        /// FORM DATA: Accommodation JSON Object
        /// </example>
        [ResponseType(typeof(Accommodation))]
        [HttpPost]
        public IHttpActionResult AddAccommodation(Accommodation Accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accommodations.Add(Accommodation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Accommodation.AccommodationID }, Accommodation);
        }

        /// <summary>
        /// A function to delete a specific accommodation from the database using the id.
        /// It uses the Accommodation model class to delete the data.
        /// </summary>
        /// 
        /// <example>
        /// POST: curl -d "" https://localhost:44375/api/AccommodationData/DeleteAccommodation/3
        /// 
        /// Here, "" is the post data and it being empty means no post data was passed.
        /// </example>
        /// 
        /// <param name="id">The accommodation id</param>
        /// 
        /// <returns>
        /// The selected data entry with the id is deleted from the database.
        /// </returns>
        /// 

        // POST: api/AccommodationData/DeleteAccommodation/2
        [ResponseType(typeof(Accommodation))]
        [HttpPost]
        public IHttpActionResult DeleteAccommodation(int id)
        {
            Accommodation accommodation = db.Accommodations.Find(id);
            if (accommodation == null)
            {
                return NotFound();
            }

            db.Accommodations.Remove(accommodation);
            db.SaveChanges();

            // Not returning anything in the OK()
            // Passing the object showed "null" values
            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccommodationExists(int id)
        {
            return db.Accommodations.Count(e => e.AccommodationID == id) > 0;
        }
    }
}