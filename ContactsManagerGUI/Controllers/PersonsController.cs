using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactsManagerGUI.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.Ascending)
        {           
           //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person name" },
                {nameof(PersonResponse.Email),"Email" },
                {nameof(PersonResponse.Age),"Age" },
                {nameof(PersonResponse.Address),"Address" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.Country),"Country" },
                {nameof(PersonResponse.DateOfBirth),"Date of birth" },
                {nameof(PersonResponse.ReceiveNewsLetters),"Receive News Letters" }
            };
           //if searchString is null or empty, it return all persons 
           List<PersonResponse> person_list = _personService.GetFilteredPersons(searchBy, searchString);

            //Persistence of values fields
            ViewBag.currentSearchBy = searchBy; 
            ViewBag.currentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = _personService.GetSortedPersons(person_list, sortBy, sortOrder);
            ViewBag.currentSortBy = sortBy;
            ViewBag.currentSortOrder = sortOrder;
            return View(sortedPersons);
        }
    }
}
