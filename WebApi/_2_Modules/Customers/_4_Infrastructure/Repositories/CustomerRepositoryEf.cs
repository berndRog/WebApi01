using Microsoft.EntityFrameworkCore;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._3_Domain.Entities;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
namespace WebApi._2_Modules.Customers._4_Infrastructure.Repositories;

public class CustomerRepositoryEf(
   ICustomersDbContext customersDbContext
) : ICustomerRepository {

   public async Task<Customer?> FindByIdAsync(
      Guid customerId, 
      CancellationToken ct
   ) {
      return await customersDbContext.Customers
         .FirstOrDefaultAsync(o => o.Id == customerId, ct);
   }
   
   public async Task<Customer?> FindByEmailAsync(
      Email email,
      CancellationToken ct
   ) {
      return await customersDbContext.Customers
         .SingleOrDefaultAsync(c => c.EmailVo == email, ct);
   }
   
   public void Add(Customer customer) {
      customersDbContext.Add<Customer>(customer);
   }
}