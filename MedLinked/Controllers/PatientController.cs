using MedLinked.Models;
using MedLinked.Models.ViewModels;
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






        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            DetailsPatient ViewModel = new DetailsPatient();

            //objective: communicate with our Patient data api to retrieve one Patient
            //curl https://localhost:44376/api/Patientdata/findpatient/{id}

            string url = "patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            Debug.WriteLine("Patient received : ");
            Debug.WriteLine(SelectedPatient.PatientFirstName);

            ViewModel.SelectedPatient = SelectedPatient;

            //show all medicalprocedures under the care of this patient
            url = "medicalproceduredata/listmedicalproceduresforpatient/" + id;
            response = client.GetAsync(url).Result;
            //IEnumerable<MedicalProcedureDto> KeptMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            //ViewModel.KeptMedicalProcedures = KeptMedicalProcedures;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }



        // GET: Patient/New
        public ActionResult New()
        {
            return View();
        }



        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient Patient)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Patient.PatientName);
            //objective: add a new Patient into our system using the API
            //curl -H "Content-Type:application/json" -d @Patient.json https://localhost:44376/api/Patientdata/addPatient 
            string url = "patientdata/addpatient";


            string jsonpayload = jss.Serialize(Patient);
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
        /// A function that grabs the details of the selected patient and renders the edit form view
        /// </summary>
        /// <param name="id">The patient to be edited</param>
        /// <returns>
        /// Returns the edit booking form page with the patient data. 
        /// </returns>
        /// 

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the patient information

            //objective: communicate with our patient data api to retrieve one patient
            //curl https://localhost:44324/api/patientdata/findpatient/{id}

            string url = "PatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(selectedPatient);
        }

        /// <summary>
        /// A function to update the details of the selected patient
        /// </summary>
        /// <param name="id">The patient id</param>
        /// <param name="patient">The patient object</param>
        /// <returns>
        /// The list view page after updating the patient details
        /// </returns>
        /// 

        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient patient)
        {
            try
            {
                // Serialize into JSON
                // Send the request to the API

                string url = "PatientData/UpdatePatient/" + id;


                string jsonpayload = jss.Serialize(patient);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/PatientData/UpdatePatient/{id}
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
        /// <param name="id">The patient id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Booking/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(selectedPatient);
        }

        /// <summary>
        /// Function to delete the selected patient entry.
        /// </summary>
        /// <param name="id">The patient id</param>
        /// <returns>
        /// The list of patients if successful else, the error view page.
        /// </returns>

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "PatientData/DeletePatient/" + id;
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