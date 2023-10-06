using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _personList; 
        private readonly ICountriesService _countriesService;



        public PersonService() {
            _personList = new List<Person>();
            _countriesService = new CountryService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            //Convert the Person object into PersonResponse type
            PersonResponse? personResponse = person.ToPersonResponse();

            //adding country name
            personResponse.Country = _countriesService.GetCountryById(person.CountryID)?.CountryName;

            return personResponse;
        }


        /// <summary>
        /// Add person to data
        /// </summary>
        /// <param name="personAddRequest"> person object to add</param>
        /// <returns> Return a PersonResponse object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public PersonResponse? AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if personAddRequest is null
            if (personAddRequest == null) throw new ArgumentNullException(nameof(PersonAddRequest));

            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            //Convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonID = Guid.NewGuid();

            //Add person object to persons list
            _personList.Add(person);

            //Convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);

        }
        /// <summary>
        /// Get all persons of the list
        /// </summary>
        /// <returns>Return a list of persons as PersonResponse type</returns>
        public List<PersonResponse> GetAllPersons()
        {
           return _personList.Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if(personID == null)
            {
                return null;
            }
            Person? person  = _personList.FirstOrDefault(person => person.PersonID == personID);
            if (person == null) return null;
            return ConvertPersonToPersonResponse(person);
        }
    }
}
