using Microsoft.AspNetCore.Mvc;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Controllers;

[ApiController]
[Route("carshop")]
//[Consumes("application/json")] default
//[Produces("application/json")] default
public class PeopleController(
   IPeopleRepository peopleRepository,
   IDataContext dataContext
   //ILogger<PersonController> logger
) : ControllerBase {
   
   // Get all people   http://localhost:5200/people
   [HttpGet("people")]  
   [ProducesResponseType(StatusCodes.Status200OK)]
   public ActionResult<IEnumerable<Person>> GetAll() {
      var people = peopleRepository.SelectAll();
      return Ok(people);
   }
   
   // Get person by Id http://localhost:5200/carshop/people/{id}
   [HttpGet("people/{id:guid}")]
   public ActionResult<Person> GetById(
      [FromRoute] Guid id
   ) {
      // switch(personRepository.FindById(id)) {
      //    case Person person:
      //       return Ok(person);
      //    case null:
      //       return NotFound("Owner with given Id not found");
      // };
      return peopleRepository.FindById(id) switch {
         Person person => Ok(person),
         null => NotFound("Owner with given Id not found")
      };
   }
   
   // Get person by name http://localhost:5200/carshop/people/name?name={name}
   [HttpGet("people/name")]
   public ActionResult<Person> GetByName(
      [FromQuery] string name
   ) {
      // switch(personRepository.FindById(id)) {
      //    case Person person:
      //       return Ok(person);
      //    case null:
      //       return NotFound("Owner with given Id not found");
      // };
      return peopleRepository.FindByName(name) switch {
         Person person => Ok(person),
         null => NotFound("Owner with given name not found")
      };
   }
   
   // Get person by email http://localhost:5200/carshop/people/email?email={email}
   [HttpGet("people/email")]
   public ActionResult<Person> GetByEmail(
      [FromQuery] string email
   ) {
      return peopleRepository.FindByEmail(email) switch {
         Person person => Ok(person),
         null => NotFound("Owner with given EMail not found")
      };
   }
   
   // Create a new person   http://localhost:5200/people
   [HttpPost("people")]  
   [ProducesResponseType(StatusCodes.Status201Created)]
   public ActionResult<Person> Create(
      [FromBody] Person person
   ) {
      peopleRepository.Add(person);
      dataContext.SaveAllChanges();
      
      return Created($"/people/{person.Id}", person);
   }
   
   // Update a person   http://localhost:5100/people/{id}
   [HttpPut("people/{id}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public IActionResult Update(
      Guid id,
      [FromBody] Person updPerson
   ) {
      var person = peopleRepository.FindById(id);
      if (person == null) {
         return NotFound();
      }
      // domain model
      person.Update(updPerson.FirstName, updPerson.LastName, updPerson.Email, updPerson.Phone);
      // repository
      peopleRepository.Update(person);
      dataContext.SaveAllChanges();
      
      return Ok(person);
   }

   // Delete a person   http://localhost:5100/people/{id}
   [HttpDelete("people/{id}")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   public IActionResult Delete(Guid id) {
      var person = peopleRepository.FindById(id);
      if (person == null) {
         return NotFound();
      }
      peopleRepository.Remove(person);
      dataContext.SaveAllChanges();
      return NoContent();
   }
}