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


        // Inverse relation - usually done for denoting a m-m relationship
        // This is the (implicit way) of describing a bridging table
        /* 
         * In this way of creating a m-m relationship, there is no way of describing or adding columns
         * to the bridging table and the table only has the PKs from the related tables
         * 
         * Used explicit way of defining a m-m relationship
         */

        // A package has many bookings

        // public ICollection<Customer> Customers { get; set; }
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