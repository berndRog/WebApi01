using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._3_Infrastructure._2_Persistence.Database;
using WebApi._3_Infrastructure._2_Persistence.ReadModel;
using WebApi._3_Infrastructure._2_Persistence.Repositories;
using WebApi._3_Infrastructure._3_Security;
namespace WebApi._3_Infrastructure;

public static class DiInfrastructureModules {
   
   public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration
   ) {
      
      var connectionString = configuration.GetConnectionString("WebApiDb");
      Console.WriteLine("---> Using SQLite connection string: " + connectionString);
      
      services.AddDbContext<WebDbContext>(options =>
         options.UseSqlite(connectionString)
      );

      // BC Db Contexts
      services.AddScoped<ICustomerDbContext, CustomerDbContextEf>(); 
      
      // ReadModels
      services.AddScoped<ICustomerReadModel, CustomerReadModelEf>();   
      
      // Repositories
      services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();
      
      // Unit of Work
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      
      // IdentityGateway
      services.AddScoped<IIdentityGateway, IdentityGatewayHttpContext>();
      
      // Seed Database
      services.AddScoped<ISeedDatabase, SeedDatabase>();

      return services;
   }
}