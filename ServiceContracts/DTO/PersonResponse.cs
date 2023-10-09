using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most method of PersonService
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        
        public Guid? CountryID { get; set; }
        public string? Country {get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the curremct object data with the parameter object
        /// </summary>
        /// <param name="obj"> The PersonResponse Object to compare</param>
        /// <returns>True or False, indicating whether all person details are matched with the parameter object</returns>
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse other = (PersonResponse)obj;
            return PersonID == other.PersonID && PersonName == other.PersonName
                && Email == other.Email && DateOfBirth == other.DateOfBirth && Gender == other.Gender
                && CountryID == other.CountryID && Address == other.Address 
                && ReceiveNewsLetters == other.ReceiveNewsLetters;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"ID: {PersonID} Name: {PersonName} Email: {Email}  DateOfBirth: {DateOfBirth} Gender: {Gender}" +
                $"CountryID: {CountryID} Address: {Address} ReceibeNewLetters: {ReceiveNewsLetters}";
        }
    }
    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert Person object into PersonResponse object
        /// </summary>
        /// <param name="person"> A Person object</param>
        /// <returns> Return a PersonResponse object</returns>
        /// No countryName
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Address = person.Address,
                CountryID = person.CountryID,
                Gender = person.Gender,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null

            };
        }
    }
}
