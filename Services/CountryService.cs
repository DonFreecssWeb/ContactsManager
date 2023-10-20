using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountryService(bool initialize = true)
        {
            _countries = new List<Country>();
            if(initialize)
            {
                _countries.AddRange(new List<Country>() {
                    new Country()
                    {
                        CountryId = Guid.Parse("E6E22042-8354-41CF-8C07-14F223D565C9"),
                        CountryName = "USA"
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("8B5747AB-2E42-41E9-B511-5A14FE8436DF"),
                        CountryName = "PERU"
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("8B5540C6-B134-4096-B4CE-9E9232A42068"),
                        CountryName = "AUSTRALIA"
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("96F38C15-2146-489D-B46D-18F48EEE770C"),
                        CountryName = "CHINA"
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("B42D189A-AEE3-4541-BB84-4409B1398BA5"),
                        CountryName = "ECUADOR"
                    }
                });
            }
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            
            //Validation: countryAddRequest can't be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            //Validation: CountryName can't be null
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            //Validation: CountryName can't be duplicate
            if(_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country
            Country country = countryAddRequest.ToCountry();           

            //Generate CountryId
            country.CountryId = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }
 
        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? countryId)
        {
            if (countryId == null) return null;
            //Default is null
          Country? country_response_from_list =  _countries.FirstOrDefault(country => country.CountryId == countryId);
            if(country_response_from_list == null)
            {
                return null;
            }
            return country_response_from_list.ToCountryResponse();
        }
    }
}