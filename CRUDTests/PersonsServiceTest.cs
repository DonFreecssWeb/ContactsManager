using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
namespace CRUDTests
{
    public class PersonsServiceTest        
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsServiceTest()
        {
            _personService = new PersonService();
            _countriesService = new CountryService();
        }
        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>            
            _personService.AddPerson(request)
            );
        }

        //When we supply a person name as null value, it should throw ArgumentException

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? request = new PersonAddRequest()
            {
                PersonaName = null,
            };    

            //Assert
            Assert.Throws<ArgumentException>(() =>
            _personService.AddPerson(request)
            );
        }

        //When we supply the proper person details, it should insert the person into the persons list
        //and it should return an object of PersonResponse with the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            
            //Arrange
            PersonAddRequest? request = new PersonAddRequest()
            {
                PersonaName = "Jorge",
                Address ="La Molina",
                DateOfBirth =  DateTime.Parse("25-05-1985"),
                Email = "jorge@gmail.com",
                Gender = GenderOptions.Male,
                CountryID = Guid.NewGuid(),
                ReceiveNewsLetters = true,
                
            };

            //Act 
            PersonResponse person_response_from_addPerson = _personService.AddPerson(request);
            List<PersonResponse> person_response_list_from_Get   = _personService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_addPerson.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_addPerson, person_response_list_from_Get);
        }
        #endregion

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? personResponse_fromm_Get =  _personService.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(personResponse_fromm_Get);
        }

        //If we supply a valid person id, it should return a personResponse object with details
        [Fact]
        public void GetPersonByPersonID_ValidPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                Address = "Jorge",
                DateOfBirth = DateTime.Parse("2023-05-10"),
                Email = "jorge@gmail.com",
                Gender = GenderOptions.Male,
                PersonaName = "Jorge",
                ReceiveNewsLetters = true,
                CountryID = countryResponse.CountryId
            };

            PersonResponse personResponse_from_add = _personService.AddPerson(personRequest);

            
             PersonResponse? personResponse_from_get = _personService.GetPersonByPersonID(personResponse_from_add.PersonID);
            
            //Assert
            Assert.Equal(personResponse_from_add, personResponse_from_get);
        }

        #endregion
    }
}
