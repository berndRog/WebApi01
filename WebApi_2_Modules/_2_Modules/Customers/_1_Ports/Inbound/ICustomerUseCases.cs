using WebApi._2_Modules.BuildingBlocks._2_Application.Dtos;
using WebApi._2_Modules.BuildingBlocks._3_Domain;
using WebApi._2_Modules.Customers._2_Application.Dtos;
namespace WebApi._2_Modules.Customers._1_Ports.Inbound;

public interface ICustomerUseCases {
   public Task<Result<CustomerDto>> CreateAsync(
      string firstname,
      string lastname,
      string? companyName,
      string email,
      string? id = null,
      AddressDto? addressDto = null,
      CancellationToken ct = default
   );
}