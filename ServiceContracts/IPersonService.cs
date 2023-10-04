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
    }
}
