using WebApi._2_Modules.BuildingBlocks._2_Application.Dtos;
using WebApi._2_Modules.BuildingBlocks._3_Domain;
using WebApi._2_Modules.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Modules.Customers._1_Ports.Inbound;
using WebApi._2_Modules.Customers._2_Application.Dtos;
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
      AddressDto? addressDto,   
      CancellationToken ct
   ) => createUc.ExecuteAsync(
      firstname: firstname, 
      lastname: lastname, 
      companyName: companyName, 
      emailString: emailString, 
      id: id, 
      addressDto: addressDto, 
      ct: ct
   );
}