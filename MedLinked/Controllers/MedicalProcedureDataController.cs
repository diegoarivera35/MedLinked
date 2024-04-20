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
    public class MedicalProcedureDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all MedicalProcedures in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all MedicalProcedures in the database, including their associated medicalprocedure.
        /// </returns>
        /// <example>
        /// GET: api/MedicalProcedureData/ListMedicalProcedures
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MedicalProcedureDto))]
        public IHttpActionResult ListMedicalProcedures()
        {
            List<MedicalProcedure> MedicalProcedures = db.MedicalProcedures.ToList();
            List<MedicalProcedureDto> MedicalProcedureDtos = new List<MedicalProcedureDto>();

            MedicalProcedures.ForEach(k => MedicalProcedureDtos.Add(new MedicalProcedureDto()
            {
                MedicalProcedureID = k.MedicalProcedureID,
                MedicalProcedureName = k.MedicalProcedureName,
                MedicalCenter = k.MedicalCenter,
                MedicalProcedureDate = k.MedicalProcedureDate,
                MedicalProcedureCost = k.MedicalProcedureCost

            }));

            return Ok(MedicalProcedureDtos);
        }

        /// <summary>
        /// Retreives the medical procedures info for a particular doctor
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all procedures in the database, including their associated patient who made the booking
        /// </returns>
        /// <param name="id">The Doctor ID</param>
        /// <example>
        /// GET: api/MedicalProcedureData/listmedicalproceduresfordoctor/3
        /// </example>
        /// 
        /// <steps>
        ///     - First the api was created to list the information to be shared
        ///     - The query is the line with Where: List<MedicalProcedure> Procedures = db.Procedures.Where(a => a.DoctorId == id).ToList();
        ///     - Then, create the ViewModel and import the DTO classes
        ///     - In the display view, use the new ViewModel object to access the data
        /// </steps>
        /// 
        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        public IHttpActionResult listmedicalproceduresfordoctor(int id)
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
        /// Returns all MedicalProcedures in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An MedicalProcedure in the system matching up to the MedicalProcedure ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the MedicalProcedure</param>
        /// <example>
        /// GET: api/MedicalProcedureData/FindMedicalProcedure/5
        /// </example>
        [ResponseType(typeof(MedicalProcedureDto))]
        [HttpGet]
        public IHttpActionResult FindMedicalProcedure(int id)
        {
            MedicalProcedure MedicalProcedure = db.MedicalProcedures.Find(id);
            MedicalProcedureDto MedicalProcedureDto = new MedicalProcedureDto()
            {
                MedicalProcedureID = MedicalProcedure.MedicalProcedureID,
                MedicalProcedureName = MedicalProcedure.MedicalProcedureName,
                MedicalCenter = MedicalProcedure.MedicalCenter,
                MedicalProcedureDate = MedicalProcedure.MedicalProcedureDate,
                MedicalProcedureCost = MedicalProcedure.MedicalProcedureCost
            };
            if (MedicalProcedure == null)
            {
                return NotFound();
            }

            return Ok(MedicalProcedureDto);
        }

        /// <summary>
        /// Updates a particular MedicalProcedure in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the MedicalProcedure ID primary key</param>
        /// <param name="MedicalProcedure">JSON FORM DATA of an MedicalProcedure</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/MedicalProcedureData/UpdateMedicalProcedure/5
        /// FORM DATA: MedicalProcedure JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMedicalProcedure(int id, MedicalProcedure MedicalProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != MedicalProcedure.MedicalProcedureID)
            {

                return BadRequest();
            }

            db.Entry(MedicalProcedure).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalProcedureExists(id))
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
        /// Adds an MedicalProcedure to the system
        /// </summary>
        /// <param name="MedicalProcedure">JSON FORM DATA of an MedicalProcedure</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: MedicalProcedure ID, MedicalProcedure Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/MedicalProcedureData/AddMedicalProcedure
        /// FORM DATA: MedicalProcedure JSON Object
        /// </example>
        [ResponseType(typeof(MedicalProcedure))]
        [HttpPost]
        public IHttpActionResult AddMedicalProcedure(MedicalProcedure MedicalProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalProcedures.Add(MedicalProcedure);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = MedicalProcedure.MedicalProcedureID }, MedicalProcedure);
        }




        /// <summary>
        /// A function to delete a specific medicalprocedure from the database using the id.
        /// It uses the MedicalProcedure model class to delete the data.
        /// </summary>
        /// 
        /// <example>
        /// POST: curl -d "" https://localhost:44375/api/DataData/DeleteMedicalProcedure/6
        /// 
        /// Here, "" is the post data and it being empty means no post data was passed.
        /// </example>
        /// 
        /// <param name="id">The medicalprocedure id</param>
        /// 
        /// <returns>
        /// The selected data entry with the id is deleted from the database.
        /// </returns>
        /// 

        // POST: api/MedicalProcedureData/DeleteMedicalProcedure/6
        [ResponseType(typeof(MedicalProcedure))]
        [HttpPost]
        public IHttpActionResult DeleteMedicalProcedure(int id)
        {
            MedicalProcedure medicalprocedure = db.MedicalProcedures.Find(id);
            if (medicalprocedure == null)
            {
                return NotFound();
            }

            db.MedicalProcedures.Remove(medicalprocedure);
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

        private bool MedicalProcedureExists(int id)
        {
            return db.MedicalProcedures.Count(e => e.MedicalProcedureID == id) > 0;
        }
    }
}