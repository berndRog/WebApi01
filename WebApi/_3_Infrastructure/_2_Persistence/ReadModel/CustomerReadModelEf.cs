using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._2_Application.Dtos;
using WebApi._2_Core.Customers._2_Application.Error;
using WebApi._2_Core.Customers._2_Application.Mappings;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.ReadModel;

internal sealed class CustomerReadModelEf(
   ICustomersDbContext customersDbContext
) : ICustomerReadModel {
   
   public async Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id,
      CancellationToken ct
   ) {
      var customerDto = await customersDbContext.Customers
         .AsNoTracking()
         .Where(c => c.Id == Id)          // filter by Id
         .Select(c => c.ToCustomerDto())  // project to CustomerDto (map)
         .SingleOrDefaultAsync(ct);

      return customerDto is null
         ? Result<CustomerDto>.Failure(CustomerApplicationErrors.NotFound)
         : Result<CustomerDto>.Success(customerDto);
   }
   
   public async Task<Result<IEnumerable<CustomerDto>>> SelectAllAsync(
      CancellationToken ct
   ) {
      var customerDtos = await customersDbContext.Customers
         .AsNoTracking()
         .Select(c => c.ToCustomerDto()) // project to CustomerDto (map)
         .ToListAsync(ct);
      return Result<IEnumerable<CustomerDto>>.Success(customerDtos);
   }
}
