using System.Net.Mail;
using WebApi._2_Modules.Customers._3_Domain.Errors;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
using WebApi._4_BuildingBlocks._3_Domain;
using WebApi._4_BuildingBlocks._3_Domain.Entities;
using WebApi._4_BuildingBlocks._3_Domain.Errors;
namespace WebApi._2_Modules.Customers._3_Domain.Entities;

public class Customer : Entity {
   #region --- properties Getter/Setter ---------------------------------------------
   // public Guid Id { get; private set; } is inherited from Entity base class
   public string Firstname { get; private set; } = string.Empty;
   public string Lastname { get; private set; } = string.Empty;
   public string? CompanyName { get; private set; } = null;
   // public string Email { get; private set; } = string.Empty;
   public Email EmailVo { get; private set; } = null!;

   public Address? AddressVo { get; private set; } = null;
   #endregion

   #region--- ctors -----------------------------------------------------------------
   private Customer() {
   }

   private Customer(
      Guid id,
      string firstname,
      string lastname,
      string? companyName,
      // string email,
      Email emailVo,
      Address? addressVo = null
   ) {
      Id = id;
      Firstname = firstname;
      Lastname = lastname;
      CompanyName = companyName;
      EmailVo = emailVo;
      AddressVo = addressVo;
   }
   #endregion

   #region--- static factory to create a Customer object ----------------------------
   public static Result<Customer> Create(
      string firstname,
      string lastname,
      string? companyName,
      Email emailVo,
      string? id = null,
      Address? addressVo = null
   ) {
      // Validate input fields
      if (string.IsNullOrWhiteSpace(firstname))
         return Result<Customer>.Failure(CustomerErrors.FirstnameIsRequired);
      if (firstname.Length is < 2 or > 100)
         return Result<Customer>.Failure(CustomerErrors.InvalidFirstname);

      if (string.IsNullOrWhiteSpace(lastname))
         return Result<Customer>.Failure(CustomerErrors.LastnameIsRequired);
      if (lastname.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidLastname);

      if (!string.IsNullOrWhiteSpace(companyName) && companyName.Length is < 2 or > 100)
         return Result<Customer>.Failure(CustomerErrors.InvalidCompanyName);

      // email already tested in usecase
      // var resultEmail = ValidateEmail(email);
      // if (resultEmail.IsFailure)         
      //    return Result<Customer>.Failure(resultEmail.Error);
      
      // Resolve an entity id from an optional raw string.
      var resultId = Entity.Resolve(id, CustomerErrors.InvalidId);
      if (resultId.IsFailure)
         return Result<Customer>.Failure(resultId.Error);
      var localId = resultId.Value;

      // address is validated in useCase
      
      // Invariants: All validations passed, create and return the Customer entity
      // if (string.IsNullOrWhiteSpace(firstname) ||
      //     string.IsNullOrWhiteSpace(lastname) ||
      //     string.IsNullOrWhiteSpace(email))
      // {
      //    return Result.Failure(CustomerErrors.NameOrCompanyRequired);
      // }

      var customer = new Customer(
         id: localId,
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         emailVo: emailVo,
         addressVo: addressVo
      );

      return Result<Customer>.Success(customer);
   }
   #endregion

   #region--- methods ---------------------------- ------------------------------------
   public string AsString() {
      var text = $"Vorname: {Firstname}, Nachname: {Lastname}\n" +
         $"Company: {CompanyName}\n" +
         $"E-Mail: {EmailVo.Value}";
      if (AddressVo != null)
         text += "\nAddress" +
                  $"\n{AddressVo.Street}" +
                  $"\n{AddressVo.PostalCode} {AddressVo.City}";
      if(AddressVo?.Country != null) 
         text +=  $"\n{AddressVo.Country}";
      return text;
   }

   private static Result<string> ValidateEmail(string input) {
      if (string.IsNullOrWhiteSpace(input))
         return Result<string>.Failure(CommonErrors.InvalidEmail);

      var email = input.Trim().ToLowerInvariant();

      // RFC max length
      if (email.Length > 254)
         return Result<string>.Failure(CommonErrors.InvalidEmail);

      // Pragmatic syntax validation
      // (robust enough for real-world usage)
      try {
         // System.Net.Mail.MailAddress performs a reasonable syntax validation
         _ = new MailAddress(email);
      }
      catch {
         return Result<string>.Failure(CommonErrors.InvalidEmail);
      }
      return Result<string>.Success(email);
   }
}
#endregion