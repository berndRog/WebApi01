using Microsoft.Extensions.Logging;
using WebApi._2_Modules.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Modules.BuildingBlocks._2_Application.Dtos;
using WebApi._2_Modules.BuildingBlocks._3_Domain;
using WebApi._2_Modules.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._2_Modules.Customers._2_Application.Error;
using WebApi._2_Modules.Customers._2_Application.Mappings;
using WebApi._2_Modules.Customers._3_Domain.Entities;
namespace WebApi._2_Modules.Customers._2_Application.UseCases;

public sealed class CustomerUcCreate(
   ICustomerRepository repository,
   IUnitOfWork unitOfWork,
   IClock clock,
   ILogger<CustomerUcCreate> logger
) {

   public async Task<Result<CustomerDto>> ExecuteAsync(
      CustomerDto customerDto,
      CancellationToken ct = default
   ) {
      var emailString = customerDto.Email;
      var addressDto = customerDto.AddressDto;
      
      // Validate email format and create EmailVo
      var resultEmail = EmailVo.Create(emailString);
      if (resultEmail.IsFailure)
         return Result<CustomerDto>.Failure(resultEmail.Error);
      var email = resultEmail.Value;
      
      // Check if email is unique (not used by another customer)
      if (await repository.FindByEmailAsync(email, ct) != null) {
         return Result<CustomerDto>.Failure(CustomerApplicationErrors.EmailMustBeUnique);
      }

      // Validate address if provided and create AddressVo
      AddressVo? addressVo = null;
      if(addressDto is not null) {
         var resultAddress = AddressVo.Create(
            street: addressDto.Street, 
            postalCode: addressDto.PostalCode, 
            city: addressDto.City, 
            country: addressDto.Country
         );
         if (resultAddress.IsFailure)
            return Result<CustomerDto>.Failure(resultAddress.Error);
         addressVo = resultAddress.Value;
      }
      
      // Create Customer entity using factory method
      var result = Customer.Create(
         firstname: customerDto.Firstname, 
         lastname: customerDto.Lastname,
         companyName: customerDto.CompanyName, 
         email: email,
         id: customerDto.Id.ToString(),
         createdAt: clock.UtcNow,
         address: addressVo
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