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
    public class AccommodationController : Controller
    {
        // GET: Accommodation

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AccommodationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44396/api/");
        }

        // GET: Accommodation/List
        public ActionResult List()
        {
            //objective: communicate with our Accommodation data api to retrieve a list of Accommodations
            //curl https://localhost:44396/api/Accommodationdata/listaccommodations


            string url = "accommodationdata/listaccommodations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AccommodationDto> Accommodations = response.Content.ReadAsAsync<IEnumerable<AccommodationDto>>().Result;

            return View(Accommodations);
        }


    }
}