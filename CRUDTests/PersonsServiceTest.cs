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

        public PersonsServiceTest()
        {
            _personService = new PersonService();
        }
        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? request = null;

            //Act 
            PersonResponse personResponse = _personService.AddPerson(request);

            //Assert
            Assert.Throws<ArgumentNullException>(() => personResponse);
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

            //Act 
            PersonResponse personResponse = _personService.AddPerson(request);

            //Assert
            Assert.Throws<ArgumentException>(() => personResponse);
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
    }
}
