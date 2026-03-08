using Microsoft.Extensions.DependencyInjection;
using WebApi._2_Modules.Customers._1_Ports.Inbound;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._2_Application.UseCases;

namespace WebApi._2_Modules.Customers;

public static class DiCustomerModules {
   
   public static IServiceCollection AddCustomerModules(
      this IServiceCollection services
   ) {
      // =========================================================
      // Inbound ports (HTTP / UI)
      // =========================================================
      // Adapters
      
      // WriteModels = Use Cases
      services.AddScoped<CustomerUcCreate>();
      services.AddScoped<ICustomerUseCases, CustomerUseCases>();

      // =========================================================
      // Outbound ports
      // =========================================================
      
      return services;
   }
}