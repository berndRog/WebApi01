using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi._2_Modules.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._3_Infrastructure._2_Persistence.Database;
using WebApi._3_Infrastructure._2_Persistence.ReadModel;
using WebApi._3_Infrastructure._2_Persistence.Repositories;
namespace WebApi._3_Infrastructure;

public static class DiInfrastructureModules {
   
   public static IServiceCollection AddInfrastructureModules(
      this IServiceCollection services,
      IConfiguration configuration
   ) {
      
      var connectionString = configuration.GetConnectionString("WebApiDb");
      Console.WriteLine("---> Using SQLite connection string: " + connectionString);
      
      services.AddDbContext<WebDbContext>(options =>
         options.UseSqlite(connectionString)
      );

      // BC Db Contexts
      services.AddScoped<ICustomersDbContext, CustomersDbContextEf>(); 
      
      
      // ReadModels
      services.AddScoped<ICustomerReadModel, CustomerReadModelEf>();   
      
      // Repositories
      services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();
      
      // Unit of Work
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      
      // Seed Database
      services.AddScoped<ISeedDatabase, SeedDatabase>();

      return services;
   }
}