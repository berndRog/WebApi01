using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WebApi._2_Core.BuildingBlocks;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._2_Application.Dtos;
using WebApi._2_Core.Customers._2_Application.Mappings;
using WebApi._2_Core.Customers._3_Domain.Errors;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.ReadModel;

internal sealed class CustomerReadModelEf(
   ICustomerDbContext customerDbContext
) : ICustomerReadModel {
   
   public async Task<Result<CustomerDto>> FindByIdAsync(
      Guid Id,
      CancellationToken ct
   ) {
      var customerDto = await customerDbContext.Customers
         .AsNoTracking()
         .Where(c => c.Id == Id) // filter by Id
         .Select(c => c.ToCustomerDto()) // project to CustomerDto (map)
         .SingleOrDefaultAsync(ct);

      return customerDto is null
         ? Result<CustomerDto>.Failure(CustomerErrors.NotFound)
         : Result<CustomerDto>.Success(customerDto);
   }

   public async Task<Result<CustomerDto>> FindByEmailAsync(
      string email, 
      CancellationToken ct = default
   ) {
      var result = EmailVo.Create(email);
      if (result.IsFailure)         
         return Result<CustomerDto>.Failure(result.Error);
      var emailVo = result.Value;
      
      var customerDto = await customerDbContext.Customers
         .AsNoTracking()
         .Where(c => c.EmailVo == emailVo)   // filter by email
         .Select(c => c.ToCustomerDto())     // map to CustomerDto
         .SingleOrDefaultAsync(ct);
      
      return customerDto is null
         ? Result<CustomerDto>.Failure(CustomerErrors.NotFound)
         : Result<CustomerDto>.Success(customerDto);
   }
   
   public async Task<Result<IReadOnlyList<CustomerDto>>> SelectByDisplayNameAsync(string displayName,
      CancellationToken ct = default) {
      var pattern = $"%{displayName}%";
      var customerDtos = await customerDbContext.Customers
         .Where(c =>
            EF.Functions.Like(
               c.CompanyName ?? c.Firstname + " " + c.Lastname,
               pattern))
         .Select(c => c.ToCustomerDto()) // map to CustomerDto
         .ToListAsync(ct);
      return Result<IReadOnlyList<CustomerDto>>.Success(customerDtos);
   }
   
   public async Task<Result<IReadOnlyList<CustomerDto>>> SelectAllAsync(
      CancellationToken ct
   ) {
      var customerDtos = await customerDbContext.Customers
         .AsNoTracking()
         .Select(c => c.ToCustomerDto()) // project to CustomerDto (map)
         .ToListAsync(ct);
      return Result<IReadOnlyList<CustomerDto>>.Success(customerDtos);
   }
}