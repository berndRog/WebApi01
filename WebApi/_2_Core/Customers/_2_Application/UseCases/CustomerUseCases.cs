using System.Runtime.CompilerServices;
using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.Customers._1_Ports.Inbound;
using WebApi._2_Core.Customers._2_Application.Dtos;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._2_Core.Customers._2_Application.UseCases;


// UseCases Facade for Customer aggregate
internal class CustomerUseCases(
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