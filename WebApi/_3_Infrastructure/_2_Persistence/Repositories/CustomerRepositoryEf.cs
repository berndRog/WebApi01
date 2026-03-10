using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._3_Domain.Entities;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.Repositories;

internal class CustomerRepositoryEf(
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