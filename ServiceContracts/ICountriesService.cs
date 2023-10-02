using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"> Country object to add</param>
        /// <returns>Returns the country object after adding it( including newly
        /// generate countryId) </returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Return all countries from the list
        /// </summary>
        /// <returns>List of CountryResponse</returns>
        List<CountryResponse> GetAllCountries();
        /// <summary>
        /// Return a country object based on the given country id
        /// </summary>
        /// <param name="id"> CountryId (Guid) to search</param>
        /// <returns>Matching Country as CountryResponse object</returns>
        CountryResponse GetCountryById(Guid? id);

    }
}