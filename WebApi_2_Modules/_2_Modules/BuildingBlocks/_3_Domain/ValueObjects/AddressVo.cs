using WebApi._2_Modules.BuildingBlocks._3_Domain.Errors;
namespace WebApi._2_Modules.BuildingBlocks._3_Domain.ValueObjects;

public sealed record AddressAlt(
   string Street,
   string PostalCode,
   string City,       
   string? Country = null 
);

// Address is a value object without identity.
// It is immutable and fully replaced on change.
public sealed record AddressVo {
   
   // Properties
   public string Street     { get; init; } = string.Empty;
   public string PostalCode { get; init; } = string.Empty;
   public string City       { get; init; } = string.Empty;
   public string? Country   { get; init; } 
   
   // private Ctor
   private AddressVo(
      string street,
      string postalCode,
      string city,
      string? country = null
   ) {
      Street = street;
      PostalCode = postalCode;
      City = city;
      Country = country;
   }

   //--- Static factory method -------------------------------------------------
   public static Result<AddressVo> Create(
      string street,
      string postalCode,
      string city,
      string? country = null
   ) {
      // Normalize input early
      street = street.Trim();
      postalCode = postalCode.Trim();
      city = city.Trim();
      country = country?.Trim();
      
      if (string.IsNullOrWhiteSpace(street))
         return Result<AddressVo>.Failure(CommonErrors.StreetIsRequired);
      
      if (string.IsNullOrWhiteSpace(postalCode))
         return Result<AddressVo>.Failure(CommonErrors.PostalCodeIsRequired);

      if (string.IsNullOrWhiteSpace(city))
         return Result<AddressVo>.Failure(CommonErrors.CityIsRequired);

      return Result<AddressVo>.Success(new AddressVo(street, postalCode, city, country));
   }
}
