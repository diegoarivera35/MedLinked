﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedLinked.Models.ViewModels
{
    public class DetailsPatient
    {

        public PatientDto SelectedPatient { get; set; }
        // To display the related bookings in the Patient details page
        public IEnumerable<BookingDto> RelatedBookings { get; set; }

        //public IEnumerable<MedicalProcedureDto> KeptMedicalProcedures { get; set; }
    }
}