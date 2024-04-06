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

        // GET: MedicalProcedure/Details/5
        public ActionResult Details(int id)
        {
            DetailsMedicalProcedure ViewModel = new DetailsMedicalProcedure();

            //objective: communicate with our MedicalProcedure data api to retrieve one MedicalProcedure
            //curl https://localhost:44376/api/MedicalProceduredata/findmedicalprocedure/{id}

            string url = "medicalproceduredata/findMedicalProcedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            MedicalProcedureDto SelectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            Debug.WriteLine("MedicalProcedure received : ");
            Debug.WriteLine(SelectedMedicalProcedure.MedicalProcedureName);

            ViewModel.SelectedMedicalProcedure = SelectedMedicalProcedure;

            //show all medicalprocedures under the care of this medicalprocedure
            url = "medicalproceduredata/listmedicalproceduresformedicalprocedure/" + id;
            response = client.GetAsync(url).Result;
            //IEnumerable<MedicalProcedureDto> KeptMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            //ViewModel.KeptMedicalProcedures = KeptMedicalProcedures;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: MedicalProcedure/New
        public ActionResult New()
        {
            return View();
        }


        // POST: MedicalProcedure/Create
        [HttpPost]
        public ActionResult Create(MedicalProcedure MedicalProcedure)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(MedicalProcedure.MedicalProcedureName);
            //objective: add a new MedicalProcedure into our system using the API
            //curl -H "Content-Type:application/json" -d @MedicalProcedure.json https://localhost:44376/api/MedicalProceduredata/addMedicalProcedure 
            string url = "medicalproceduredata/addmedicalprocedure";


            string jsonpayload = jss.Serialize(MedicalProcedure);
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
        /// A function that grabs the details of the selected medicalprocedure and renders the edit form view
        /// </summary>
        /// <param name="id">The medicalprocedure to be edited</param>
        /// <returns>
        /// Returns the edit booking form page with the medicalprocedure data. 
        /// </returns>
        /// 

        // GET: MedicalProcedure/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the medicalprocedure information

            //objective: communicate with our medicalprocedure data api to retrieve one medicalprocedure
            //curl https://localhost:44324/api/medicalproceduredata/findmedicalprocedure/{id}

            string url = "MedicalProcedureData/FindMedicalProcedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MedicalProcedureDto selectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;

            return View(selectedMedicalProcedure);
        }

        /// <summary>
        /// A function to update the details of the selected medicalprocedure
        /// </summary>
        /// <param name="id">The medicalprocedure id</param>
        /// <param name="medicalprocedure">The medicalprocedure object</param>
        /// <returns>
        /// The list view page after updating the medicalprocedure details
        /// </returns>
        /// 

        // POST: MedicalProcedure/Update/5
        [HttpPost]
        public ActionResult Update(int id, MedicalProcedure medicalprocedure)
        {
            try
            {
                // Serialize into JSON
                // Send the request to the API

                string url = "MedicalProcedureData/UpdateMedicalProcedure/" + id;


                string jsonpayload = jss.Serialize(medicalprocedure);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/MedicalProcedureData/UpdateMedicalProcedure/{id}
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
        /// <param name="id">The medicalprocedure id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Booking/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "MedicalProcedureData/FindMedicalProcedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MedicalProcedureDto selectedMedicalProcedure = response.Content.ReadAsAsync<MedicalProcedureDto>().Result;
            return View(selectedMedicalProcedure);
        }

        /// <summary>
        /// Function to delete the selected medicalprocedure entry.
        /// </summary>
        /// <param name="id">The medicalprocedure id</param>
        /// <returns>
        /// The list of medicalprocedures if successful else, the error view page.
        /// </returns>

        // POST: MedicalProcedure/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "MedicalProcedureData/DeleteMedicalProcedure/" + id;
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