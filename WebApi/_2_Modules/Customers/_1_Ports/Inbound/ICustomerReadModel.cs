using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._4_BuildingBlocks._3_Domain;
namespace WebApi._2_Modules.Customers._1_Ports.Inbound;

public interface ICustomerReadModel {
   
   Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id, 
      CancellationToken ct = default
   );

   Task<Result<IEnumerable<CustomerDto>>> SelectAllAsync(
      CancellationToken ct = default
   );
   
}