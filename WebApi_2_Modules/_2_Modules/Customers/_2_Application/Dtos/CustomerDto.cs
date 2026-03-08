using WebApi._2_Modules.BuildingBlocks._2_Application.Dtos;
namespace WebApi._2_Modules.Customers._2_Application.Dtos;

public sealed record CustomerDto(
   Guid Id,
   string Firstname,
   string Lastname,
   string? CompanyName,
   string Email, 
   AddressDto? AddressDto
);
