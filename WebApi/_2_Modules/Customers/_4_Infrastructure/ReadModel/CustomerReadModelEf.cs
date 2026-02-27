using Microsoft.EntityFrameworkCore;
using WebApi._2_Modules.Customers._1_Ports.Inbound;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._2_Modules.Customers._2_Application.Error;
using WebApi._2_Modules.Customers._2_Application.Mappings;
using WebApi._2_Modules.Customers._3_Domain.Entities;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
using WebApi._4_BuildingBlocks._3_Domain;
namespace WebApi._2_Modules.Customers._4_Infrastructure.ReadModel;

public sealed class CustomerReadModelEf(
   ICustomersDbContext customersDbContext
) : ICustomerReadModel {
   
   public async Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id,
      CancellationToken ct
   ) {
      var customerDto = await customersDbContext.Customers
         .AsNoTracking()
         .Where(c => c.Id == Id)       // filter by Id
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
