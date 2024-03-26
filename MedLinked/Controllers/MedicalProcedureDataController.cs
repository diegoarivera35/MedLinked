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
        /// CONTENT: all MedicalProcedures in the database, including their associated doctor.
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