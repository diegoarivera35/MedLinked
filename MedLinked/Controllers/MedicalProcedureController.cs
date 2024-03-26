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
    public class MedicalProcedureController : Controller
    {
        // GET: MedicalProcedure

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MedicalProcedureController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44396/api/");
        }

        // GET: MedicalProcedure/List
        public ActionResult List()
        {
            //objective: communicate with our MedicalProcedure data api to retrieve a list of MedicalProcedures
            //curl https://localhost:44396/api/MedicalProceduredata/listmedicalprocedures


            string url = "medicalproceduredata/listmedicalprocedures";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<MedicalProcedureDto> MedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;
            //Debug.WriteLine("Number of MedicalProcedures received : ");
            //Debug.WriteLine(MedicalProcedures.Count());


            return View(MedicalProcedures);
        }


    }
}