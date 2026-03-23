using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.Customers._2_Application.Dtos;
namespace WebApi._2_Core.Customers._1_Ports.Outbound;

public interface ICustomerReadModel {
   
   // Load a customer aggregate by its identifier
   Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id, 
      CancellationToken ct = default
   );
   
   // Load all customers with SQL like displayName
   Task<IEnumerable<CustomerDto>> SelectByNameAsync(
      string displayName,
      CancellationToken ct = default
   );

   // Select all customers
   Task<Result<IEnumerable<CustomerDto>>> SelectAllAsync(
      CancellationToken ct = default
   );
   
}