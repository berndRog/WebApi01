using WebApi._2_Modules.BuildingBlocks._3_Domain;
using WebApi._2_Modules.Customers._2_Application.Dtos;
namespace WebApi._2_Modules.Customers._1_Ports.Inbound;

public interface ICustomerUseCases {
   Task<Result<CustomerDto>> CreateAsync(
      CustomerDto customerDto,
      CancellationToken ct = default
   );
}