using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _personList; 
        private readonly ICountriesService _countriesService;
        public PersonService(bool initialize = true) {
           
            _personList = new List<Person>();
            _countriesService = new CountryService();
           
            if(initialize)
            {
                _personList.Add(new Person()
                {
                    PersonID = Guid.Parse("603D0303-A54F-4F14-9838-5615ED9BE310"),
                    PersonName = "Jefferson",
                    Email = "jwaggett0@who.int",
                    DateOfBirth = DateTime.Parse("1996-11-22"),
                    Gender = "Male",
                    Address = "57 Fairfield Plaza",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("E6E22042-8354-41CF-8C07-14F223D565C9")
                });
                _personList.Add(new Person()
                {
                    PersonID = Guid.Parse("C825D6F0-8971-4A17-BED7-E9437D8B3C4E"),
                    PersonName = "Brendon",
                    Email = "basson1@businesswire.com",
                    DateOfBirth = DateTime.Parse("1992-01-28"),
                    Gender = "Male",
                    Address = "0 Trailsway Point",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("8B5747AB-2E42-41E9-B511-5A14FE8436DF")
                });
                _personList.Add(new Person()
                {
                    PersonID = Guid.Parse("{C7D9F733-5558-4055-A7A3-7F67409EDDA1}"),
                    PersonName = "Lenard",
                    Email = "lfitzsimmons2@xing.com",
                    DateOfBirth = DateTime.Parse("1999-08-20"),
                    Gender = "Male",
                    Address = "348 Mandrake Park",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("8B5540C6-B134-4096-B4CE-9E9232A42068")
                });
                _personList.Add(new Person()
                {
                    PersonID = Guid.Parse("8D3AC826-C908-4BDE-89CC-C807F6517EB0"),
                    PersonName = "Trista",
                    Email = "tbandt3@cnn.com",
                    DateOfBirth = DateTime.Parse("1995-09-26"),
                    Gender = "Female",
                    Address = "6466 Westport Place",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("8B5540C6-B134-4096-B4CE-9E9232A42068")
                });
                _personList.Add(new Person()
                {
                    PersonID = Guid.Parse("{14103039-D7CA-4F16-8034-A22939FCC1D1}"),
                    PersonName = "Woodie",
                    Email = "wellgood4@sciencedirect.com",
                    DateOfBirth = DateTime.Parse("1996-03-18"),
                    Gender = "Female",
                    Address = "48580 Norway Maple Place",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("96F38C15-2146-489D-B46D-18F48EEE770C")
                });

            }     

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

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(PersonUpdateRequest));
            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _personList.FirstOrDefault(person => person.PersonID == personUpdateRequest.PersonID);

            if (matchingPerson == null)
            {
                throw new ArgumentException("The given person id doesn't exist");
            }
            
            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.PersonID = personUpdateRequest.PersonID;

            return ConvertPersonToPersonResponse(matchingPerson);
            
        }
        public bool DeletePerson(Guid? personID)
        {

            if (personID == null)
                throw new ArgumentNullException(nameof(personID));

            Person? matchingPerson =  _personList.FirstOrDefault(person => person.PersonID == personID);

            if (matchingPerson == null) 
                return false;

            _personList.Remove(matchingPerson);
            return true;           

        }
    }
}
