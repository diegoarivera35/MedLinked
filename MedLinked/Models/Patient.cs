﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MedLinked.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string PatientGender { get; set; }

        public DateTime PatientDOB { get; set; }

        public string PatientNationality { get; set; }

        public string PatientEmail { get; set; }

        public int PatientPhone { get; set; }



        // Navigation property representing the collection of medical procedures associated with this patient
        // public ICollection<Booking> Bookings { get; set; }

    }


    public class PatientDto
    {
        public int PatientID { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string PatientGender { get; set; }

        public DateTime PatientDOB { get; set; }

        public string PatientNationality { get; set; }

        public string PatientEmail { get; set; }

        public int PatientPhone { get; set; }

    }
}