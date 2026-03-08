using WebApi._2_Modules.BuildingBlocks._3_Domain;
using WebApi._2_Modules.Customers._2_Application.Dtos;
namespace WebApi._2_Modules.Customers._1_Ports.Outbound;

public interface ICustomerReadModel {
   
   Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id, 
      CancellationToken ct = default
   );

   Task<Result<IEnumerable<CustomerDto>>> SelectAllAsync(
      CancellationToken ct = default
   );
   
}