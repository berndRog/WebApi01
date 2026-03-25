using WebApi._2_Core.Customers._1_Ports.Inbound;
using WebApi._2_Core.Customers._2_Application.UseCases;
namespace WebApi._2_Core.Customers;

public static class DiCustomer {
   
   public static IServiceCollection AddCustomerModules(
      this IServiceCollection services
   ) {
      // Inbound ports (HTTP / UI)
      // WriteModels = Use Cases
      services.AddScoped<CustomerUcCreate>();
      services.AddScoped<ICustomerUseCases, CustomerUseCases>();
      return services;
   }
}