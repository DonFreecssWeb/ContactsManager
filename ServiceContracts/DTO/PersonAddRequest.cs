using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {        
        public string? PersonaName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Convert the current object of PersonAddRequest into Person object
        /// </summary>
        /// <returns> Return a Person object</returns>
        public Person ToPerson()
        {
            return new Person
            {
                PersonaName = PersonaName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
