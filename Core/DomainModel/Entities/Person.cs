namespace WebApi.Core.DomainModel.Entities; 

public class Person: AEntity {

   // properties with getter only
   public override Guid Id { get; init; } = Guid.NewGuid();
   public string FirstName { get; private set; } = string.Empty;
   public string LastName { get; private set; } = string.Empty;
   public string? Email { get; private set; } = null;
   public string? Phone { get; private set; } = null;
   public string? ImageUrl { get; private set; } = null;
   
   // EF Core requires a constructor
   public Person() {}
   public Person(Guid id, string firstName, string lastName, string? email = null,
                 string? phone = null, string? imageUrl = null) {
      Id = id;
      FirstName= firstName;
      LastName = lastName;
      Email = email;
      Phone = phone;
      ImageUrl = imageUrl;
   }   
   
   // method
   public void Set(string? email = null, string? phone = null, string? imageUrl = null) {
      if(email != null) Email = email;
      if(phone != null) Phone = phone;
      if(imageUrl != null) ImageUrl = imageUrl;
   } 
   
   public void Update(Person updPerson) {
      FirstName = updPerson.FirstName;
      LastName = updPerson.LastName;
      if(updPerson.Email != null) Email = updPerson.Email;
      if(updPerson.Phone != null) Phone = updPerson.Phone;
      if(updPerson.ImageUrl != null) ImageUrl = updPerson.ImageUrl;
   }
   
}