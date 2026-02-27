using WebApi._2_Modules.Customers._1_Ports.Inbound;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._2_Application.UseCases;
using WebApi._2_Modules.Customers._4_Infrastructure.ReadModel;
using WebApi._2_Modules.Customers._4_Infrastructure.Repositories;
namespace WebApi._2_Modules.Customers;

public static class DiCustomerModules {
   
   public static IServiceCollection AddCustomerModules(
      this IServiceCollection services
   ) {
      // =========================================================
      // Inbound ports (HTTP / UI)
      // =========================================================
      // Adapters

      // ReadModels
      services.AddScoped<ICustomerReadModel, CustomerReadModelEf>();      
      
      // WriteModels = Use Cases
      services.AddScoped<CustomerUcCreate>();
      services.AddScoped<ICustomerUseCases, CustomerUseCases>();

      // =========================================================
      // Outbound ports
      // =========================================================
      // Repositories
      services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();
      
      return services;
   }
}