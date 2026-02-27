using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._4_BuildingBlocks._3_Domain;
namespace WebApi._2_Modules.Customers._1_Ports.Inbound;

public interface ICustomerUseCases {
   public Task<Result<CustomerDto>> CreateAsync(
      string firstname,
      string lastname,
      string? companyName,
      string email,
      string? id = null,
      string? street = null,
      string? postalCode = null,
      string? city = null,
      string? country = null,
      CancellationToken ct = default
   );
}