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
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Bookings in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Bookings in the database, including their associated doctor.
        /// </returns>
        /// <example>
        /// GET: api/BookingData/ListBookings
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        public IHttpActionResult ListBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(k => BookingDtos.Add(new BookingDto()
            {
                BookingID = k.BookingID,
                Status = k.Status,
                BookingDate = k.BookingDate,
                PatientFirstName = k.Patient.PatientFirstName,
                PatientLastName = k.Patient.PatientLastName,
                DoctorFirstName = k.Doctor.DoctorFirstName,
                DoctorLastName = k.Doctor.DoctorLastName,
                MedicalProcedureName = k.MedicalProcedure.MedicalProcedureName,
                MedicalProcedureDate = k.MedicalProcedure.MedicalProcedureDate,
                AccommodationName = k.Accommodation.AccommodationName,
                Departure = k.Accommodation.Departure,
                GrandTotal = k.GrandTotal
            }));

            return Ok(BookingDtos);
        }

        /// <summary>
        /// Retreives the bookings info for a particular patient
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all bookings in the database, including their associated patient who made the booking
        /// </returns>
        /// <param name="id">The Patient ID</param>
        /// <example>
        /// GET: api/BookingData/ListBookingsForPatients/3
        /// </example>
        /// 
        /// <steps>
        ///     - First the api was created to list the information to be shared
        ///     - The query is the line with Where: List<Booking> Bookings = db.Bookings.Where(a => a.PatientId == id).ToList();
        ///     - Then, create the ViewModel and import the DTO classes
        ///     - In the display view, use the new ViewModel object to access the data
        /// </steps>
        /// 
        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        public IHttpActionResult ListBookingsForPatients(int id)
        {
            //SQL Equivalent:
            //Select * from bookings where bookings.PatientId = {id}
            List<Booking> Bookings = db.Bookings.Where(c => c.PatientID == id).ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(c => BookingDtos.Add(new BookingDto()
            {
                BookingID = c.BookingID,
                // using the entity relationship for these Patient entity columns
                PatientFirstName = c.Patient.PatientFirstName,
                PatientLastName = c.Patient.PatientLastName,
                // using the entity relationship for these Doctor entity columns
                DoctorFirstName = c.Doctor.DoctorFirstName,
                DoctorLastName = c.Doctor.DoctorLastName,
                // using the entity relationship for these MedicalProcedure entity columns
                MedicalProcedureName = c.MedicalProcedure.MedicalProcedureName,
                MedicalProcedureDate = c.MedicalProcedure.MedicalProcedureDate,
                // using the entity relationship for these Accommodation entity columns
                AccommodationName = c.Accommodation.AccommodationName,
                Departure = c.Accommodation.Departure,
                Status = c.Status,
                GrandTotal = c.GrandTotal
            }));

            return Ok(BookingDtos);
        }


        /// <summary>
        /// Returns all Bookings in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Booking in the system matching up to the Booking ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Booking</param>
        /// <example>
        /// GET: api/BookingData/FindBooking/5
        /// </example>
        [ResponseType(typeof(BookingDto))]
        [HttpGet]
        public IHttpActionResult FindBooking(int id)
        {
            Booking Booking = db.Bookings.Find(id);
            BookingDto BookingDto = new BookingDto()
            {
                BookingID = Booking.BookingID,
                Status = Booking.Status,
                BookingDate = Booking.BookingDate,
                PatientFirstName = Booking.Patient.PatientFirstName,
                PatientLastName = Booking.Patient.PatientLastName,
                DoctorFirstName = Booking.Doctor.DoctorFirstName,
                DoctorLastName = Booking.Doctor.DoctorLastName,
                MedicalProcedureName = Booking.MedicalProcedure.MedicalProcedureName,
                MedicalProcedureDate = Booking.MedicalProcedure.MedicalProcedureDate,
                AccommodationName = Booking.Accommodation.AccommodationName,
                Departure = Booking.Accommodation.Departure,
                GrandTotal = Booking.GrandTotal
            };
            if (Booking == null)
            {
                return NotFound();
            }

            return Ok(BookingDto);
        }

        /// <summary>
        /// Updates a particular Booking in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Booking ID primary key</param>
        /// <param name="Booking">JSON FORM DATA of an Booking</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/UpdateBooking/5
        /// FORM DATA: Booking JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBooking(int id, Booking Booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Booking.BookingID)
            {

                return BadRequest();
            }

            db.Entry(Booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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
        /// Adds an Booking to the system
        /// </summary>
        /// <param name="Booking">JSON FORM DATA of an Booking</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Booking ID, Booking Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/AddBooking
        /// FORM DATA: Booking JSON Object
        /// </example>
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult AddBooking(Booking Booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(Booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Booking.BookingID }, Booking);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}