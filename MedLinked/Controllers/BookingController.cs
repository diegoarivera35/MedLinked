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
    public class BookingController : Controller
    {
        // GET: Booking

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44396/api/");
        }

        // GET: Booking/List
        public ActionResult List()
        {
            //objective: communicate with our Booking data api to retrieve a list of Bookings
            //curl https://localhost:44396/api/Bookingdata/listbookings


            string url = "bookingdata/listbookings";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingDto> Bookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            return View(Bookings);
        }


    }
}