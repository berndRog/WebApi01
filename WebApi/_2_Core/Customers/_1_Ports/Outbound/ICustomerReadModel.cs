using WebApi._2_Core.BuildingBlocks;
using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.Customers._2_Application.Dtos;
namespace WebApi._2_Core.Customers._1_Ports.Outbound;

public interface ICustomerReadModel {
   
   // Load a customer aggregate by its identifier
   Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id, 
      CancellationToken ct = default
   );
   
   // Find customer by email address
   Task<Result<CustomerDto>> FindByEmailAsync(
      string email,
      CancellationToken ct = default
   );

   // Return all customers
   Task<Result<IReadOnlyList<CustomerDto>>> SelectAllAsync(
      CancellationToken ct
   );
   
   // Load all customers with SQL like displayName
   Task<Result<IReadOnlyList<CustomerDto>>> SelectByDisplayNameAsync(string displayName,
      CancellationToken ct = default);
   
}