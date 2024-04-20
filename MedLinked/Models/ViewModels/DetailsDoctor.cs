using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedLinked.Models.ViewModels
{
    public class DetailsDoctor
    {
        public DoctorDto SelectedDoctor { get; set; }
        // To display the medical procedures under this doctor
        public IEnumerable<MedicalProcedureDto> KeptMedicalProcedures { get; set; }
    }
}




