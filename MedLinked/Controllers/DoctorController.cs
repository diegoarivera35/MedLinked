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


            string url = "DoctorData/ListDoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DoctorDto> Doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            return View(Doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            DetailsDoctor ViewModel = new DetailsDoctor();

            //objective: communicate with our Doctor data api to retrieve one Doctor
            //curl https://localhost:44376/api/Doctordata/finddoctor/{id}

            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            Debug.WriteLine("Doctor received : ");
            Debug.WriteLine(SelectedDoctor.DoctorFirstName);

            ViewModel.SelectedDoctor = SelectedDoctor;

            //show all medicalprocedures under the care of this doctor
            url = "medicalproceduredata/listmedicalproceduresfordoctor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MedicalProcedureDto> KeptMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            ViewModel.KeptMedicalProcedures = KeptMedicalProcedures;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Doctor/New
        public ActionResult New()
        {
            return View();
        }


        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(Doctor Doctor)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Doctor.DoctorName);
            //objective: add a new Doctor into our system using the API
            //curl -H "Content-Type:application/json" -d @Doctor.json https://localhost:44376/api/Doctordata/addDoctor 
            string url = "DoctorData/AddDoctor";


            string jsonpayload = jss.Serialize(Doctor);
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
        /// A function that grabs the details of the selected doctor and renders the edit form view
        /// </summary>
        /// <param name="id">The doctor to be edited</param>
        /// <returns>
        /// Returns the edit booking form page with the doctor data. 
        /// </returns>
        /// 

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the doctor information

            //objective: communicate with our doctor data api to retrieve one doctor
            //curl https://localhost:44324/api/doctordata/finddoctor/{id}

            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DoctorDto selectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;

            return View(selectedDoctor);
        }

        /// <summary>
        /// A function to update the details of the selected doctor
        /// </summary>
        /// <param name="id">The doctor id</param>
        /// <param name="doctor">The doctor object</param>
        /// <returns>
        /// The list view page after updating the doctor details
        /// </returns>
        /// 

        // POST: Doctor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Doctor doctor)
        {
            try
            {
                // Serialize into JSON
                // Send the request to the API

                string url = "DoctorData/UpdateDoctor/" + id;


                string jsonpayload = jss.Serialize(doctor);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/DoctorData/UpdateDoctor/{id}
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
        /// <param name="id">The doctor id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Booking/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto selectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(selectedDoctor);
        }

        /// <summary>
        /// Function to delete the selected doctor entry.
        /// </summary>
        /// <param name="id">The doctor id</param>
        /// <returns>
        /// The list of doctors if successful else, the error view page.
        /// </returns>

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DoctorData/DeleteDoctor/" + id;
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