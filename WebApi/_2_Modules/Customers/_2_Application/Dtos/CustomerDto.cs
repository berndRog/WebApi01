namespace WebApi._2_Modules.Customers._2_Application.Dtos;

public sealed record CustomerDto(
   Guid Id,
   string Firstname,
   string Lastname,
   string? CompanyName,
   string Email, 
   string? Street,
   string? PostalCode,
   string? City,
   string? Country
);
