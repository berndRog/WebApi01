using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._3_Domain.Entities;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.Repositories;

internal class CustomerRepositoryEf(
   ICustomerDbContext customerDbContext
) : ICustomerRepository {
   public async Task<Customer?> FindByIdAsync(
      Guid id,
      CancellationToken ct
   ) {
      return await customerDbContext.Customers
         .FirstOrDefaultAsync(o => o.Id == id, ct);
   }

   public Task<Customer?> FindBySubjectAsync(
      string subject,
      CancellationToken ct = default
   ) {
      throw new NotImplementedException();
   }

   public async Task<Customer?> FindByEmailAsync(
      EmailVo email,
      CancellationToken ct
   ) {
      return await customerDbContext.Customers
         .SingleOrDefaultAsync(c => c.EmailVo == email, ct);
   }

   public async Task<IEnumerable<Customer>> SelectByNameAsync(
      string displayName,
      CancellationToken ct = default
   ) {
      var pattern = $"%{displayName}%";
      return await customerDbContext.Customers
         .Where(c =>
            EF.Functions.Like(
               c.CompanyName ?? c.Firstname + " " + c.Lastname,
               pattern))
         .ToListAsync(ct);
   }

   public async Task<bool> ExistsActiveAsync(
      Guid id,
      CancellationToken ct = default
   ) {
      return await customerDbContext.Customers
            .AsTracking()
            .FirstOrDefaultAsync(o => o.Id == id, ct)
         is { IsActive: true };
   }

   public async Task<IEnumerable<Customer>> SelectAllAsync(
      CancellationToken ct = default
   ) {
      return await customerDbContext.Customers
         .ToListAsync(ct);
   }

   public void Add(Customer customer) {
      customerDbContext.Add(customer);
   }

   public void Update(Customer customer) {
      customerDbContext.Update(customer);
   }
}