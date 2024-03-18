using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedLinked.Models
{
    public class MedicalProcedure
    {
        [Key]
        public int MedicalProcedureID { get; set; }

        public string MedicalProcedureName { get; set; }

        public string MedicalCenter { get; set; }

        public DateTime MedicalProcedureDate { get; set; }

        public float MedicalProcedureCost { get; set; }


    }

    public class MedicalProcedureDto
    {
        public int MedicalProcedureID { get; set; }
        public string MedicalProcedureName { get; set; }
        public string MedicalCenter { get; set; }
        public DateTime MedicalProcedureDate { get; set; }
        public float MedicalProcedureCost { get; set; }


    }

}