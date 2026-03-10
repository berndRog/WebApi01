using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.Customers._2_Application.Dtos;
namespace WebApi._2_Core.Customers._1_Ports.Inbound;

public interface ICustomerUseCases {
   Task<Result<CustomerDto>> CreateAsync(
      CustomerDto customerDto,
      CancellationToken ct = default
   );
}