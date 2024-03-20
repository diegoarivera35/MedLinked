using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedLinked.Models
{
    /// <summary>
    /// The bridging table for MedLinked
    /// </summary>
    public class Booking
    {
        // What describes a Booking?

        // The primary key for Booking table
        [Key]
        public int BookingID { get; set; }

        // The payment status of the booking made
        public string Status { get; set; }

        // The date of booking - it should be before the start and end date
        public DateTime BookingDate { get; set; }

        // The grand total - will be the total after Tax/VAT + Accommodation Cost + Medical Procedure Cost
        public float GrandTotal { get; set; }



        // The FK relationship

        // Creating a one-to-many relationship between Booking and other related tables (The explicit way)(uses one-many relationship)
        // As a Booking has only one procedure and accommodation for a patient.

        /*
         * Note: After a new table has been created and the foreign key relationship has been made, to run the commands
         * to create-database, we first need to clear the manually added rows in the table. Then, try updating the database 
         * again.
         */

        // The PK from the Patient table
        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        // To access the details of the patients from the Patient table
        // A virtual navigation property should be created
        // The idea being when a booking is checked, the selected patient is also accessible
        public virtual Patient Patient { get; set; }


        // The PK from the Doctors table
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }

        // To access the details of the doctors from the Doctor table
        // A virtual navigation property should be created
        public virtual Doctor Doctor { get; set; }


        // The PK from the MedicalProcedure table
        [ForeignKey("MedicalProcedure")]
        public int MedicalProcedureID { get; set; }

        // To access the details of the procedures from the MedicalProcedure table
        // A virtual navigation property should be created
        public virtual MedicalProcedure MedicalProcedure { get; set; }


        // The PK from the Accommodation table
        [ForeignKey("Accommodation")]
        public int AccommodationID { get; set; }

        // To access the details of the accommodation from the Accommodation table
        // A virtual navigation property should be created
        public virtual Accommodation Accommodation { get; set; }
    }

    /*
     * A simpler version of the booking class with simpler data.
     * Used to display the shared columns between two tables but should have an explicit (FKs) way of m-m relationship
     */
    public class BookingDto
    {
        [Key]
        // The PK of Booking table
        public int BookingID { get; set; }

        // The payment status
        public string Status { get; set; }

        // The date when the booking was done
        public DateTime BookingDate { get; set; }

        // The firstname for patient
        public string PatientFirstName { get; set; }

        // The lastname for patient
        public string PatientLastName { get; set; }

        // The doctor's firstname
        public string DoctorFirstName { get; set; }

        // The doctor's lastname
        public string DoctorLastName { get; set; }

        // The medical procedure name
        public string MedicalProcedureName { get; set; }

        // The date when the procedure is taking place
        public DateTime MedicalProcedureDate { get; set; }

        // The name of the accommodation
        public string AccommodationName { get; set; }

        // The departure date
        public DateTime Departure { get; set; }

        // The total after tax and VAT
        public float GrandTotal { get; set; }
    }
}