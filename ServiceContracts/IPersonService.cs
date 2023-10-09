using System;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents the business logic for manipulating Person entity
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="request"> Person to add</param>
        /// <returns>Return the same person details, along with newly generated Id</returns>
        PersonResponse AddPerson(PersonAddRequest? request);
        /// <summary>
        /// Return all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type f</returns>
        List<PersonResponse> GetAllPersons();
        /// <summary>
        /// Get person by PersonID
        /// </summary>
        /// <param name="personID"> The ID to search the person</param>
        /// <returns>Return a PersonResponse object by matching the person id</returns>
        PersonResponse? GetPersonByPersonID(Guid? personID);
        /// <summary>
        /// return all persons objects that matches with given a search string
        /// </summary>
        /// <param name="searchBy"> The attribute to filter</param>
        /// <param name="searchString"> The person to search</param>
        /// <returns> Return all list if the search is empty  or the list of persons if the search matches.</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
    }
}
