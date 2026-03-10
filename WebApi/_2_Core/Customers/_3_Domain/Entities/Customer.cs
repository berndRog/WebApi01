using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.BuildingBlocks._3_Domain.Entities;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._3_Domain.Errors;
namespace WebApi._2_Core.Customers._3_Domain.Entities;

public class Customer : AggregateRoot {
   
   //--- properties 
   // public Guid Id { get; private set; } is inherited from Entity base class
   public string Firstname { get; private set; } = string.Empty;
   public string Lastname { get; private set; } = string.Empty;
   public string? CompanyName { get; private set; } = null;
   
   // public string Email { get; private set; } = string.Empty;
   public EmailVo Email { get; private set; } = null!;
   
   public AddressVo? Address { get; private set; } = null;
   
   //--- ctors 
   private Customer() {
   }

   private Customer(
      Guid id,
      string firstname,
      string lastname,
      string? companyName,
      EmailVo email,
      AddressVo? address = null
   ) {
      Id = id;
      Firstname = firstname;
      Lastname = lastname;
      CompanyName = companyName;
      Email = email;
      Address= address;
   }
   
   // --- static factory to create a Customer object ---------------------------
   public static Result<Customer> Create(
      string firstname,
      string lastname,
      string? companyName,
      EmailVo email,
      string? id = null,
      DateTimeOffset createdAt = default!,
      AddressVo? address = null
   ) {
      // Normalize input early
      firstname = firstname.Trim();
      lastname = lastname.Trim();
      companyName = companyName?.Trim();
      
      // Validate input fields
      if (string.IsNullOrWhiteSpace(firstname))
         return Result<Customer>.Failure(CustomerErrors.FirstnameIsRequired);
      if (firstname.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidFirstname);

      if (string.IsNullOrWhiteSpace(lastname))
         return Result<Customer>.Failure(CustomerErrors.LastnameIsRequired);
      if (lastname.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidLastname);

      if (!string.IsNullOrWhiteSpace(companyName) && companyName.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidCompanyName);
      
      if (createdAt == default)
         return Result<Customer>.Failure(CustomerErrors.CreatedAtIsRequired);
      
      // Resolve an entity id from an optional raw string.
      var resultId = Resolve(id, CustomerErrors.InvalidId);
      if (resultId.IsFailure)
         return Result<Customer>.Failure(resultId.Error);
      var localId = resultId.Value;
      
      // Create the entity with validated and normalized input
      var customer = new Customer(
         id: localId,
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         email: email,
         address: address
      );
      
      // Initialize the entity's state 
      customer.Initialize(createdAt);

      return Result<Customer>.Success(customer);
   }
   
   // --- methods ---------------------------- ---------------------------------
   public Result UpdateAddress(
      AddressVo address,
      DateTimeOffset updatedAt = default
   ) {
      if (updatedAt == default)
         return Result.Failure(CustomerErrors.UpdatedAtIsRequiredInUpdateAddress);

      Address = address;
      Touch(updatedAt);
      return Result.Success();
   }
   
   public Result RemoveAddress(
      DateTimeOffset updatedAt = default
   ) {
      if (updatedAt == default)
         return Result.Failure(CustomerErrors.UpdatedAtIsRequiredInRemoveAddress);

      Address = null;
      Touch(updatedAt);
      return Result.Success();
   }
   
   public Result UpdateEmail(
      EmailVo email,
      DateTimeOffset updatedAt = default
   ) {
      if (updatedAt == default)
         return Result.Failure(CustomerErrors.UpdatedAtIsRequiredInUpdateEmail);
 
      Email = email;
      Touch(updatedAt);
      return Result.Success();
   }
   
   public string AsString() {
      var text = $"Vorname: {Firstname}, Nachname: {Lastname}\n" +
         $"Company: {CompanyName}\n" +
         $"E-Mail: {Email.Value}";
      if (Address != null)
         text += "\nAddress" +
                  $"\n{Address.Street}" +
                  $"\n{Address.PostalCode} {Address.City}";
      if(Address?.Country != null) 
         text +=  $"\n{Address.Country}";
      return text;
   }
}