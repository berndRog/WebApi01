using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._2_Modules.Customers._3_Domain.Entities;
namespace WebApi._2_Modules.Customers._2_Application.Mappings;

public static class OwnerMappings {
   public static CustomerDto ToCustomerDto(this Customer customer) => new(
      Id: customer.Id,
      Firstname: customer.Firstname,
      Lastname: customer.Lastname,
      CompanyName: customer.CompanyName,
      Email: customer.Email.Value,
      AddressDto: customer.Address?.ToAddressDto()
   );
}
