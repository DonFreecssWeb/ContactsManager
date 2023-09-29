using NuGet.Frameworks;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountryService();
        }

        //When CountryAddRequest is null, it should ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }
        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsDuplicate()
        {
       
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }
        //When you supply proper country name, it should insert the country to the
        //existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {

            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            
            //Act
            CountryResponse? countryResponse = _countriesService.AddCountry(request);

            //Assert
           Assert.True(countryResponse.CountryId != Guid.Empty);
        }
    }
}
