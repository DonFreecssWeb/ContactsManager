using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using Entities;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService(false);
            _countriesService = new CountryService();
            _testOutputHelper = testOutputHelper;
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
                PersonName = null,
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
                PersonName = "Jorge",
                Address = "La Molina",
                DateOfBirth = DateTime.Parse("25-05-1985"),
                Email = "jorge@gmail.com",
                Gender = GenderOptions.Male,
                CountryID = Guid.NewGuid(),
                ReceiveNewsLetters = true,

            };

            //Act 
            PersonResponse person_response_from_addPerson = _personService.AddPerson(request);
            List<PersonResponse> person_response_list_from_Get = _personService.GetAllPersons();

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
            PersonResponse? personResponse_fromm_Get = _personService.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(personResponse_fromm_Get);
        }

        //If we supply a valid person id, it should return a personResponse object with details
        [Fact]
        public void GetPersonByPersonID_WithPersonID()
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
                PersonName = "Jorge",
                ReceiveNewsLetters = true,
                CountryID = countryResponse.CountryId
            };

            PersonResponse personResponse_from_add = _personService.AddPerson(personRequest);


            PersonResponse? personResponse_from_get = _personService.GetPersonByPersonID(personResponse_from_add.PersonID);

            //Assert
            Assert.Equal(personResponse_from_add, personResponse_from_get);
        }

        #endregion

        #region GetAllPersons
        //It Should return empty list by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

            //Assert
            Assert.Empty(persons_from_get);


        }

        //We add few persons and we should retrieve those persons as PersonResponse list from GetAllPersons()
        [Fact]
        public void GetAllPersons_GetProperList()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            //Act
            List<PersonAddRequest> personsRequest_list = new List<PersonAddRequest>()
            {
            new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2023-05-10"),
                            Email = "jorge@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Jorge",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },
             new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2000-01-20"),
                            Email = "Warren@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Warren",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },

        };

            List<PersonResponse> personResponses_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personsRequest_list)
            {

                personResponses_list_from_add.Add(_personService.AddPerson(personRequest));
            }

            //print personResponses_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in personResponses_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());

            }


            //Act
            List<PersonResponse> personResponses_list_from_get = _personService.GetAllPersons();

            //print personResponses_list_from_get
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_get in personResponses_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());

            }
            //Assert
            foreach (PersonResponse personResponse_expected in personResponses_list_from_add)
            {

                Assert.Contains(personResponse_expected, personResponses_list_from_get);
            }

        }
        #endregion

        #region GetFilteredPersons_EmptySearch
        //If the seach text is empty and search is by "PersonName", it should returl all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            //Act
            List<PersonAddRequest> personsRequest_list = new List<PersonAddRequest>()
            {
            new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2023-05-10"),
                            Email = "jorge@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Jorge",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },
             new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2000-01-20"),
                            Email = "Warren@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Warren",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },

        };

            List<PersonResponse> personResponses_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personsRequest_list)
            {

                personResponses_list_from_add.Add(_personService.AddPerson(personRequest));
            }

            //print personResponses_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in personResponses_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());

            }

            //Act
            List<PersonResponse> personResponses_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print personResponses_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in personResponses_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //Assert
            foreach (PersonResponse personResponse_from_add in personResponses_list_from_add)
            {
                Assert.Contains(personResponse_from_add, personResponses_list_from_search);
            }
        }

        //We  will add few persons and then we will seach base person name with some search string. It should return
        //a list of persons that matches
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            //Act
            List<PersonAddRequest> personsRequest_list = new List<PersonAddRequest>()
            {
            new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2023-05-10"),
                            Email = "Anita@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Anita",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },
             new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2000-01-20"),
                            Email = "Warren@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Warren",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },
                    new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2000-01-20"),
                            Email = "yoana@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Yoana",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },

        };

            List<PersonResponse> personResponses_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personsRequest_list)
            {

                personResponses_list_from_add.Add(_personService.AddPerson(personRequest));
            }

            //print personResponses_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in personResponses_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());

            }

            //Act
            List<PersonResponse> personResponses_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "an");

            //print personResponses_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in personResponses_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //Assert
            foreach (PersonResponse personResponse_from_add in personResponses_list_from_add)
            {
                if (personResponse_from_add.PersonName != null)
                {
                    if (personResponse_from_add.PersonName.Contains("na", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponse_from_add, personResponses_list_from_search);
                    }
                }

            }
        }
        #endregion

        #region GetSortedPersons
        // When we sort based in personName, it should return a list of PersonResponse in descending order
        [Fact]
        public void GetSortedPersons()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            //Act
            List<PersonAddRequest> personsRequest_list = new List<PersonAddRequest>()
            {
            new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2023-05-10"),
                            Email = "Anita@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Anita",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },
             new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2000-01-20"),
                            Email = "Warren@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Warren",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },
                    new PersonAddRequest()
                        {
                            Address = "isil",
                            DateOfBirth = DateTime.Parse("2000-01-20"),
                            Email = "yoana@gmail.com",
                            Gender = GenderOptions.Male,
                            PersonName = "Yoana",
                            ReceiveNewsLetters = true,
                            CountryID = countryResponse.CountryId
                        },

        };

            List<PersonResponse> personResponses_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personsRequest_list)
            {

                personResponses_list_from_add.Add(_personService.AddPerson(personRequest));
            }

            //print personResponses_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in personResponses_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());

            }

            //Act
            List<PersonResponse> personResponses_list_from_sort = _personService.GetSortedPersons(personResponses_list_from_add, nameof(Person.PersonName), SortOrderOptions.Descending);

            //print personResponses_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in personResponses_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            personResponses_list_from_add = personResponses_list_from_add.OrderByDescending(person => person.PersonName).ToList();            //Assert

            //Assert
            for (int i = 0; i < personResponses_list_from_add.Count; i++)
            {
                Assert.Equal(personResponses_list_from_add[i], personResponses_list_from_sort[i]);
            }
        }
        #endregion

        #region UpdatePerson
        //If we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange           

            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => 
            
            //Act
            _personService.UpdatePerson(personUpdateRequest)
            );            
        }

        //If we supply invalid person id, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonID() {

            //Arrange

            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest()
            {
                PersonID = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
               //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //When person name is null, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Jorge",
                CountryID = countryResponse.CountryId,
                Email = "Jorge@gmail.com",
                Address = "abc",
                Gender = GenderOptions.Male
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse_from_add.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //First add a new person and try to update the person name and email
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Jorge",
                Email = "Jorge@gmail.com",
                Address = "abc",
                DateOfBirth = DateTime.Parse("1987-05-05"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = countryResponse.CountryId              
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Warren";
            personUpdateRequest.Email = "Warren@gmail.com";
 

            //Act
            PersonResponse person_response_from_update = _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_update.PersonID);
            //Assert
            Assert.Equal(person_response_from_get, person_response_from_update);
            
        }

        #endregion

        #region DeletePerson
        //If we supply an valid person id, it should return true
        [Fact]
       public void DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Jorge",
                Email = "jorge@gmail.com",
                DateOfBirth = DateTime.Parse("2000-05-15"),
                Address = "abc",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = countryResponse.CountryId                              
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            //Act
            _testOutputHelper.WriteLine("Persons before to delete:");
            _testOutputHelper.WriteLine(_personService.GetAllPersons().Count.ToString());
            bool isDeleted = _personService.DeletePerson(personResponse_from_add.PersonID);
            _testOutputHelper.WriteLine("Persons after to delete:");
            _testOutputHelper.WriteLine(_personService.GetAllPersons().Count.ToString());

            //Assert
            Assert.True(isDeleted);    
        }

        //If we supply an valid person id, it should return true
        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Peru"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Jorge",
                Email = "jorge@gmail.com",
                DateOfBirth = DateTime.Parse("2000-05-15"),
                Address = "abc",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = countryResponse.CountryId
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            Guid newPersonID = Guid.NewGuid();
            //Act
            _testOutputHelper.WriteLine("Persons before to delete:");
            _testOutputHelper.WriteLine(_personService.GetAllPersons().Count.ToString());
            bool isDeleted = _personService.DeletePerson(newPersonID);
            _testOutputHelper.WriteLine("Persons after to delete:");
            _testOutputHelper.WriteLine(_personService.GetAllPersons().Count.ToString());
            //Assert
            Assert.False(isDeleted);
        }
        #endregion

    }
}