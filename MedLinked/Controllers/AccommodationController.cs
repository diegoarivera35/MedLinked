using MedLinked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;
using MedLinked.Models.ViewModels;

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

        // GET: Accommodation/Details/2
        public ActionResult Details(int id)
        {
            DetailsAccommodation ViewModel = new DetailsAccommodation();

            //objective: communicate with our Accommodation data api to retrieve one Accommodation
            //curl https://localhost:44376/api/AccommodationData/FindAccommodation/{id}

            string url = "AccommodationData/FindAccommodation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AccommodationDto SelectedAccommodation = response.Content.ReadAsAsync<AccommodationDto>().Result;

            ViewModel.SelectedAccommodation = SelectedAccommodation;

            //show all medicalprocedures under the care of this accommodation
            url = "medicalproceduredata/listmedicalproceduresforaccommodation/" + id;
            response = client.GetAsync(url).Result;
            //IEnumerable<MedicalProcedureDto> KeptMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            //ViewModel.KeptMedicalProcedures = KeptMedicalProcedures;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }



        // GET: Accommodation/New
        public ActionResult New()
        {
            return View();
        }



        // POST: Accommodation/Create
        [HttpPost]
        public ActionResult Create(Accommodation Accommodation)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Accommodation.AccommodationName);
            //objective: add a new Accommodation into our system using the API
            //curl -H "Content-Type:application/json" -d @Accommodation.json https://localhost:44376/api/Accommodationdata/addAccommodation 
            string url = "AccommodationData/AddAccommodation";


            string jsonpayload = jss.Serialize(Accommodation);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// A function that grabs the details of the selected accommodation and renders the edit form view
        /// </summary>
        /// <param name="id">The accommodation to be edited</param>
        /// <returns>
        /// Returns the edit booking form page with the accommodation data. 
        /// </returns>
        /// 

        // GET: Accommodation/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the accommodation information

            //objective: communicate with our accommodation data api to retrieve one accommodation
            //curl https://localhost:44324/api/AccommodationData/FindAccommodation/{id}

            string url = "AccommodationData/FindAccommodation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AccommodationDto selectedAccommodation = response.Content.ReadAsAsync<AccommodationDto>().Result;

            return View(selectedAccommodation);
        }

        /// <summary>
        /// A function to update the details of the selected accommodation
        /// </summary>
        /// <param name="id">The accommodation id</param>
        /// <param name="accommodation">The accommodation object</param>
        /// <returns>
        /// The list view page after updating the accommodation details
        /// </returns>
        /// 

        // POST: Accommodation/Update/5
        [HttpPost]
        public ActionResult Update(int id, Accommodation accommodation)
        {
            try
            {
                // Serialize into JSON
                // Send the request to the API

                string url = "AccommodationData/UpdateAccommodation/" + id;


                string jsonpayload = jss.Serialize(accommodation);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/AccommodationData/UpdateAccommodation/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Function to show the delete confirm dialog box.
        /// </summary>
        /// <param name="id">The accommodation id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Booking/DeleteConfirm/2
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AccommodationData/FindAccommodation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AccommodationDto selectedAccommodation = response.Content.ReadAsAsync<AccommodationDto>().Result;
            return View(selectedAccommodation);
        }

        /// <summary>
        /// Function to delete the selected accommodation entry.
        /// </summary>
        /// <param name="id">The accommodation id</param>
        /// <returns>
        /// The list of accommodations if successful else, the error view page.
        /// </returns>

        // POST: Accommodation/Delete/2
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "AccommodationData/DeleteAccommodation/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}