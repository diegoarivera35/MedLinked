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
    public class PatientController : Controller
    {
        // GET: Patient

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44396/api/");
        }

        // GET: Patient/List
        public ActionResult List()
        {
            //objective: communicate with our Patient data api to retrieve a list of Patients
            //curl https://localhost:44396/api/Patientdata/listpatients


            string url = "patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> Patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            //Debug.WriteLine("Number of Patients received : ");
            //Debug.WriteLine(Patients.Count());


            return View(Patients);
        }


    }
}