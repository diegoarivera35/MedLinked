using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedLinked.Models
{
    public class Accommodation
    {
        // What describes an Accommodation?

        // The primary key for this entity
        [Key]
        public int AccommodationID { get; set; }

        // The name of the accommodation
        public string AccommodationName { get; set; }

        // The address of the accommodation
        public string Address { get; set; }

        // The destination
        public string Destination { get; set; }

        // The departure date
        public DateTime Departure { get; set; }

        // The arrival/return date
        public DateTime Arrival { get; set; }

        // The cost for the accommodation
        public float AccommodationCost { get; set; }
    }

    /*
     * A simpler version of the accommodation class with simpler data.
     * Used to display the shared columns between two tables but should have an explicit (FKs) way of m-m relationship
     */
    public class AccommodationDto
    {
        // The primary key for this entity
        [Key]
        public int AccommodationID { get; set; }

        // The name of the accommodation
        public string AccommodationName { get; set; }

        // The address of the accommodation
        public string Address { get; set; }

        // The destination
        public string Destination { get; set; }

        // The departure date
        public DateTime Departure { get; set; }

        // The arrival/return date
        public DateTime Arrival { get; set; }

        // The cost for the accommodation
        public float AccommodationCost { get; set; }
    }
}