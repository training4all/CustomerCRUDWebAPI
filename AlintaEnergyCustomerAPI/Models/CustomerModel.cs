using System;
using System.ComponentModel.DataAnnotations;

namespace AlintaEnergyCustomerAPI.Models
{
    /// <summary>
    /// Represents a customer in system
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// unique id to differentiate customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// first name of customer
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// last name of customer
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// date of birth without time of customer
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
