using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> getAllPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = getAllPersons;
            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchString))
            {
                return GetAllPersons();
            }
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    {
                        matchingPersons = getAllPersons.Where(person => (!string.IsNullOrEmpty(person.PersonName)) ?
                        person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }
                case nameof(Person.Email):
                    {
                        matchingPersons = getAllPersons.Where(person => (!string.IsNullOrEmpty(person.Email)) ?
                        person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }
                case nameof(Person.DateOfBirth):
                    {
                        matchingPersons = getAllPersons.Where(person => (person.DateOfBirth != null) ?
                        person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }
                case nameof(Person.Gender):
                    {
                        matchingPersons = getAllPersons.Where(person => (!string.IsNullOrEmpty(person.Gender)) ?
                        person.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }
                case nameof(Person.CountryID):
                    {
                        matchingPersons = getAllPersons.Where(person => (!string.IsNullOrEmpty(person.Country)) ?
                        person.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }
                case nameof(Person.Address):
                    {
                        matchingPersons = getAllPersons.Where(person => (!string.IsNullOrEmpty(person.Address)) ?
                        person.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }

                default:
                    {
                        matchingPersons = getAllPersons;
                        break;
                    }
            }
            
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }
            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
                    {

                        (nameof(PersonResponse.PersonName),SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.PersonName,StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.PersonName), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.Email), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.Email), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.Address), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.Address), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.DateOfBirth).ToList(),
                        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.DateOfBirth).ToList(),
                        (nameof(PersonResponse.Gender), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.Gender), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.ReceiveNewsLetters).ToList(),
                        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.ReceiveNewsLetters).ToList(),
                        (nameof(PersonResponse.Age), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.Age).ToList(),
                        (nameof(PersonResponse.Age), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.Age).ToList(),
                        (nameof(PersonResponse.Country), SortOrderOptions.Ascending) => allPersons.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                        (nameof(PersonResponse.Country), SortOrderOptions.Descending) => allPersons.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                        _ => allPersons
                    };
           
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? request)
        {
            throw new NotImplementedException();
        }
    }
}
