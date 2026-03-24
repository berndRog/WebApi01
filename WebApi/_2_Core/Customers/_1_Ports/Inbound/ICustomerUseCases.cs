using WebApi._2_Core.BuildingBlocks;
using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.Customers._2_Application.Dtos;
namespace WebApi._2_Core.Customers._1_Ports.Inbound;

public interface ICustomerUseCases {
   
   // Create a fully initialized customer
   // And also create the first account
   Task<Result<CustomerDto>> CreateAsync(
      CustomerDto customerDto,
      // string? accountIdString,
      // string? ibanString,
      CancellationToken ct = default
   );
}