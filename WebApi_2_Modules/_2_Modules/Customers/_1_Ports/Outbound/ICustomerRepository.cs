using WebApi._2_Modules.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Modules.Customers._3_Domain.Entities;
namespace WebApi._2_Modules.Customers._1_Ports.Outbound;

public interface ICustomerRepository {
   
   Task<Customer?> FindByIdAsync(
      Guid customerId, 
      CancellationToken ct = default
   );
   
   Task<Customer?> FindByEmailAsync(
      EmailVo emailVo,
      CancellationToken ct = default
   );
   
   void Add(Customer customer);
   
}