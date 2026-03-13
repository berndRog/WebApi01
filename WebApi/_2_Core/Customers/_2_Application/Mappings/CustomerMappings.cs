using WebApi._2_Core.BuildingBlocks._2_Application.Mappings;
using WebApi._2_Core.Customers._2_Application.Dtos;
using WebApi._2_Core.Customers._3_Domain.Entities;
namespace WebApi._2_Core.Customers._2_Application.Mappings;

public static class CustomerMappings {
   public static CustomerDto ToCustomerDto(this Customer customer) => new(
      Id: customer.Id,
      Firstname: customer.Firstname,
      Lastname: customer.Lastname,
      CompanyName: customer.CompanyName,
      EmailString: customer.EmailVo.Value,
      AddressDto: customer.AddressVo?.ToAddressDto()
   );
}
