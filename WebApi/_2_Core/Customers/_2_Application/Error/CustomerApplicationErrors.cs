using WebApi._2_Core.BuildingBlocks._3_Domain.Enums;
using WebApi._2_Core.BuildingBlocks._3_Domain.Errors;
namespace WebApi._2_Core.Customers._2_Application.Error;

public static class CustomerApplicationErrors {
   
   public static readonly DomainErrors NotFound =
      new(
         ErrorCode.NotFound,
         Title: "Customer: Not found",
         Message: "No customer with the given id exists."
      );

   public static readonly DomainErrors EmailMustBeUnique =
      new(
         ErrorCode.Conflict,
         Title: "Email Must Be Unique",
         Message: "An employee with the given email address already exists."
      );
}