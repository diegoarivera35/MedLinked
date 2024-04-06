using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedLinked.Models.ViewModels
{
    public class DetailsBooking
    {
        public BookingDto SelectedBooking { get; set; }

        // the AccommodationDto class
        public IEnumerable<AccommodationDto> Accommodations { get; set; }

        // the PatientDto class
        public IEnumerable<PatientDto> Patients { get; set; }

        // the DoctorDto class
        public IEnumerable<DoctorDto> Doctors { get; set; }

        // the MedicalProcedureDto class
        public IEnumerable<MedicalProcedureDto> MedicalProcedures { get; set; }
    }
}






