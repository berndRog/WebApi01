using Microsoft.EntityFrameworkCore;
using WebApi._2_Modules.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._3_Domain.Entities;
namespace WebApi._3_Infrastructure._2_Persistence.Repositories;

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
      EmailVo email,
      CancellationToken ct
   ) {
      return await customersDbContext.Customers
         .SingleOrDefaultAsync(c => c.Email == email, ct);
   }
   
   public void Add(Customer customer) {
      customersDbContext.Add<Customer>(customer);
   }
}