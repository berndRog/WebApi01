using WebApi._2_Core.BuildingBlocks._2_Application.Dtos;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
namespace WebApi._2_Core.BuildingBlocks._2_Application.Mappings;

public static class AddressMappings {
   public static AddressDto ToAddressDto(this AddressVo address) => new(
      Street: address.Street,
      PostalCode: address.PostalCode,
      City: address.City,
      Country: address.Country
   );
}
