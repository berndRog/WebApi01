using WebApi._2_Modules.Customers._1_Ports.Inbound;
using WebApi._2_Modules.Customers._2_Application.Dtos;
using WebApi._4_BuildingBlocks._3_Domain;
namespace WebApi._2_Modules.Customers._2_Application.UseCases;


// UseCases Facade for Customer aggregate
public class CustomerUseCases(
   CustomerUcCreate createUc
): ICustomerUseCases {

   public Task<Result<CustomerDto>> CreateAsync(
      string firstname,
      string lastname,
      string? companyName,
      string emailString,
      string? id,
      string? street,
      string? postalCode,
      string? city,
      string? country,
      CancellationToken ct
   ) => createUc.ExecuteAsync(
      firstname: firstname, 
      lastname: lastname, 
      companyName: companyName, 
      email: emailString, 
      id: id, 
      street: street, 
      postalCode: postalCode, 
      city: city,
      country: country, 
      ct: ct
   );
}