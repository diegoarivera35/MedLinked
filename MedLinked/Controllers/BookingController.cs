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

        // GET: Booking/Details/5
        public ActionResult Details(int id)
        {
            DetailsBooking ViewModel = new DetailsBooking();

            //objective: communicate with our Booking data api to retrieve one Booking
            //curl https://localhost:44376/api/Bookingdata/findbooking/{id}

            string url = "bookingdata/findBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto SelectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            ViewModel.SelectedBooking = SelectedBooking;

            //show all medicalprocedures under this booking
            url = "medicalproceduredata/listmedicalproceduresforbooking/" + id;
            response = client.GetAsync(url).Result;
            //IEnumerable<MedicalProcedureDto> KeptMedicalProcedures = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            //ViewModel.KeptMedicalProcedures = KeptMedicalProcedures;


            return View(ViewModel);
        }


        public ActionResult Error()
        {

            return View();
        }


        // GET: Booking/New
        public ActionResult New()
        {
            //information about all patients in the system.
            //GET api/patientsdata/listpatients

            string url = "PatientData/ListPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // import the view modal for patient list
            // Here, we are using the object for the view model
            // GET api/PatientData/ListPatients
            DetailsBooking CreateBookingViewModel = new DetailsBooking();

            IEnumerable<PatientDto> PatientsOptions = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            // The Patients object is taken from the DetailsBooking view model
            CreateBookingViewModel.Patients = PatientsOptions;


            // GET api/AccommodationData/ListAccommodations
            url = "AccommodationData/ListAccommodations";
            response = client.GetAsync(url).Result;
            IEnumerable<AccommodationDto> AccommodationsOptions = response.Content.ReadAsAsync<IEnumerable<AccommodationDto>>().Result;

            // The Accommodations object is taken from the DetailsBooking view model
            CreateBookingViewModel.Accommodations = AccommodationsOptions;


            // GET api/MedicalProcedureData/ListMedicalProcedures
            url = "MedicalProcedureData/ListMedicalProcedures";
            response = client.GetAsync(url).Result;
            IEnumerable<MedicalProcedureDto> MedicalProceduresOptions = response.Content.ReadAsAsync<IEnumerable<MedicalProcedureDto>>().Result;

            // The MedicalProcedures object is taken from the DetailsBooking view model
            CreateBookingViewModel.MedicalProcedures = MedicalProceduresOptions;


            // GET api/DoctorsData/ListDoctors
            url = "DoctorData/ListDoctors";
            response = client.GetAsync(url).Result;
            IEnumerable<DoctorDto> DoctorsOptions = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            CreateBookingViewModel.Doctors = DoctorsOptions;

            return View(CreateBookingViewModel);
        }


        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(Booking Booking)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Booking.BookingName);
            //objective: add a new Booking into our system using the API
            //curl -H "Content-Type:application/json" -d @Booking.json https://localhost:44376/api/Bookingdata/addBooking 
            string url = "bookingdata/addbooking";


            string jsonpayload = jss.Serialize(Booking);
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
        /// A function that grabs the details of the selected booking and renders the edit form view
        /// </summary>
        /// <param name="id">The booking to be edited</param>
        /// <returns>
        /// Returns the edit booking form page with the booking data. 
        /// </returns>
        /// 

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the booking information

            //objective: communicate with our booking data api to retrieve one booking
            //curl https://localhost:44324/api/bookingdata/findbooking/{id}

            string url = "BookingData/FindBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectedBooking);
        }

        /// <summary>
        /// A function to update the details of the selected booking
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <param name="booking">The booking object</param>
        /// <returns>
        /// The list view page after updating the booking details
        /// </returns>
        /// 

        // POST: Booking/Update/5
        [HttpPost]
        public ActionResult Update(int id, Booking booking)
        {
            try
            {
                // Serialize into JSON
                // Send the request to the API

                string url = "BookingData/UpdateBooking/" + id;


                string jsonpayload = jss.Serialize(booking);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/BookingData/UpdateBooking/{id}
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
        /// <param name="id">The booking id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Booking/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "BookingData/FindBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            return View(selectedBooking);
        }

        /// <summary>
        /// Function to delete the selected booking entry.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>
        /// The list of bookings if successful else, the error view page.
        /// </returns>

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "BookingData/DeleteBooking/" + id;
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