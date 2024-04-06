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
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Doctors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Doctors in the database, including their associated doctor.
        /// </returns>
        /// <example>
        /// GET: api/DoctorData/ListDoctors
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DoctorDto))]
        public IHttpActionResult ListDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();
            List<DoctorDto> DoctorDtos = new List<DoctorDto>();

            Doctors.ForEach(k => DoctorDtos.Add(new DoctorDto()
            {
                DoctorID = k.DoctorID,
                DoctorFirstName = k.DoctorFirstName,
                DoctorLastName = k.DoctorLastName,
                DoctorPhone = k.DoctorPhone,
                DoctorEmail = k.DoctorEmail,
                DoctorSpecialization = k.DoctorSpecialization,

                

            }));

            return Ok(DoctorDtos);
        }


        /// <summary>
        /// Returns all Doctors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Doctor in the system matching up to the Doctor ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Doctor</param>
        /// <example>
        /// GET: api/DoctorData/FindDoctor/5
        /// </example>
        [ResponseType(typeof(DoctorDto))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorID = Doctor.DoctorID,
                DoctorFirstName = Doctor.DoctorFirstName,
                DoctorLastName = Doctor.DoctorLastName,
                DoctorPhone = Doctor.DoctorPhone,
                DoctorEmail = Doctor.DoctorEmail,
                DoctorSpecialization = Doctor.DoctorSpecialization
            };
            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(DoctorDto);
        }

        /// <summary>
        /// Updates a particular Doctor in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Doctor ID primary key</param>
        /// <param name="Doctor">JSON FORM DATA of an Doctor</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/UpdateDoctor/5
        /// FORM DATA: Doctor JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDoctor(int id, Doctor Doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Doctor.DoctorID)
            {

                return BadRequest();
            }

            db.Entry(Doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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
        /// Adds an Doctor to the system
        /// </summary>
        /// <param name="Doctor">JSON FORM DATA of an Doctor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Doctor ID, Doctor Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/AddDoctor
        /// FORM DATA: Doctor JSON Object
        /// </example>
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult AddDoctor(Doctor Doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(Doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Doctor.DoctorID }, Doctor);
        }

        /// <summary>
        /// A function to delete a specific doctor from the database using the id.
        /// It uses the Doctor model class to delete the data.
        /// </summary>
        /// 
        /// <example>
        /// POST: curl -d "" https://localhost:44375/api/DataData/DeleteDoctor/6
        /// 
        /// Here, "" is the post data and it being empty means no post data was passed.
        /// </example>
        /// 
        /// <param name="id">The doctor id</param>
        /// 
        /// <returns>
        /// The selected data entry with the id is deleted from the database.
        /// </returns>
        /// 

        // POST: api/DoctorData/DeleteDoctor/6
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
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

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorID == id) > 0;
        }
    }
}