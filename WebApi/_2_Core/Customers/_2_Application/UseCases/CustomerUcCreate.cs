using System.Runtime.CompilerServices;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._2_Application.Dtos;
using WebApi._2_Core.Customers._2_Application.Mappings;
using WebApi._2_Core.Customers._3_Domain;
using WebApi._2_Core.Customers._3_Domain.Entities;
using WebApi._2_Core.Customers._3_Domain.Errors;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._2_Core.Customers._2_Application.UseCases;

internal sealed class CustomerUcCreate(
   IIdentityGateway identityGateway,
   ICustomerRepository customerRepository,
   IUnitOfWork unitOfWork,
   IClock clock,
   ILogger<CustomerUcCreate> logger
) {

   public async Task<Result<CustomerDto>> ExecuteAsync(
      CustomerDto customerDto,
      CancellationToken ct = default
   ) {
      var email = customerDto.Email;
      var addressDto = customerDto.AddressDto;
      
      // 1) subject required
      var resultSubject = IdentitySubject.Check(identityGateway.Subject);
      if (resultSubject.IsFailure) 
         return Result<CustomerDto>.Failure(resultSubject.Error);
      var subject = resultSubject.Value;
      
      // 2) Check if email is unique (not used by another customer)
      var resultEmail = EmailVo.Create(customerDto.Email);
      if (resultEmail.IsFailure) 
         return Result<CustomerDto>.Failure(resultEmail.Error);
      var emailVo = resultEmail.Value;
      
      if (await customerRepository.FindByEmailAsync(emailVo, ct) != null)
         return Result<CustomerDto>.Failure(CustomerErrors.EmailMustBeUnique);
      
      // 3) Validate address if provided and create AddressVo
      var resultAddress = AddressVo.Create(
         street: addressDto.Street,
         postalCode: addressDto.PostalCode,
         city: addressDto.City,
         country: addressDto.Country
      );
      if (resultAddress.IsFailure)
         return Result<CustomerDto>.Failure(resultAddress.Error);
      var addressVo = resultAddress.Value;
      
      // 4) Create Customer entity using factory method
      var resultCustomer = Customer.Create(
         firstname: customerDto.Firstname,
         lastname: customerDto.Lastname,
         companyName: customerDto.CompanyName,
         emailVo: emailVo,
         subject: subject,
         addressVo: addressVo,
         id: customerDto.Id.ToString(),
         createdAt: clock.UtcNow
      );
      if (resultCustomer.IsFailure) 
         return Result<CustomerDto>.Failure(resultCustomer.Error);
      var customer = resultCustomer.Value;
      
      // 5) Add customer to repository (tracked by EF)
      customerRepository.Add(customer);
      
      // 6) Save all chaages to database
      var rows = await unitOfWork.SaveAllChangesAsync("Customer created", ct);
      logger.LogInformation("CustomerUcCreatePerson done customerId={id} savedRows={rows}",
         customer.Id, rows);

     
      return Result<CustomerDto>.Success(customer.ToCustomerDto());
   }
}