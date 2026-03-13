using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.Customers._1_Ports.Inbound;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._2_Application.UseCases;
using WebApi._3_Infrastructure._2_Persistence;
using WebApi._3_Infrastructure._2_Persistence.Database;
using WebApi._3_Infrastructure._2_Persistence.Repositories;
using WebApi._3_Infrastructure._3_Security;
using WebApiTest._3_Infrastructure;

namespace WebApiTest;

public static class DiInfrastructureTestModule {
   
   public static IServiceCollection AddTestModules(
      this IServiceCollection services,
      DbConnection dbConnection,
      bool enableSensitiveDataLogging = true
   ) {
      services.AddSingleton(dbConnection);

      services.AddDbContext<WebDbContext>((sp, options) => {
         var connection = sp.GetRequiredService<DbConnection>();
         options.UseSqlite(connection);

         if (enableSensitiveDataLogging)
            options.EnableSensitiveDataLogging();
      });

      // BC Db Contexts
      services.AddScoped<ICustomerDbContext, CustomerDbContextEf>();

      // Repositories
      services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();

      // Unit of Work
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      // Clock 
      services.AddScoped<IClock, FakeClock>();
      
      // IdentityGateWay 
      services.AddScoped<IIdentityGateway,IdentityGatewayHttpContext>();
      
      //--- Core Use Cases -----------------------------------------------------
      services.AddScoped<CustomerUcCreate>();
      services.AddScoped<ICustomerUseCases, CustomerUseCases>();
      
      // Seed
      services.AddScoped<Seed>();
      services.AddScoped<TestSeed>();
      
      
      return services;
   }
}