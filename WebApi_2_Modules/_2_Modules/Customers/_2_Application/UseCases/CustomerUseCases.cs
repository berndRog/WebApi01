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
      CustomerDto customerDto,  
      CancellationToken ct
   ) => createUc.ExecuteAsync(
      customerDto: customerDto,
      ct: ct
   );
}