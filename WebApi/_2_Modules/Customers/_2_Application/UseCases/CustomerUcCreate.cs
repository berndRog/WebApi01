using BankingApi._4_BuildingBlocks._1_Ports.Inbound;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._2_Modules.Customers._2_Application.Error;
using WebApi._2_Modules.Customers._2_Application.Mappings;
using WebApi._2_Modules.Customers._3_Domain.Entities;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
using WebApi._4_BuildingBlocks._3_Domain;
namespace WebApi._2_Modules.Customers._2_Application.UseCases;

public sealed class CustomerUcCreate(
   ICustomerRepository repository,
   IUnitOfWork unitOfWork,
   ILogger<CustomerUcCreate> logger
) {

   public async Task<Result<CustomerDto>> ExecuteAsync(
      string firstname,
      string lastname,
      string? companyName,
      string email,
      string? id = null,
      string? street = null,
      string? postalCode = null,
      string? city = null,
      string? country = null,
      CancellationToken ct = default
   ) {

      var resultEmail = Email.Create(email);
      if (resultEmail.IsFailure)
         return Result<CustomerDto>.Failure(resultEmail.Error);
      var emailVo = resultEmail.Value;
      
      if (await repository.FindByEmailAsync(emailVo, ct) != null) {
         return Result<CustomerDto>.Failure(CustomerApplicationErrors.EmailMustBeUnique);
      }

      Address? addressVo = null;
      var resultAddress = Address.Create(
         street: street, 
         postalCode: postalCode, 
         city: city, 
         country: country
      );
      if (resultAddress.IsSuccess) addressVo = resultAddress.Value;
      
      var result = Customer.Create(
         //clock: clock,
         firstname: firstname, 
         lastname: lastname,
         companyName: companyName, 
         emailVo: emailVo,
         id: id,
         addressVo: addressVo
      );

      if (result.IsFailure)
         return Result<CustomerDto>.Failure(result.Error);
            // .LogIfFailure(logger, "CustomerUcCreate.DomainRejected",
            //    new { firstname, lastname, companyName, email, id, 
            //       street, postalCode, city, country });
      
      // Add owner to repository (tracked by EF)
      var customer = result.Value!;
      repository.Add(customer);
      
      // Save all changes to database using a transaction
      var savedRows = await unitOfWork.SaveAllChangesAsync("Create Customer(Person)", ct);
      logger.LogInformation("CustomerUcCreatePerson done customerId={id} savedRows={rows}",
         customer.Id, savedRows);

     
      return Result<CustomerDto>.Success(customer.ToCustomerDto());
   }
}