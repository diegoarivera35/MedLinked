using MedLinked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace MedLinked.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44396/api/");
        }

        // GET: Doctor/List
        public ActionResult List()
        {
            //objective: communicate with our Doctor data api to retrieve a list of Doctors
            //curl https://localhost:44396/api/Doctordata/listdoctors


            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> Doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            //Debug.WriteLine("Number of Doctors received : ");
            //Debug.WriteLine(Doctors.Count());


            return View(Doctors);
        }


    }
}