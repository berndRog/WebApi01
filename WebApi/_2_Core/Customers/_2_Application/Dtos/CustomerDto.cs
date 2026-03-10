using WebApi._2_Core.BuildingBlocks._2_Application.Dtos;
namespace WebApi._2_Core.Customers._2_Application.Dtos;

public sealed record CustomerDto(
   Guid Id,
   string Firstname,
   string Lastname,
   string? CompanyName,
   string EmailString, 
   AddressDto? AddressDto
);
