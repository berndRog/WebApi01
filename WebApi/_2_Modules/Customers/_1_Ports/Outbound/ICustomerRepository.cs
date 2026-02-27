using WebApi._2_Modules.Customers._3_Domain.Entities;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
namespace WebApi._2_Modules.Customers._1_Ports.Outbound;

public interface ICustomerRepository {
   
   Task<Customer?> FindByIdAsync(
      Guid customerId, 
      CancellationToken ct = default
   );
   
   Task<Customer?> FindByEmailAsync(
      Email email,
      CancellationToken ct = default
   );
   
   void Add(Customer customer);
   
}